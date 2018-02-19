Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Contract
#Region "LicenseCapacity Class"
    Public NotInheritable Class LicenseCapacity
        Inherits Core.CoreControl
        Private LicensecapacityInfo As LicensecapacityInfo = New LicensecapacityInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal LicensecapacityCont As Container.Licensecapacity, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If LicensecapacityCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With LicensecapacityInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ItemCode = '" & LicensecapacityCont.ItemCode & "' AND ReceiverID = '" & LicensecapacityCont.ReceiverID & "' AND ReceiverLocID = '" & LicensecapacityCont.ReceiverLocID & "'")
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
                                .TableName = "Licensecapacity"
                                .AddField("CapacityLevel", LicensecapacityCont.CapacityLevel, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastUpdate", LicensecapacityCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", LicensecapacityCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", LicensecapacityCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", LicensecapacityCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ItemCode = '" & LicensecapacityCont.ItemCode & "' AND ReceiverID = '" & LicensecapacityCont.ReceiverID & "' AND ReceiverLocID = '" & LicensecapacityCont.ReceiverLocID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("ItemCode", LicensecapacityCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverID", LicensecapacityCont.ReceiverID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverLocID", LicensecapacityCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ItemCode = '" & LicensecapacityCont.ItemCode & "' AND ReceiverID = '" & LicensecapacityCont.ReceiverID & "' AND ReceiverLocID = '" & LicensecapacityCont.ReceiverLocID & "'")
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
                LicensecapacityCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveList(ByVal ListLicensecapacityCont As List(Of Container.Licensecapacity), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            SaveList = False
            Dim ListSQL As ArrayList = New ArrayList()
            Try
                If ListLicensecapacityCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    For Each LicensecapacityCont In ListLicensecapacityCont
                        If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                            StartSQLControl()

                            With LicensecapacityInfo.MyInfo
                                strSQL = BuildSelect(.CheckFields, .TableName, "ItemCode = '" & LicensecapacityCont.ItemCode & "' AND ReceiverID = '" & LicensecapacityCont.ReceiverID & "' AND ReceiverLocID = '" & LicensecapacityCont.ReceiverLocID & "'")
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
                                pType = SQLControl.EnumSQLType.stUpdate
                            Else
                                pType = SQLControl.EnumSQLType.stInsert
                            End If

                            StartSQLControl()
                            With objSQL
                                .TableName = "Licensecapacity"
                                .AddField("CapacityLevel", LicensecapacityCont.CapacityLevel, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastUpdate", LicensecapacityCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", LicensecapacityCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", LicensecapacityCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", LicensecapacityCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ItemCode = '" & LicensecapacityCont.ItemCode & "' AND ReceiverID = '" & LicensecapacityCont.ReceiverID & "' AND ReceiverLocID = '" & LicensecapacityCont.ReceiverLocID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("ItemCode", LicensecapacityCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverID", LicensecapacityCont.ReceiverID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverLocID", LicensecapacityCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ItemCode = '" & LicensecapacityCont.ItemCode & "' AND ReceiverID = '" & LicensecapacityCont.ReceiverID & "' AND ReceiverLocID = '" & LicensecapacityCont.ReceiverLocID & "'")
                                End Select
                            End With
                        End If
                        ListSQL.Add(strSQL)
                    Next
                    Try
                        If ListSQL IsNot Nothing AndAlso ListSQL.Count > 0 Then
                            objConn.BatchExecute(ListSQL, CommandType.Text)
                        End If
                        Return True
                    Catch axExecute As Exception
                        Dim sqlStatement As String = " "
                        If objConn.FailedSQLStatement.Count > 0 Then
                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
                        End If

                        If pType = SQLControl.EnumSQLType.stInsert Then
                            message = axExecute.Message.ToString()
                            Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                        Else
                            message = axExecute.Message.ToString()
                            Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                        End If
                    Finally
                        'objSQL.Dispose()
                    End Try
                    Return True
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
                ListLicensecapacityCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Public Function Insert(ByVal LicensecapacityCont As Container.Licensecapacity, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(LicensecapacityCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'ADD
        Public Function InsertList(ByVal ListLicensecapacityCont As List(Of Container.Licensecapacity), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return SaveList(ListLicensecapacityCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal LicensecapacityCont As Container.Licensecapacity, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(LicensecapacityCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal LicensecapacityCont As Container.Licensecapacity, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If LicensecapacityCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With LicensecapacityInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ItemCode = '" & LicensecapacityCont.ItemCode & "' AND ReceiverID = '" & LicensecapacityCont.ReceiverID & "' AND ReceiverLocID = '" & LicensecapacityCont.ReceiverLocID & "'")
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
                                strSQL = BuildUpdate(LicensecapacityInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & LicensecapacityCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, LicensecapacityCont.UpdateBy) & "' WHERE" & _
                                "ItemCode = '" & LicensecapacityCont.ItemCode & "' AND ReceiverID = '" & LicensecapacityCont.ReceiverID & "' AND ReceiverLocID = '" & LicensecapacityCont.ReceiverLocID & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(LicensecapacityInfo.MyInfo.TableName, "ItemCode = '" & LicensecapacityCont.ItemCode & "' AND ReceiverID = '" & LicensecapacityCont.ReceiverID & "' AND ReceiverLocID = '" & LicensecapacityCont.ReceiverLocID & "'")
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
                LicensecapacityCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetLICENSECAPACITY(ByVal ItemCode As System.String, ByVal ReceiverID As System.String, ByVal ReceiverLocID As System.String) As Container.Licensecapacity
            Dim rLicensecapacity As Container.Licensecapacity = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With LicensecapacityInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "ItemCode = '" & ItemCode & "' AND ReceiverID = '" & ReceiverID & "' AND ReceiverLocID = '" & ReceiverLocID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rLicensecapacity = New Container.Licensecapacity
                                rLicensecapacity.ItemCode = drRow.Item("ItemCode")
                                rLicensecapacity.ReceiverID = drRow.Item("ReceiverID")
                                rLicensecapacity.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rLicensecapacity.CapacityLevel = drRow.Item("CapacityLevel")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rLicensecapacity.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rLicensecapacity.UpdateBy = drRow.Item("UpdateBy")
                                rLicensecapacity.SyncCreate = drRow.Item("SyncCreate")
                                If Not IsDBNull(drRow.Item("SyncLastUpd")) Then
                                    rLicensecapacity.SyncLastUpd = drRow.Item("SyncLastUpd")
                                End If
                            Else
                                rLicensecapacity = Nothing
                            End If
                        Else
                            rLicensecapacity = Nothing
                        End If
                    End With
                End If
                Return rLicensecapacity
            Catch ex As Exception
                Throw ex
            Finally
                rLicensecapacity = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetLICENSECAPACITY(ByVal ItemCode As System.String, ByVal ReceiverID As System.String, ByVal ReceiverLocID As System.String, DecendingOrder As Boolean) As List(Of Container.Licensecapacity)
            Dim rLicensecapacity As Container.Licensecapacity = Nothing
            Dim lstLicensecapacity As List(Of Container.Licensecapacity) = New List(Of Container.Licensecapacity)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With LicensecapacityInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ItemCode, ReceiverID, ReceiverLocID DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "ItemCode = '" & ItemCode & "' AND ReceiverID = '" & ReceiverID & "' AND ReceiverLocID = '" & ReceiverLocID & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rLicensecapacity = New Container.Licensecapacity
                                rLicensecapacity.ItemCode = drRow.Item("ItemCode")
                                rLicensecapacity.ReceiverID = drRow.Item("ReceiverID")
                                rLicensecapacity.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rLicensecapacity.CapacityLevel = drRow.Item("CapacityLevel")
                                rLicensecapacity.UpdateBy = drRow.Item("UpdateBy")
                                rLicensecapacity.SyncCreate = drRow.Item("SyncCreate")
                                lstLicensecapacity.Add(rLicensecapacity)
                            Next

                        Else
                            rLicensecapacity = Nothing
                        End If
                        Return lstLicensecapacity
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rLicensecapacity = Nothing
                lstLicensecapacity = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetLICENSECAPACITYList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With LicensecapacityInfo.MyInfo
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

        Public Overloads Function GetLICENSECAPACITYShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With LicensecapacityInfo.MyInfo
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

        Public Overloads Function GetCapacity(ByVal ItemCode As String, ByVal BizLocID As String, ByVal BizRegID As String) As Data.DataTable
            If StartConnection() = True Then
                With LicensecapacityInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT CASE WHEN ISNULL(LC.CapacityLevel,0) = 0 THEN LI.Qty ELSE LC.CapacityLevel END AS CapacityLevel FROM LICENSE LS " & _
                        "WITH(NOLOCK) INNER JOIN LICENSEITEM LI WITH(NOLOCK) ON LS.ContractNo = LI.ContractNo LEFT JOIN LICENSECAPACITY LC " & _
                        "WITH(NOLOCK) ON LI.ItemCode = LC.ItemCode WHERE LI.ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCode) & "' " & _
                        "AND LS.CompanyID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizRegID) & "' AND LS.LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizLocID) & "' " & _
                        "GROUP BY LC.CapacityLevel, LI.Qty"
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
#Region "Licensecapacity Container"
        Public Class Licensecapacity_FieldName
            Public ItemCode As System.String = "ItemCode"
            Public ReceiverID As System.String = "ReceiverID"
            Public ReceiverLocID As System.String = "ReceiverLocID"
            Public CapacityLevel As System.String = "CapacityLevel"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public SyncCreate As System.String = "SyncCreate"
            Public SyncLastUpd As System.String = "SyncLastUpd"
        End Class

        Public Class Licensecapacity
            Protected _ItemCode As System.String
            Protected _ReceiverID As System.String
            Protected _ReceiverLocID As System.String
            Private _CapacityLevel As System.Decimal
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ItemCode As System.String
                Get
                    Return _ItemCode
                End Get
                Set(ByVal Value As System.String)
                    _ItemCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ReceiverID As System.String
                Get
                    Return _ReceiverID
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ReceiverLocID As System.String
                Get
                    Return _ReceiverLocID
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverLocID = Value
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

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Licensecapacity Info"
    Public Class LicensecapacityInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "ItemCode,ReceiverID,ReceiverLocID,CapacityLevel,Flag,LastUpdate,UpdateBy,SyncCreate,SyncLastUpd"
                .CheckFields = "ItemCode,ReceiverID,ReceiverLocID,CapacityLevel,Flag,LastUpdate,UpdateBy,SyncCreate,SyncLastUpd"
                .TableName = "Licensecapacity"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "ItemCode,ReceiverID,ReceiverLocID,CapacityLevel,Flag,LastUpdate,UpdateBy,SyncCreate,SyncLastUpd"
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
#Region "LICENSECAPACITY Scheme"
    Public Class LICENSECAPACITYScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItemCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ReceiverID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ReceiverLocID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "CapacityLevel"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)

        End Sub

        Public ReadOnly Property ItemCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property ReceiverID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property ReceiverLocID As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property CapacityLevel As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
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