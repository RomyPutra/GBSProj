Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
#Region "ANNOUNCEMENT Class"
    Public NotInheritable Class ANNOUNCEMENT
        Inherits Core.CoreControl
        Private AnnouncementInfo As AnnouncementInfo = New AnnouncementInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal AnnouncementCont As Container.Announcement, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If AnnouncementCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With AnnouncementInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "PostID = '" & AnnouncementCont.PostID & "'")
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
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            message = "Record already exist"
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "Announcement"
                                .AddField("PostTitle", AnnouncementCont.PostTitle, SQLControl.EnumDataType.dtStringN)
                                .AddField("PostNote", AnnouncementCont.PostNote, SQLControl.EnumDataType.dtStringN)
                                .AddField("PostPath1", AnnouncementCont.PostPath1, SQLControl.EnumDataType.dtStringN)
                                .AddField("PostPath2", AnnouncementCont.PostPath2, SQLControl.EnumDataType.dtStringN)
                                .AddField("PostPath3", AnnouncementCont.PostPath3, SQLControl.EnumDataType.dtStringN)
                                .AddField("TargetLocate", AnnouncementCont.TargetLocate, SQLControl.EnumDataType.dtStringN)
                                .AddField("TargetGroup", AnnouncementCont.TargetGroup, SQLControl.EnumDataType.dtStringN)
                                .AddField("EffectDate", AnnouncementCont.EffectDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ExpiryDate", AnnouncementCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", AnnouncementCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", AnnouncementCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", AnnouncementCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", AnnouncementCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", AnnouncementCont.IsHost, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "PostID = '" & AnnouncementCont.PostID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("PostID", AnnouncementCont.PostID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "PostID = '" & AnnouncementCont.PostID & "'")
                                End Select
                            End With
                            Try
                                If BatchExecute Then
                                    BatchList = New ArrayList
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
                AnnouncementCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal AnnouncementCont As Container.Announcement, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(AnnouncementCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal AnnouncementCont As Container.Announcement, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(AnnouncementCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal AnnouncementCont As Container.Announcement, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If AnnouncementCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With AnnouncementInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "PostID = '" & AnnouncementCont.PostID & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Active")) = 1 Then
                                        blnInUse = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = True And blnInUse = True Then
                            With objSQL
                                strSQL = BuildUpdate(AnnouncementInfo.MyInfo.TableName, " SET Active = 0" & _
                                " , LastUpdate = '" & AnnouncementCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, AnnouncementCont.UpdateBy) & "' WHERE " & _
                                "PostID = '" & AnnouncementCont.PostID & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(AnnouncementInfo.MyInfo.TableName, "PostID = '" & AnnouncementCont.PostID & "'")
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
                AnnouncementCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetANNOUNCEMENT(ByVal PostID As System.String) As Container.Announcement
            Dim rAnnouncement As Container.Announcement = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With AnnouncementInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "PostID = '" & PostID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rAnnouncement = New Container.Announcement
                                rAnnouncement.PostID = drRow.Item("PostID")
                                rAnnouncement.PostTitle = drRow.Item("PostTitle")
                                rAnnouncement.PostNote = drRow.Item("PostNote")
                                rAnnouncement.PostPath1 = drRow.Item("PostPath1")
                                rAnnouncement.PostPath2 = drRow.Item("PostPath2")
                                rAnnouncement.PostPath3 = drRow.Item("PostPath3")
                                rAnnouncement.TargetLocate = drRow.Item("TargetLocate")
                                rAnnouncement.TargetGroup = drRow.Item("TargetGroup")
                                If Not IsDBNull(drRow.Item("EffectDate")) Then
                                    rAnnouncement.EffectDate = drRow.Item("EffectDate")
                                End If
                                If Not IsDBNull(drRow.Item("ExpiryDate")) Then
                                    rAnnouncement.ExpiryDate = drRow.Item("ExpiryDate")
                                End If
                                rAnnouncement.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rAnnouncement.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rAnnouncement.UpdateBy = drRow.Item("UpdateBy")
                                rAnnouncement.Active = drRow.Item("Active")
                                rAnnouncement.rowguid = drRow.Item("rowguid")
                                If Not IsDBNull(drRow.Item("SyncCreate")) Then
                                    rAnnouncement.SyncCreate = drRow.Item("SyncCreate")
                                End If
                                If Not IsDBNull(drRow.Item("SyncLastUpd")) Then
                                    rAnnouncement.SyncLastUpd = drRow.Item("SyncLastUpd")
                                End If
                                rAnnouncement.LastSyncBy = drRow.Item("LastSyncBy")
                                rAnnouncement.IsHost = drRow.Item("IsHost")
                            Else
                                rAnnouncement = Nothing
                            End If
                        Else
                            rAnnouncement = Nothing
                        End If
                    End With
                End If
                Return rAnnouncement
            Catch ex As Exception
                Throw ex
            Finally
                rAnnouncement = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetANNOUNCEMENT(ByVal PostID As System.String, DecendingOrder As Boolean) As List(Of Container.Announcement)
            Dim rAnnouncement As Container.Announcement = Nothing
            Dim lstAnnouncement As List(Of Container.Announcement) = New List(Of Container.Announcement)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With AnnouncementInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by PostID DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "PostID = '" & PostID & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rAnnouncement = New Container.Announcement
                                rAnnouncement.PostID = drRow.Item("PostID")
                                rAnnouncement.PostTitle = drRow.Item("PostTitle")
                                rAnnouncement.PostNote = drRow.Item("PostNote")
                                rAnnouncement.PostPath1 = drRow.Item("PostPath1")
                                rAnnouncement.PostPath2 = drRow.Item("PostPath2")
                                rAnnouncement.PostPath3 = drRow.Item("PostPath3")
                                rAnnouncement.TargetLocate = drRow.Item("TargetLocate")
                                rAnnouncement.TargetGroup = drRow.Item("TargetGroup")
                                rAnnouncement.CreateBy = drRow.Item("CreateBy")
                                rAnnouncement.UpdateBy = drRow.Item("UpdateBy")
                                rAnnouncement.Active = drRow.Item("Active")
                                rAnnouncement.rowguid = drRow.Item("rowguid")
                                rAnnouncement.LastSyncBy = drRow.Item("LastSyncBy")
                                rAnnouncement.IsHost = drRow.Item("IsHost")
                                lstAnnouncement.Add(rAnnouncement)
                            Next

                        Else
                            rAnnouncement = Nothing
                        End If
                        Return lstAnnouncement
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rAnnouncement = Nothing
                lstAnnouncement = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetANNOUNCEMENTList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With AnnouncementInfo.MyInfo                   
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

        Public Overloads Function GetANNOUNCEMENTShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With AnnouncementInfo.MyInfo
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


#End Region
    End Class
#End Region

#Region "Container"
    Namespace Container
#Region "Announcement Container"
        Public Class Announcement_FieldName
          
        End Class

        Public Class Announcement
            Public fPostID As System.String = "PostID"
            Public fPostTitle As System.String = "PostTitle"
            Public fPostNote As System.String = "PostNote"
            Public fPostPath1 As System.String = "PostPath1"
            Public fPostPath2 As System.String = "PostPath2"
            Public fPostPath3 As System.String = "PostPath3"
            Public fTargetLocate As System.String = "TargetLocate"
            Public fTargetGroup As System.String = "TargetGroup"
            Public fEffectDate As System.String = "EffectDate"
            Public fExpiryDate As System.String = "ExpiryDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fActive As System.String = "Active"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fLastSyncBy As System.String = "LastSyncBy"
            Public fIsHost As System.String = "IsHost"


            Protected _PostID As System.String
            Private _PostTitle As System.String
            Private _PostNote As System.String
            Private _PostPath1 As System.String
            Private _PostPath2 As System.String
            Private _PostPath3 As System.String
            Private _TargetLocate As System.String
            Private _TargetGroup As System.String
            Private _EffectDate As System.DateTime
            Private _ExpiryDate As System.DateTime
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
            Public Property PostID As System.String
                Get
                    Return _PostID
                End Get
                Set(ByVal Value As System.String)
                    _PostID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PostTitle As System.String
                Get
                    Return _PostTitle
                End Get
                Set(ByVal Value As System.String)
                    _PostTitle = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PostNote As System.String
                Get
                    Return _PostNote
                End Get
                Set(ByVal Value As System.String)
                    _PostNote = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PostPath1 As System.String
                Get
                    Return _PostPath1
                End Get
                Set(ByVal Value As System.String)
                    _PostPath1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PostPath2 As System.String
                Get
                    Return _PostPath2
                End Get
                Set(ByVal Value As System.String)
                    _PostPath2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PostPath3 As System.String
                Get
                    Return _PostPath3
                End Get
                Set(ByVal Value As System.String)
                    _PostPath3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TargetLocate As System.String
                Get
                    Return _TargetLocate
                End Get
                Set(ByVal Value As System.String)
                    _TargetLocate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TargetGroup As System.String
                Get
                    Return _TargetGroup
                End Get
                Set(ByVal Value As System.String)
                    _TargetGroup = Value
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
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ExpiryDate As System.DateTime
                Get
                    Return _ExpiryDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ExpiryDate = Value
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
#Region "Announcement Info"
    Public Class AnnouncementInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "PostID,PostTitle,PostNote,PostPath1,PostPath2,PostPath3,TargetLocate,TargetGroup,EffectDate,ExpiryDate,CreateBy,LastUpdate,UpdateBy,Active,rowguid,SyncCreate,SyncLastUpd,LastSyncBy,IsHost"
                .CheckFields = "Active,IsHost"
                .TableName = "Announcement"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "PostID,PostTitle,PostNote,PostPath1,PostPath2,PostPath3,TargetLocate,TargetGroup,EffectDate,ExpiryDate,CreateBy,LastUpdate,UpdateBy,Active,rowguid,SyncCreate,SyncLastUpd,LastSyncBy,IsHost"
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
#Region "ANNOUNCEMENT Scheme"
    Public Class ANNOUNCEMENTScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PostID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "PostTitle"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "PostNote"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "PostPath1"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "PostPath2"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "PostPath3"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "TargetLocate"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "TargetGroup"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "EffectDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ExpiryDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)

        End Sub

        Public ReadOnly Property PostID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property PostTitle As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property PostNote As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property PostPath1 As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property PostPath2 As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property PostPath3 As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property TargetLocate As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property TargetGroup As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property EffectDate As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property ExpiryDate As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace