
Imports SEAL.Data
imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
#Region "WAC_CHARQUEST Class"
Public NotInheritable Class WAC_CHARQUEST
	Inherits Core.CoreControl
	Private Wac_charquestInfo As Wac_charquestInfo = New Wac_charquestInfo
	
	Public Sub New(ByVal Conn As String)
        ConnectionString = Conn
        ConnectionSetup()
    End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ListWac_charquestCont As List(Of Container.Wac_charquest), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ListWac_charquestCont Is Nothing Or ListWac_charquestCont.Count <= 0 Then

                    StartSQLControl()
                Else

                    StartSQLControl()

                    With objSQL
                        .TableName = "Wac_charquest WITH (ROWLOCK)"
                        strSQL = BuildDelete(.TableName, "BaseID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListWac_charquestCont(0).BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListWac_charquestCont(0).WasCode) & "' AND WasType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListWac_charquestCont(0).WasType) & "' ")
                        BatchList.Add(strSQL)
                    End With

                    For Each Wac_charquestCont In ListWac_charquestCont
                        With objSQL
                            .TableName = "Wac_charquest"
                            .AddField("QuestAnswer", Wac_charquestCont.QuestAnswer, SQLControl.EnumDataType.dtStringN)
                            .AddField("SubText", Wac_charquestCont.SubText, SQLControl.EnumDataType.dtStringN)
                            '.AddField("rowguid", Wac_charquestCont.rowguid, SQLControl.EnumDataType.dtString)
                            .AddField("CreateDate", Wac_charquestCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", Wac_charquestCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", Wac_charquestCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", Wac_charquestCont.UpdateBy, SQLControl.EnumDataType.dtString)
                            .AddField("Status", Wac_charquestCont.Status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Active", Wac_charquestCont.Active, SQLControl.EnumDataType.dtNumeric)
                            .AddField("SyncCreate", Wac_charquestCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("SyncLastUpd", Wac_charquestCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                            .AddField("LastSyncBy", Wac_charquestCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                            .AddField("BaseID", Wac_charquestCont.BaseID, SQLControl.EnumDataType.dtString)
                            .AddField("WasCode", Wac_charquestCont.WasCode, SQLControl.EnumDataType.dtString)
                            .AddField("WasType", Wac_charquestCont.WasType, SQLControl.EnumDataType.dtString)
                            .AddField("CompanyID", Wac_charquestCont.CompanyID, SQLControl.EnumDataType.dtString)
                            .AddField("LocID", Wac_charquestCont.LocID, SQLControl.EnumDataType.dtString)
                            .AddField("DictionaryNo", Wac_charquestCont.DictionaryNo, SQLControl.EnumDataType.dtString)
                            .AddField("QuestID", Wac_charquestCont.QuestID, SQLControl.EnumDataType.dtString)
                            .AddField("SeqNo", Wac_charquestCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)

                        End With
                        BatchList.Add(strSQL)
                    Next
                End If

                Try
                    If BatchExecute Then
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
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Return False
            Finally
                ListWac_charquestCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ListWac_charquestCont As List(Of Container.Wac_charquest), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ListWac_charquestCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal Wac_charquestCont As List(Of Container.Wac_charquest), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Wac_charquestCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal Wac_charquestCont As Container.Wac_charquest, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Wac_charquestCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_charquestInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BaseID = '" & Wac_charquestCont.BaseID & "' AND WasCode = '" & Wac_charquestCont.WasCode & "' AND WasType = '" & Wac_charquestCont.WasType & "' AND CompanyID = '" & Wac_charquestCont.CompanyID & "' AND LocID = '" & Wac_charquestCont.LocID & "' AND DictionaryNo = '" & Wac_charquestCont.DictionaryNo & "' AND QuestID = '" & Wac_charquestCont.QuestID & "'")
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
                                strSQL = BuildUpdate(Wac_charquestInfo.MyInfo.TableName, " SET Flag = 0" &
                                " , LastUpdate = '" & Wac_charquestCont.LastUpdate & "' , UpdateBy = '" &
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_charquestCont.UpdateBy) & "' WHERE" &
                                "BaseID = '" & Wac_charquestCont.BaseID & "' AND WasCode = '" & Wac_charquestCont.WasCode & "' AND WasType = '" & Wac_charquestCont.WasType & "' AND CompanyID = '" & Wac_charquestCont.CompanyID & "' AND LocID = '" & Wac_charquestCont.LocID & "' AND DictionaryNo = '" & Wac_charquestCont.DictionaryNo & "' AND QuestID = '" & Wac_charquestCont.QuestID & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Wac_charquestInfo.MyInfo.TableName, "BaseID = '" & Wac_charquestCont.BaseID & "' AND WasCode = '" & Wac_charquestCont.WasCode & "' AND WasType = '" & Wac_charquestCont.WasType & "' AND CompanyID = '" & Wac_charquestCont.CompanyID & "' AND LocID = '" & Wac_charquestCont.LocID & "' AND DictionaryNo = '" & Wac_charquestCont.DictionaryNo & "' AND QuestID = '" & Wac_charquestCont.QuestID & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Return False
                            'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Return False
                'Throw axDelete
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Return False
                'Throw exDelete
            Finally
                Wac_charquestCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetWAC_CHARQUEST(ByVal WasCode As System.String, ByVal WasType As System.String) As Container.Wac_charquest
            Dim rWac_charquest As Container.Wac_charquest = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Wac_charquestInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "WasCode = '" & WasCode & "' AND WasType = '" & WasType & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_charquest = New Container.Wac_charquest
                                rWac_charquest.BaseID = drRow.Item("BaseID")
                                rWac_charquest.WasCode = drRow.Item("WasCode")
                                rWac_charquest.WasType = drRow.Item("WasType")
                                rWac_charquest.CompanyID = drRow.Item("CompanyID")
                                rWac_charquest.LocID = drRow.Item("LocID")
                                rWac_charquest.DictionaryNo = drRow.Item("DictionaryNo")
                                rWac_charquest.QuestID = drRow.Item("QuestID")
                                rWac_charquest.QuestAnswer = drRow.Item("QuestAnswer")
                                rWac_charquest.SubText = drRow.Item("SubText")
                                rWac_charquest.rowguid = drRow.Item("rowguid")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rWac_charquest.CreateDate = drRow.Item("CreateDate")
                                End If
                                rWac_charquest.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rWac_charquest.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rWac_charquest.UpdateBy = drRow.Item("UpdateBy")
                                rWac_charquest.Status = drRow.Item("Status")
                                rWac_charquest.Active = drRow.Item("Active")
                                rWac_charquest.SyncCreate = drRow.Item("SyncCreate")
                                rWac_charquest.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWac_charquest.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rWac_charquest = Nothing
                            End If
                        Else
                            rWac_charquest = Nothing
                        End If
                    End With
                End If
                Return rWac_charquest
            Catch ex As Exception
                Throw ex
            Finally
                rWac_charquest = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_CHARQUEST(ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String, ByVal CompanyID As System.String, ByVal LocID As System.String, ByVal DictionaryNo As System.String, ByVal QuestID As System.String, DecendingOrder As Boolean) As List(Of Container.Wac_charquest)
            Dim rWac_charquest As Container.Wac_charquest = Nothing
            Dim lstWac_charquest As List(Of Container.Wac_charquest) = New List(Of Container.Wac_charquest)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Wac_charquestInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by BaseID, WasCode, WasType, CompanyID, LocID, DictionaryNo, QuestID DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "BaseID = '" & BaseID & "' AND WasCode = '" & WasCode & "' AND WasType = '" & WasType & "' AND CompanyID = '" & CompanyID & "' AND LocID = '" & LocID & "' AND DictionaryNo = '" & DictionaryNo & "' AND QuestID = '" & QuestID & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_charquest = New Container.Wac_charquest
                                rWac_charquest.BaseID = drRow.Item("BaseID")
                                rWac_charquest.WasCode = drRow.Item("WasCode")
                                rWac_charquest.WasType = drRow.Item("WasType")
                                rWac_charquest.CompanyID = drRow.Item("CompanyID")
                                rWac_charquest.LocID = drRow.Item("LocID")
                                rWac_charquest.DictionaryNo = drRow.Item("DictionaryNo")
                                rWac_charquest.QuestID = drRow.Item("QuestID")
                                rWac_charquest.QuestAnswer = drRow.Item("QuestAnswer")
                                rWac_charquest.SubText = drRow.Item("SubText")
                                rWac_charquest.rowguid = drRow.Item("rowguid")
                                rWac_charquest.CreateBy = drRow.Item("CreateBy")
                                rWac_charquest.UpdateBy = drRow.Item("UpdateBy")
                                rWac_charquest.Status = drRow.Item("Status")
                                rWac_charquest.Active = drRow.Item("Active")
                                rWac_charquest.SyncCreate = drRow.Item("SyncCreate")
                                rWac_charquest.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWac_charquest.LastSyncBy = drRow.Item("LastSyncBy")
                                lstWac_charquest.Add(rWac_charquest)
                            Next

                        Else
                            rWac_charquest = Nothing
                        End If
                        Return lstWac_charquest
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWac_charquest = Nothing
                lstWac_charquest = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_CHARQUESTList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Wac_charquestInfo.MyInfo
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

        Public Overloads Function GetWAC_CHARQUESTShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With Wac_charquestInfo.MyInfo
                    If ShortListing Then
                        strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Else
                        strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function DeleteCharQuest(ByVal BaseID As String, ByVal WasCode As String, ByVal WasType As String, ByVal QuestID As String) As Boolean
            If StartConnection() = True Then
                With Wac_charquestInfo.MyInfo
                    strSQL = "Delete from WAC_CHARQUEST WHERE" &
                            " BaseID = '" & BaseID & "' AND WasCode = '" & WasCode & "' AND WasType = '" & WasType & "' AND QuestID IN ('" & QuestID & "')"
                    objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return True
                End With
            Else
                Return False
            End If
            EndConnection()
        End Function


#End Region
    End Class
#End Region

#Region "Container"
Namespace Container
#Region "Wac_charquest Container"
public Class Wac_charquest_FieldName
	Public BaseID As System.String = "BaseID"
	Public WasCode As System.String = "WasCode"
	Public WasType As System.String = "WasType"
	Public CompanyID As System.String = "CompanyID"
	Public LocID As System.String = "LocID"
	Public DictionaryNo As System.String = "DictionaryNo"
	Public QuestID As System.String = "QuestID"
	Public QuestAnswer As System.String = "QuestAnswer"
	Public SubText As System.String = "SubText"
	Public rowguid As System.String = "rowguid"
	Public CreateDate As System.String = "CreateDate"
	Public CreateBy As System.String = "CreateBy"
	Public LastUpdate As System.String = "LastUpdate"
	Public UpdateBy As System.String = "UpdateBy"
	Public Status As System.String = "Status"
	Public Active As System.String = "Active"
	Public Flag As System.String = "Flag"
	Public SyncCreate As System.String = "SyncCreate"
	Public SyncLastUpd As System.String = "SyncLastUpd"
	Public LastSyncBy As System.String = "LastSyncBy"
            Public _SeqNo As System.Int32
        End Class

Public Class Wac_charquest
	Protected _BaseID As System.String
	Protected _WasCode As System.String
	Protected _WasType As System.String
	Protected _CompanyID As System.String
	Protected _LocID As System.String
	Protected _DictionaryNo As System.String
	Protected _QuestID As System.String
	Private _QuestAnswer As System.String
	Private _SubText As System.String
	Private _rowguid As System.Guid
	Private _CreateDate As System.DateTime
	Private _CreateBy As System.String
	Private _LastUpdate As System.DateTime
	Private _UpdateBy As System.String
	Private _Status As System.Byte
	Private _Active As System.Byte
	Private _SyncCreate As System.DateTime
	Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String
            Private _SeqNo As System.String

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
	Public Property CompanyID As System.String
		Get 
			Return _CompanyID
		End Get
		Set(ByVal Value As System.String)
			_CompanyID = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory
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
	''' Mandatory
    ''' </summary>
	Public Property DictionaryNo As System.String
		Get 
			Return _DictionaryNo
		End Get
		Set(ByVal Value As System.String)
			_DictionaryNo = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory
    ''' </summary>
	Public Property QuestID As System.String
		Get 
			Return _QuestID
		End Get
		Set(ByVal Value As System.String)
			_QuestID = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property QuestAnswer As System.String
		Get 
			Return _QuestAnswer
		End Get
		Set(ByVal Value As System.String)
			_QuestAnswer = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property SubText As System.String
		Get 
			Return _SubText
		End Get
		Set(ByVal Value As System.String)
			_SubText = Value
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
            Public Property SeqNo As System.String
                Get
                    Return _SeqNo
                End Get
                Set(ByVal Value As System.String)
                    _SeqNo = Value
                End Set
            End Property
        End Class
#End Region
End Namespace
#End Region

#Region "Class Info"
#Region "Wac_charquest Info"
public Class Wac_charquestInfo
		Inherits Core.CoreBase
	Protected Overrides Sub InitializeClassInfo()
		With MyInfo
                .FieldsList = "BaseID,WasCode,WasType,CompanyID,LocID,DictionaryNo,QuestID,QuestAnswer,SubText,rowguid,CreateDate,CreateBy,LastUpdate,UpdateBy,Status,Active,Flag,SyncCreate,SyncLastUpd,LastSyncBy,SeqNo"
                .CheckFields = "Status,Active,Flag"
			.TableName = "Wac_charquest"
            .DefaultCond = Nothing
            .DefaultOrder = Nothing
            .Listing = "BaseID,WasCode,WasType,CompanyID,LocID,DictionaryNo,QuestID,QuestAnswer,SubText,rowguid,CreateDate,CreateBy,LastUpdate,UpdateBy,Status,Active,Flag,SyncCreate,SyncLastUpd,LastSyncBy"
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
#Region "WAC_CHARQUEST Scheme"
Public Class WAC_CHARQUESTScheme
Inherits Core.SchemeBase
	Protected Overrides Sub InitializeInfo()
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "BaseID"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(0,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "WasCode"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(1,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "WasType"
				.Length = 3
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(2,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "CompanyID"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(3,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "LocID"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(4,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "DictionaryNo"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(5,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "QuestID"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(6,this)
			With this
				.DataType = SQLControl.EnumDataType.dtStringN
				.FieldName = "QuestAnswer"
				.Length = 4000
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(7,this)
			With this
				.DataType = SQLControl.EnumDataType.dtStringN
				.FieldName = "SubText"
				.Length = 4000
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(8,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "rowguid"
				.Length = 16
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(9,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "CreateDate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(10,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "CreateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(11,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "LastUpdate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(12,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "UpdateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(13,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Status"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(14,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Active"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(15,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Flag"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(16,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "SyncCreate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(17,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "SyncLastUpd"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(18,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "LastSyncBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "SeqNo"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)

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
		Public ReadOnly Property CompanyID As StrucElement
			Get
                Return MyBase.GetItem(3)
            End Get
        End Property
		Public ReadOnly Property LocID As StrucElement
			Get
                Return MyBase.GetItem(4)
            End Get
        End Property
		Public ReadOnly Property DictionaryNo As StrucElement
			Get
                Return MyBase.GetItem(5)
            End Get
        End Property
		Public ReadOnly Property QuestID As StrucElement
			Get
                Return MyBase.GetItem(6)
            End Get
        End Property
	
		Public ReadOnly Property QuestAnswer As StrucElement
			Get
                Return MyBase.GetItem(7)
            End Get
        End Property
		Public ReadOnly Property SubText As StrucElement
			Get
                Return MyBase.GetItem(8)
            End Get
        End Property
		Public ReadOnly Property rowguid As StrucElement
			Get
                Return MyBase.GetItem(9)
            End Get
        End Property
		Public ReadOnly Property CreateDate As StrucElement
			Get
                Return MyBase.GetItem(10)
            End Get
        End Property
		Public ReadOnly Property CreateBy As StrucElement
			Get
                Return MyBase.GetItem(11)
            End Get
        End Property
		Public ReadOnly Property LastUpdate As StrucElement
			Get
                Return MyBase.GetItem(12)
            End Get
        End Property
		Public ReadOnly Property UpdateBy As StrucElement
			Get
                Return MyBase.GetItem(13)
            End Get
        End Property
		Public ReadOnly Property Status As StrucElement
			Get
                Return MyBase.GetItem(14)
            End Get
        End Property
		Public ReadOnly Property Active As StrucElement
			Get
                Return MyBase.GetItem(15)
            End Get
        End Property
		Public ReadOnly Property Flag As StrucElement
			Get
                Return MyBase.GetItem(16)
            End Get
        End Property
		Public ReadOnly Property SyncCreate As StrucElement
			Get
                Return MyBase.GetItem(17)
            End Get
        End Property
		Public ReadOnly Property SyncLastUpd As StrucElement
			Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property SeqNo As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
        Return MyBase.GetItem(Key)
    End Function
End Class
#End Region
#End Region

End Namespace