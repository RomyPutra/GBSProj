Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace GeneralSettings
#Region "QANSWER Class"
    Public NotInheritable Class QANSWER
        Inherits Core.CoreControl
        Private QanswerInfo As QanswerInfo = New QanswerInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal QanswerCont As Container.Qanswer, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If QanswerCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With QanswerInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SpID = '" & QanswerCont.SpID & "'")
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
                            'Throw New ApplicationException("210011: Record already exist")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIconInformation,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "Qanswer"
                                .AddField("Name", QanswerCont.Name, SQLControl.EnumDataType.dtStringN)
                                .AddField("Code", QanswerCont.Code, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SeqNo", QanswerCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Description", QanswerCont.Description, SQLControl.EnumDataType.dtStringN)
                                .AddField("Mean", QanswerCont.Mean, SQLControl.EnumDataType.dtNumeric)
                                .AddField("QQusID", QanswerCont.QQusID, SQLControl.EnumDataType.dtString)
                                .AddField("LoadedAnswer", QanswerCont.LoadedAnswer, SQLControl.EnumDataType.dtStringN)
                                .AddField("TranslatedAnswer", QanswerCont.TranslatedAnswer, SQLControl.EnumDataType.dtStringN)
                                .AddField("SubText", QanswerCont.SubText, SQLControl.EnumDataType.dtStringN)
                                .AddField("Active", QanswerCont.Active, SQLControl.EnumDataType.dtNumeric)
                                ' .AddField("rowguid", QanswerCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("Status", QanswerCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncCreate", QanswerCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", QanswerCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", QanswerCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SpID = '" & QanswerCont.SpID & "' AND Code = '" & QanswerCont.Code & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("SpID", QanswerCont.SpID, SQLControl.EnumDataType.dtString)
                                                .AddField("Code", QanswerCont.Code, SQLControl.EnumDataType.dtString)
                                                .AddField("SeqNo", QanswerCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SpID = '" & QanswerCont.SpID & "'")
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
                'Throw axAssign
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Return False
            Finally
                QanswerCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal QanswerCont As Container.Qanswer, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(QanswerCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal QanswerCont As Container.Qanswer, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(QanswerCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal QanswerCont As Container.Qanswer, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If QanswerCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With QanswerInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SpID = '" & QanswerCont.SpID & "' AND Code = '" & QanswerCont.Code & "' AND SeqNo = '" & QanswerCont.SeqNo & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 1 Then
                                        blnInUse = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = True And blnInUse = True Then
                            With objSQL
                                strSQL = BuildUpdate(QanswerInfo.MyInfo.TableName, " SET Flag = 0" &
                                 " WHERE " &
                                "SpID = '" & QanswerCont.SpID & "' AND Code = '" & QanswerCont.Code & "' AND SeqNo = '" & QanswerCont.SeqNo & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(QanswerInfo.MyInfo.TableName, "SpID = '" & QanswerCont.SpID & "' AND Code = '" & QanswerCont.Code & "' AND SeqNo = '" & QanswerCont.SeqNo & "'")
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
                QanswerCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetQANSWER(ByVal SpID As System.String, ByVal Code As System.String) As Container.Qanswer
            Dim rQanswer As Container.Qanswer = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With QanswerInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "SpID = '" & SpID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rQanswer = New Container.Qanswer
                                rQanswer.SpID = drRow.Item("SpID")
                                rQanswer.Code = drRow.Item("Code")
                                rQanswer.SeqNo = drRow.Item("SeqNo")
                                rQanswer.Name = drRow.Item("Name")
                                rQanswer.Description = drRow.Item("Description")
                                rQanswer.Mean = drRow.Item("Mean")
                                rQanswer.QQusID = drRow.Item("QQusID")
                                rQanswer.LoadedAnswer = drRow.Item("LoadedAnswer")
                                rQanswer.TranslatedAnswer = drRow.Item("TranslatedAnswer")
                                rQanswer.SubText = drRow.Item("SubText")
                                rQanswer.Active = drRow.Item("Active")
                                rQanswer.rowguid = drRow.Item("rowguid")
                                rQanswer.Status = drRow.Item("Status")
                                rQanswer.SyncCreate = drRow.Item("SyncCreate")
                                rQanswer.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rQanswer.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rQanswer = Nothing
                            End If
                        Else
                            rQanswer = Nothing
                        End If
                    End With
                End If
                Return rQanswer
            Catch ex As Exception
                Throw ex
            Finally
                rQanswer = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetQANSWER(ByVal SpID As System.String, ByVal Code As System.String, ByVal SeqNo As System.Int32, DecendingOrder As Boolean) As List(Of Container.Qanswer)
            Dim rQanswer As Container.Qanswer = Nothing
            Dim lstQanswer As List(Of Container.Qanswer) = New List(Of Container.Qanswer)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With QanswerInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by SpID, Code, SeqNo DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "SpID = '" & SpID & "' AND SeqNo = '" & SeqNo & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rQanswer = New Container.Qanswer
                                rQanswer.SpID = drRow.Item("SpID")
                                rQanswer.Code = drRow.Item("Code")
                                rQanswer.SeqNo = drRow.Item("SeqNo")
                                rQanswer.Name = drRow.Item("Name")
                                rQanswer.Description = drRow.Item("Description")
                                rQanswer.Mean = drRow.Item("Mean")
                                rQanswer.QQusID = drRow.Item("QQusID")
                                rQanswer.LoadedAnswer = drRow.Item("LoadedAnswer")
                                rQanswer.TranslatedAnswer = drRow.Item("TranslatedAnswer")
                                rQanswer.SubText = drRow.Item("SubText")
                                rQanswer.Active = drRow.Item("Active")
                                rQanswer.rowguid = drRow.Item("rowguid")
                                rQanswer.Status = drRow.Item("Status")
                                rQanswer.SyncCreate = drRow.Item("SyncCreate")
                                rQanswer.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rQanswer.LastSyncBy = drRow.Item("LastSyncBy")
                                lstQanswer.Add(rQanswer)
                            Next

                        Else
                            rQanswer = Nothing
                        End If
                        Return lstQanswer
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rQanswer = Nothing
                lstQanswer = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetQANSWERList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With QanswerInfo.MyInfo
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

        Public Overloads Function GetQANSWERShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With QanswerInfo.MyInfo
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
#Region "Qanswer Container"
        Public Class Qanswer_FieldName
            Public SpID As System.String = "SpID"
            Public Code As System.String = "Code"
            Public SeqNo As System.String = "SeqNo"
            Public Name As System.String = "Name"
            Public Description As System.String = "Description"
            Public Mean As System.String = "Mean"
            Public QQusID As System.String = "QQusID"
            Public LoadedAnswer As System.String = "LoadedAnswer"
            Public TranslatedAnswer As System.String = "TranslatedAnswer"
            Public SubText As System.String = "SubText"
            Public Active As System.String = "Active"
            Public Flag As System.String = "Flag"
            Public rowguid As System.String = "rowguid"
            Public Status As System.String = "Status"
            Public SyncCreate As System.String = "SyncCreate"
            Public SyncLastUpd As System.String = "SyncLastUpd"
            Public LastSyncBy As System.String = "LastSyncBy"
        End Class

        Public Class Qanswer
            Protected _SpID As System.String
            Protected _Code As System.String
            Protected _SeqNo As System.Int32
            Private _Name As System.String
            Private _Description As System.String
            Private _Mean As System.Decimal
            Private _QQusID As System.String
            Private _LoadedAnswer As System.String
            Private _TranslatedAnswer As System.String
            Private _SubText As System.String
            Private _Active As System.Byte
            Private _rowguid As System.Guid
            Private _Status As System.Byte
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property SpID As System.String
                Get
                    Return _SpID
                End Get
                Set(ByVal Value As System.String)
                    _SpID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Code As System.String
                Get
                    Return _Code
                End Get
                Set(ByVal Value As System.String)
                    _Code = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property SeqNo As System.Int32
                Get
                    Return _SeqNo
                End Get
                Set(ByVal Value As System.Int32)
                    _SeqNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Name As System.String
                Get
                    Return _Name
                End Get
                Set(ByVal Value As System.String)
                    _Name = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Description As System.String
                Get
                    Return _Description
                End Get
                Set(ByVal Value As System.String)
                    _Description = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Mean As System.Decimal
                Get
                    Return _Mean
                End Get
                Set(ByVal Value As System.Decimal)
                    _Mean = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property QQusID As System.String
                Get
                    Return _QQusID
                End Get
                Set(ByVal Value As System.String)
                    _QQusID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LoadedAnswer As System.String
                Get
                    Return _LoadedAnswer
                End Get
                Set(ByVal Value As System.String)
                    _LoadedAnswer = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TranslatedAnswer As System.String
                Get
                    Return _TranslatedAnswer
                End Get
                Set(ByVal Value As System.String)
                    _TranslatedAnswer = Value
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

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Qanswer Info"
    Public Class QanswerInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "SpID,Code,SeqNo,Name,Description,Mean,QQusID,LoadedAnswer,TranslatedAnswer,SubText,Active,Flag,rowguid,Status,SyncCreate,SyncLastUpd,LastSyncBy"
                .CheckFields = "Active,Flag,Status"
                .TableName = "Qanswer"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "SpID,Code,SeqNo,Name,Description,Mean,QQusID,LoadedAnswer,TranslatedAnswer,SubText,Active,Flag,rowguid,Status,SyncCreate,SyncLastUpd,LastSyncBy"
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
#Region "QANSWER Scheme"
    Public Class QANSWERScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "SpID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Code"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "SeqNo"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Name"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Description"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Mean"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "QQusID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "LoadedAnswer"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "TranslatedAnswer"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SubText"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
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
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
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

        Public ReadOnly Property SpID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property Code As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property SeqNo As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property Name As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property Description As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property Mean As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property QQusID As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property LoadedAnswer As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property TranslatedAnswer As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property SubText As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
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
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
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