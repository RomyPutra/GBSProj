
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace HELPS
#Region "HELPSUBHDR Class"
    Public NotInheritable Class HELPSUBHDR
        Inherits Core.CoreControl
        Private HelpsubhdrInfo As HelpsubhdrInfo = New HelpsubhdrInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal HelpsubhdrCont As Container.Helpsubhdr, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If HelpsubhdrCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With HelpsubhdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "HelpID = '" & HelpsubhdrCont.HelpID & "' AND HelpSubID = '" & HelpsubhdrCont.HelpSubID & "'")
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
                                .TableName = "Helpsubhdr"
                                .AddField("Subtitle", HelpsubhdrCont.Subtitle, SQLControl.EnumDataType.dtStringN)
                                .AddField("CreateDate", HelpsubhdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", HelpsubhdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", HelpsubhdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", HelpsubhdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("rowguid", HelpsubhdrCont.rowguid, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "HelpID = '" & HelpsubhdrCont.HelpID & "' AND HelpSubID = '" & HelpsubhdrCont.HelpSubID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("HelpID", HelpsubhdrCont.HelpID, SQLControl.EnumDataType.dtString)
                                                .AddField("HelpSubID", HelpsubhdrCont.HelpSubID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "HelpID = '" & HelpsubhdrCont.HelpID & "' AND HelpSubID = '" & HelpsubhdrCont.HelpSubID & "'")
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
                HelpsubhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal HelpsubhdrCont As Container.Helpsubhdr, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(HelpsubhdrCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal HelpsubhdrCont As Container.Helpsubhdr, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(HelpsubhdrCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal HelpsubhdrCont As Container.Helpsubhdr, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If HelpsubhdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With HelpsubhdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "HelpID = '" & HelpsubhdrCont.HelpID & "' AND HelpSubID = '" & HelpsubhdrCont.HelpSubID & "'")
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
                                strSQL = BuildUpdate(HelpsubhdrInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & HelpsubhdrCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, HelpsubhdrCont.UpdateBy) & "' WHERE" & _
                                "HelpID = '" & HelpsubhdrCont.HelpID & "' AND HelpSubID = '" & HelpsubhdrCont.HelpSubID & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(HelpsubhdrInfo.MyInfo.TableName, "HelpID = '" & HelpsubhdrCont.HelpID & "' AND HelpSubID = '" & HelpsubhdrCont.HelpSubID & "'")
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
                HelpsubhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetHELPSUBHDR(ByVal HelpID As System.String, ByVal HelpSubID As System.String) As Container.Helpsubhdr
            Dim rHelpsubhdr As Container.Helpsubhdr = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With HelpsubhdrInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "HelpID = '" & HelpID & "' AND HelpSubID = '" & HelpSubID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rHELPSUBHDR = New Container.HELPSUBHDR
                                rHELPSUBHDR.HelpID = drRow.Item("HelpID")
                                rHELPSUBHDR.HelpSubID = drRow.Item("HelpSubID")
                                rHelpsubhdr.Subtitle = drRow.Item("Subtitle")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rHelpsubhdr.CreateDate = drRow.Item("CreateDate")
                                End If
                                rHelpsubhdr.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rHelpsubhdr.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rHelpsubhdr.UpdateBy = drRow.Item("UpdateBy")
                                rHelpsubhdr.rowguid = drRow.Item("rowguid")
                            Else
                                rHelpsubhdr = Nothing
                            End If
                        Else
                            rHelpsubhdr = Nothing
                        End If
                    End With
                End If
                Return rHelpsubhdr
            Catch ex As Exception
                Throw ex
            Finally
                rHelpsubhdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetHELPSUBHDR(ByVal HelpID As System.String, ByVal HelpSubID As System.String, DecendingOrder As Boolean) As List(Of Container.Helpsubhdr)
            Dim rHelpsubhdr As Container.Helpsubhdr = Nothing
            Dim lstHelpsubhdr As List(Of Container.Helpsubhdr) = New List(Of Container.Helpsubhdr)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With HelpsubhdrInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by HelpID, HelpSubID DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "HelpID = '" & HelpID & "' AND HelpSubID = '" & HelpSubID & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rHELPSUBHDR = New Container.HELPSUBHDR
                                rHELPSUBHDR.HelpID = drRow.Item("HelpID")
                                rHELPSUBHDR.HelpSubID = drRow.Item("HelpSubID")
                                rHelpsubhdr.Subtitle = drRow.Item("Subtitle")
                                rHelpsubhdr.CreateBy = drRow.Item("CreateBy")
                                rHelpsubhdr.UpdateBy = drRow.Item("UpdateBy")
                                rHelpsubhdr.rowguid = drRow.Item("rowguid")
                                lstHelpsubhdr.Add(rHelpsubhdr)
                            Next

                        Else
                            rHelpsubhdr = Nothing
                        End If
                        Return lstHelpsubhdr
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rHelpsubhdr = Nothing
                lstHelpsubhdr = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetHELPSUBHDRList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With HelpsubhdrInfo.MyInfo
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

        Public Overloads Function GetHELPSUBHDRShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With HelpsubhdrInfo.MyInfo
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
#Region "Helpsubhdr Container"
        Public Class Helpsubhdr_FieldName
            Public HelpID As System.String = "HelpID"
            Public HelpSubID As System.String = "HelpSubID"
            Public Subtitle As System.String = "Subtitle"
            Public CreateDate As System.String = "CreateDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public rowguid As System.String = "rowguid"
        End Class

        Public Class Helpsubhdr
            Protected _HelpID As System.String
            Protected _HelpSubID As System.String
            Private _Subtitle As System.String
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _rowguid As System.Guid

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property HelpID As System.String
                Get
                    Return _HelpID
                End Get
                Set(ByVal Value As System.String)
                    _HelpID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property HelpSubID As System.String
                Get
                    Return _HelpSubID
                End Get
                Set(ByVal Value As System.String)
                    _HelpSubID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Subtitle As System.String
                Get
                    Return _Subtitle
                End Get
                Set(ByVal Value As System.String)
                    _Subtitle = Value
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

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Helpsubhdr Info"
    Public Class HelpsubhdrInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "HelpID,HelpSubID,Subtitle,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid"
                .CheckFields = ""
                .TableName = "Helpsubhdr"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "HelpID,HelpSubID,Subtitle,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid"
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
#Region "HELPSUBHDR Scheme"
    Public Class HELPSUBHDRScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "HelpID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "HelpSubID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Subtitle"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)

        End Sub

        Public ReadOnly Property HelpID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property HelpSubID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property

        Public ReadOnly Property Subtitle As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace