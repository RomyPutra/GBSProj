
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
#Region "WAC_WASLABEL Class"
    Public NotInheritable Class WAC_WASLABEL
        Inherits Core.CoreControl
        Private Wac_waslabelInfo As Wac_waslabelInfo = New Wac_waslabelInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        
        'ADD
        Private Function Save(ByVal ListContWACHdr As List(Of Container.Wac_waslabel), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, ByRef WasCode As String) As Boolean
            Dim strSQL As String = ""
            Dim ListSQL As ArrayList = New ArrayList()
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim BaseID As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ListContWACHdr Is Nothing And ListContWACHdr.Count <= 0 Then
                    'Message return
                Else
                    StartSQLControl()
                    strSQL = "DELETE Wac_waslabel WITH (ROWLOCK) WHERE WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasCode) & "'"
                    ListSQL.Add(strSQL)
                    For Each Wac_waslabelCont In ListContWACHdr
                        blnExec = False
                        blnFound = False
                        blnFlag = False
                        If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                            StartSQLControl()
                            With Wac_waslabelInfo.MyInfo
                                strSQL = BuildSelect(.CheckFields, .TableName, "BaseID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_waslabelCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_waslabelCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_waslabelCont.WasType) & "' AND LabelID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_waslabelCont.LabelID) & "'")
                            End With
                            rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                            blnExec = True

                            If rdr Is Nothing = False Then
                                With rdr
                                    If .Read Then
                                        blnFound = True
                                        If Convert.ToInt16(.Item("Active")) = 1 Then
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
                            End If
                        End If
                        ListSQL.Add(strSQL)
                    Next

                    Try
                        'execute
                        objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
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
                        Gibraltar.Agent.Log.Error("WAC_WASLABEL", axExecute.Message & sqlStatement, axExecute.StackTrace)
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
                ListContWACHdr = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
            Return True
        End Function

        Public Function Insert(ByVal ListContWACHdr As List(Of Container.Wac_waslabel), ByRef message As String, ByVal WasCode As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ListContWACHdr, SQLControl.EnumSQLType.stInsert, message, WasCode)
        End Function

        'AMEND
        Public Function Update(ByVal ListContWACHdr As List(Of Container.Wac_waslabel), ByRef message As String, ByVal WasCode As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ListContWACHdr, SQLControl.EnumSQLType.stUpdate, message, WasCode)
        End Function

        Public Function Delete(ByVal Wac_waslabelCont As Container.Wac_waslabel, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Wac_waslabelCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_waslabelInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BaseID = '" & Wac_waslabelCont.BaseID & "' AND WasCode = '" & Wac_waslabelCont.WasCode & "' AND WasType = '" & Wac_waslabelCont.WasType & "' AND LabelID = '" & Wac_waslabelCont.LabelID & "'")
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
                                strSQL = BuildUpdate(Wac_waslabelInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & Wac_waslabelCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_waslabelCont.UpdateBy) & "' WHERE" & _
                                "BaseID = '" & Wac_waslabelCont.BaseID & "' AND WasCode = '" & Wac_waslabelCont.WasCode & "' AND WasType = '" & Wac_waslabelCont.WasType & "' AND LabelID = '" & Wac_waslabelCont.LabelID & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Wac_waslabelInfo.MyInfo.TableName, "BaseID = '" & Wac_waslabelCont.BaseID & "' AND WasCode = '" & Wac_waslabelCont.WasCode & "' AND WasType = '" & Wac_waslabelCont.WasType & "' AND LabelID = '" & Wac_waslabelCont.LabelID & "'")
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
                Wac_waslabelCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetWAC_WASLABEL(ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String, ByVal LabelID As System.String) As Container.Wac_waslabel
            Dim rWac_waslabel As Container.Wac_waslabel = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Wac_waslabelInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "BaseID = '" & BaseID & "' AND WasCode = '" & WasCode & "' AND WasType = '" & WasType & "' AND LabelID = '" & LabelID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_waslabel = New Container.Wac_waslabel
                                rWac_waslabel.BaseID = drRow.Item("BaseID")
                                rWac_waslabel.WasCode = drRow.Item("WasCode")
                                rWac_waslabel.WasType = drRow.Item("WasType")
                                rWac_waslabel.LabelID = drRow.Item("LabelID")
                                If Not IsDBNull(drRow.Item("EffectDate")) Then
                                    rWac_waslabel.EffectDate = drRow.Item("EffectDate")
                                End If
                                rWac_waslabel.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rWac_waslabel.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rWac_waslabel.UpdateBy = drRow.Item("UpdateBy")
                                rWac_waslabel.Active = drRow.Item("Active")
                                rWac_waslabel.rowguid = drRow.Item("rowguid")
                                If Not IsDBNull(drRow.Item("SyncCreate")) Then
                                    rWac_waslabel.SyncCreate = drRow.Item("SyncCreate")
                                End If
                                If Not IsDBNull(drRow.Item("SyncLastUpd")) Then
                                    rWac_waslabel.SyncLastUpd = drRow.Item("SyncLastUpd")
                                End If
                                rWac_waslabel.LastSyncBy = drRow.Item("LastSyncBy")
                                rWac_waslabel.IsHost = drRow.Item("IsHost")
                            Else
                                rWac_waslabel = Nothing
                            End If
                        Else
                            rWac_waslabel = Nothing
                        End If
                    End With
                End If
                Return rWac_waslabel
            Catch ex As Exception
                Throw ex
            Finally
                rWac_waslabel = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_WASLABEL(ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String, ByVal LabelID As System.String, DecendingOrder As Boolean) As List(Of Container.Wac_waslabel)
            Dim rWac_waslabel As Container.Wac_waslabel = Nothing
            Dim lstWac_waslabel As List(Of Container.Wac_waslabel) = New List(Of Container.Wac_waslabel)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Wac_waslabelInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by BaseID, WasCode, WasType, LabelID DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "BaseID = '" & BaseID & "' AND WasCode = '" & WasCode & "' AND WasType = '" & WasType & "' AND LabelID = '" & LabelID & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_waslabel = New Container.Wac_waslabel
                                rWac_waslabel.BaseID = drRow.Item("BaseID")
                                rWac_waslabel.WasCode = drRow.Item("WasCode")
                                rWac_waslabel.WasType = drRow.Item("WasType")
                                rWac_waslabel.LabelID = drRow.Item("LabelID")
                                rWac_waslabel.CreateBy = drRow.Item("CreateBy")
                                rWac_waslabel.UpdateBy = drRow.Item("UpdateBy")
                                rWac_waslabel.Active = drRow.Item("Active")
                                rWac_waslabel.rowguid = drRow.Item("rowguid")
                                rWac_waslabel.LastSyncBy = drRow.Item("LastSyncBy")
                                rWac_waslabel.IsHost = drRow.Item("IsHost")
                                lstWac_waslabel.Add(rWac_waslabel)
                            Next

                        Else
                            rWac_waslabel = Nothing
                        End If
                        Return lstWac_waslabel
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWac_waslabel = Nothing
                lstWac_waslabel = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetLabelListByWasteCodeWasteType(ByVal BaseID As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_waslabelInfo.MyInfo
                    strSQL = "select b.LabelPath1, b.LabelPath2, b.LabelPath3, w.*" & _
                        " from wac_waslabel w INNER JOIN wac_baselabel b on w.LabelID = b.LabelID" & _
                        " where baseid='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, BaseID) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetLabelListByWasteCode(ByVal WasteCode As String, Optional ByVal LabelCode As String = "") As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_waslabelInfo.MyInfo
                    strSQL = "select b.Description, b.LabelCode, b.LabelPath1, b.LabelPath2, b.LabelPath3, " & _
                        " w.WasCode, CASE WHEN w.WasType='0' THEN 'Solid' WHEN w.WasType='1' THEN 'Sludge' WHEN w.WasType='2' THEN 'Liquid' WHEN w.WasType='3' THEN 'Gas' END AS WasType, w.WasType AS WasteType,w.BaseID, w.LabelID, w.SyncCreate " & _
                        " from wac_waslabel w INNER JOIN wac_baselabel b on w.LabelID = b.LabelID" & _
                        " where w.WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'"
                    If LabelCode <> "" Then
                        strSQL &= " AND b.LabelCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LabelCode) & "'"
                    End If

                    strSQL &= " Order By SyncCreate DESC"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetLabelListItem(Optional ByVal ItemCode As String = "", Optional ByVal Status As String = "") As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_waslabelInfo.MyInfo
                    strSQL = "SELECT DISTINCT IT.ItemCode, IT.ItmDesc, case WHEN WAC.WasCode IS NULL THEN 'Pending Assigned' ELSE 'Assigned' END As ComponentStatus FROM ITEM IT WITH (NOLOCK) LEFT JOIN WAC_WASLABEL WAC WITH (NOLOCK) ON IT.ItemCode=WAC.WasCode WHERE FLAG <> 2 "
                    If Status <> "" Then
                        If Status = "1" Then
                            strSQL &= " AND WAC.WasCode is null"
                        Else
                            strSQL &= " AND WAC.WasCode is not null"
                        End If
                    End If
                    If ItemCode <> "" Then
                        strSQL &= " AND IT.ItemCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "'"
                    End If
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
#Region "Wac_waslabel Container"
        Public Class Wac_waslabel_FieldName
            Public BaseID As System.String = "BaseID"
            Public WasCode As System.String = "WasCode"
            Public WasType As System.String = "WasType"
            Public LabelID As System.String = "LabelID"
            Public EffectDate As System.String = "EffectDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public Active As System.String = "Active"
            Public rowguid As System.String = "rowguid"
            Public SyncCreate As System.String = "SyncCreate"
            Public SyncLastUpd As System.String = "SyncLastUpd"
            Public LastSyncBy As System.String = "LastSyncBy"
            Public IsHost As System.String = "IsHost"
        End Class

        Public Class Wac_waslabel
            Protected _BaseID As System.String
            Protected _WasCode As System.String
            Protected _WasType As System.String
            Protected _LabelID As System.String
            Private _EffectDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String
            Private _IsHost As System.Byte

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
            ''' Mandatory
            ''' </summary>
            Public Property LabelID As System.String
                Get
                    Return _LabelID
                End Get
                Set(ByVal Value As System.String)
                    _LabelID = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property EffectDate As System.DateTime
                Get
                    Return _EffectDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _EffectDate = Value
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

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Wac_waslabel Info"
    Public Class Wac_waslabelInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "BaseID,WasCode,WasType,LabelID,EffectDate,CreateBy,LastUpdate,UpdateBy,Active,rowguid,SyncCreate,SyncLastUpd,LastSyncBy,IsHost"
                .CheckFields = "Active,IsHost"
                .TableName = "Wac_waslabel"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "BaseID,WasCode,WasType,LabelID,EffectDate,CreateBy,LastUpdate,UpdateBy,Active,rowguid,SyncCreate,SyncLastUpd,LastSyncBy,IsHost"
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
#Region "WAC_WASLABEL Scheme"
    Public Class WAC_WASLABELScheme
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
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LabelID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "EffectDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)

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
        Public ReadOnly Property LabelID As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property

        Public ReadOnly Property EffectDate As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace