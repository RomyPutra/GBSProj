Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General


Namespace Actions
    Public NotInheritable Class Inventory_submit
        Inherits Core.CoreControl
        Private Inventory_submitInfo As Inventory_submitInfo = New Inventory_submitInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal connecn As String)
            ConnectionString = connecn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal Inventory_submitCont As Container.Inventory_submit, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Inventory_submitCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Inventory_submitInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Inventory_submitCont.ID) & "' AND KOD_INV = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Inventory_submitCont.KOD_INV) & "'")
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
                                .TableName = "Inventory_submit WITH (ROWLOCK)"
                                .AddField("NORUJUKAN", Inventory_submitCont.NORUJUKAN, SQLControl.EnumDataType.dtString)
                                .AddField("IDLOGIN", Inventory_submitCont.IDLOGIN, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IDPREMIS", Inventory_submitCont.IDPREMIS, SQLControl.EnumDataType.dtString)
                                .AddField("NAMAPREMIS", Inventory_submitCont.NAMAPREMIS, SQLControl.EnumDataType.dtStringN)
                                .AddField("TARIKH_INVENTORI", Inventory_submitCont.TARIKH_INVENTORI, SQLControl.EnumDataType.dtCustom)
                                .AddField("NEGERI", Inventory_submitCont.NEGERI, SQLControl.EnumDataType.dtString)
                                .AddField("STATUS_S", Inventory_submitCont.STATUS_S, SQLControl.EnumDataType.dtString)
                                .AddField("TARIKH_WUJUD", Inventory_submitCont.TARIKH_WUJUD, SQLControl.EnumDataType.dtCustom)
                                .AddField("WUJUD_PENGGUNA", Inventory_submitCont.WUJUD_PENGGUNA, SQLControl.EnumDataType.dtStringN)
                                .AddField("TARIKH_KEMASKINI", Inventory_submitCont.TARIKH_KEMASKINI, SQLControl.EnumDataType.dtDateTime)
                                .AddField("KEMASKINI_PENGGUNA", Inventory_submitCont.KEMASKINI_PENGGUNA, SQLControl.EnumDataType.dtStringN)
                                .AddField("OFFICER_NAME", Inventory_submitCont.OFFICER_NAME, SQLControl.EnumDataType.dtStringN)
                                .AddField("DESIGNATION", Inventory_submitCont.DESIGNATION, SQLControl.EnumDataType.dtString)

                                .AddField("TRAN", Inventory_submitCont.TRAN, SQLControl.EnumDataType.dtString)
                                .AddField("BULAN", Inventory_submitCont.BULAN, SQLControl.EnumDataType.dtString)
                                .AddField("TAHUN", Inventory_submitCont.TAHUN, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Inventory_submitCont.ID) & "' AND KOD_INV = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Inventory_submitCont.KOD_INV) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("ID", Inventory_submitCont.ID, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("KOD_INV", Inventory_submitCont.KOD_INV, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Inventory_submitCont.ID) & "' AND KOD_INV = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Inventory_submitCont.KOD_INV) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/InventorySubmit", axExecute.Message & strSQL, axExecute.StackTrace)

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
                Inventory_submitCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Inventory_submitCont As Container.Inventory_submit, ByRef message As String) As Boolean
            Return Save(Inventory_submitCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal Inventory_submitCont As Container.Inventory_submit, ByRef message As String) As Boolean
            Return Save(Inventory_submitCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal Inventory_submitCont As Container.Inventory_submit, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Inventory_submitCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Inventory_submitInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Inventory_submitCont.ID) & "' AND KOD_INV = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Inventory_submitCont.KOD_INV) & "'")
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
                                strSQL = BuildUpdate("Inventory_submit WITH (ROWLOCK)", " SET Flag = 0" & _
                                 "' WHERE" & _
                                "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Inventory_submitCont.ID) & "' AND KOD_INV = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Inventory_submitCont.KOD_INV) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Inventory_submit WITH (ROWLOCK)", "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Inventory_submitCont.ID) & "' AND KOD_INV = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Inventory_submitCont.KOD_INV) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/InventorySubmit", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/InventorySubmit", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/InventorySubmit", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                Inventory_submitCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetInventory_submit(ByVal ID As System.Int32, ByVal KOD_INV As System.String) As Container.Inventory_submit
            Dim rInventory_submit As Container.Inventory_submit = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Inventory_submitInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, ID) & "' AND KOD_INV = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, KOD_INV) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rInventory_submit = New Container.Inventory_submit
                                rInventory_submit.ID = drRow.Item("ID")
                                rInventory_submit.KOD_INV = drRow.Item("KOD_INV")
                            Else
                                rInventory_submit = Nothing
                            End If
                        Else
                            rInventory_submit = Nothing
                        End If
                    End With
                End If
                Return rInventory_submit
            Catch ex As Exception
                Throw ex
            Finally
                rInventory_submit = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetInventory_submit(ByVal ID As System.Int32, ByVal KOD_INV As System.String, DecendingOrder As Boolean) As List(Of Container.Inventory_submit)
            Dim rInventory_submit As Container.Inventory_submit = Nothing
            Dim lstInventory_submit As List(Of Container.Inventory_submit) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Inventory_submitInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal ID As System.Int32, ByVal KOD_INV As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, ID) & "' AND KOD_INV = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, KOD_INV) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rInventory_submit = New Container.Inventory_submit
                                rInventory_submit.ID = drRow.Item("ID")
                                rInventory_submit.KOD_INV = drRow.Item("KOD_INV")
                            Next
                            lstInventory_submit.Add(rInventory_submit)
                        Else
                            rInventory_submit = Nothing
                        End If
                        Return lstInventory_submit
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rInventory_submit = Nothing
                lstInventory_submit = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetInventory_submitList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Inventory_submitInfo.MyInfo
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

        Public Overloads Function GetInventory_submitShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With Inventory_submitInfo.MyInfo
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

        Public Overloads Function GetInventory_submitListCustom(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Inventory_submitInfo.MyInfo

                    strSQL = "select * ,( SELECT TOP 1 right(convert(varchar, TARIKH_PENGELUAR, 106), 8) FROM inventori WHERE ID_INVENTORI = inventory_submit.ID order by TARIKH_PENGELUAR desc)as TransDateMY " & _
                    " from Inventory_submit " & _
                    " inner join bizentity on REPLACE(LTRIM(RTRIM(IDPREMIS)),' ','') = BizRegID " & _
                    " where  exists ( " & _
                    " select id_inventori,TARIKH_PENGELUAR from inventori where ID_INVENTORI =Inventory_submit.id and KOD_BT is not null " & _
                    " and KUANTITI_METRIK IS NOT NULL and TARIKH_PENGELUAR is not null) " & _
                    " and " & FieldCond
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetTransDate(Optional ByVal condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Inventory_submitInfo.MyInfo

                    strSQL = "SELECT distinct right(convert(varchar, TARIKH_PENGELUAR, 106), 8) as TransDateMY,Inventory_submit.ID " & _
                                " From inventory_submit,inventori   WHERE inventori.ID_INVENTORI = inventory_submit.ID and TARIKH_PENGELUAR is not null and " & condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetInventoryPrintHDR(Optional ByVal ID_INVENTORI As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Inventory_submitInfo.MyInfo

                    strSQL = "SELECT DISTINCT l.AccNo, d.ID_INVENTORI as RefNo, UPPER(DATENAME(m,d.TARIKH_PENGELUAR)) as Month, " & _
                            " YEAR(d.TARIKH_PENGELUAR) as YearCode, e.BizRegID, e.CompanyName, e.Address1, e.Address2, e.Address3, " & _
                            " e.Address4, s.StateDesc as State, h.STATUS_S as Status, h.OFFICER_NAME as ContactPerson, h.Designation, " & _
                            " h.Date " & _
                            " FROM inventori d " & _
                            " inner join inventory_submit h on h.ID=d.ID_INVENTORI and h.IDPREMIS=d.IDPREMIS and LTRIM(RTRIM(h.STATUS_S))='SUBMIT' " & _
                            " inner join BIZENTITY e on e.BizRegID=REPLACE(LTRIM(RTRIM(h.IDPREMIS)),' ','') " & _
                            " inner join BIZLOCATE l on l.BizRegID=e.BizRegID " & _
                            " inner join STATE s on s.StateCode=e.State and s.CountryCode='MY' " & _
                            " where d.KOD_BT Is Not NULL And d.KUANTITI_METRIK Is Not NULL "

                    If ID_INVENTORI IsNot Nothing AndAlso ID_INVENTORI <> "" Then
                        strSQL &= " AND ID_INVENTORI='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ID_INVENTORI) & "'"
                    End If

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
                EndSQLControl()
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetInventoryPrintDTL(Optional ByVal ID_INVENTORI As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Inventory_submitInfo.MyInfo

                    strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY d.ID_INVENTORI) AS [#],d.ID_INVENTORI as RefNo, d.TARIKH_PENGELUAR as Date, MONTH(d.TARIKH_PENGELUAR) as MthCode, " & _
                        " YEAR(d.TARIKH_PENGELUAR) as YearCode, e.BizRegID, d.KOD_BT as ItemCode, ISNULL(d.NAMA_BT,'') as ItemName, " & _
                        " ISNULL(d.B_F,0) as Opening, ISNULL(d.KUANTITI_METRIK,0) as QtyIn, ISNULL(d.WH_QUANTITY,0) as QtyHandling, " & _
                        " ISNULL(d.WH_METHOD,'') as CodeDesc, ISNULL(d.WH_PLACE,'') as Remark, ISNULL(d.C_F,0) as Closing " & _
                        " FROM inventori d " & _
                        " inner join inventory_submit h on h.ID=d.ID_INVENTORI and h.IDPREMIS=d.IDPREMIS and LTRIM(RTRIM(h.STATUS_S))='SUBMIT' " & _
                        " inner join BIZENTITY e on e.BizRegID=REPLACE(LTRIM(RTRIM(h.IDPREMIS)),' ','') " & _
                        " where d.KOD_BT IS NOT NULL and d.KUANTITI_METRIK IS NOT NULL "

                    If ID_INVENTORI IsNot Nothing AndAlso ID_INVENTORI <> "" Then
                        strSQL &= " AND ID_INVENTORI='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ID_INVENTORI) & "'"
                    End If

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
                EndSQLControl()
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region
    End Class


    Namespace Container
#Region "Class Container"
        Public Class Inventory_submit
            Public fID As System.String = "ID"
            Public fKOD_INV As System.String = "KOD_INV"
            Public fNORUJUKAN As System.String = "NORUJUKAN"
            Public fIDLOGIN As System.String = "IDLOGIN"
            Public fIDPREMIS As System.String = "IDPREMIS"
            Public fNAMAPREMIS As System.String = "NAMAPREMIS"
            Public fTARIKH_INVENTORI As System.String = "TARIKH_INVENTORI"
            Public fNEGERI As System.String = "NEGERI"
            Public fSTATUS_S As System.String = "STATUS_S"
            Public fTARIKH_WUJUD As System.String = "TARIKH_WUJUD"
            Public fWUJUD_PENGGUNA As System.String = "WUJUD_PENGGUNA"
            Public fTARIKH_KEMASKINI As System.String = "TARIKH_KEMASKINI"
            Public fKEMASKINI_PENGGUNA As System.String = "KEMASKINI_PENGGUNA"
            Public fOFFICER_NAME As System.String = "OFFICER_NAME"
            Public fDESIGNATION As System.String = "DESIGNATION"
            Public fDATE As System.String = "DATE"
            Public fTRAN As System.String = "TRAN"
            Public fBULAN As System.String = "BULAN"
            Public fTAHUN As System.String = "TAHUN"
            Public fCompanyName As System.String = "CompanyName"
            Public fTransDateMY As System.String = "TransDateMY"


            Protected _ID As System.Int32
            Protected _KOD_INV As System.String
            Private _NORUJUKAN As System.String
            Private _IDLOGIN As System.Int32
            Private _IDPREMIS As System.String
            Private _NAMAPREMIS As System.String
            Private _TARIKH_INVENTORI As System.Object
            Private _NEGERI As System.String
            Private _STATUS_S As System.String
            Private _TARIKH_WUJUD As System.Object
            Private _WUJUD_PENGGUNA As System.String
            Private _TARIKH_KEMASKINI As System.DateTime
            Private _KEMASKINI_PENGGUNA As System.String
            Private _OFFICER_NAME As System.String
            Private _DESIGNATION As System.String
            Private _DATE As System.Object
            Private _TRAN As System.String
            Private _BULAN As System.String
            Private _TAHUN As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ID As System.Int32
                Get
                    Return _ID
                End Get
                Set(ByVal Value As System.Int32)
                    _ID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property KOD_INV As System.String
                Get
                    Return _KOD_INV
                End Get
                Set(ByVal Value As System.String)
                    _KOD_INV = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property NORUJUKAN As System.String
                Get
                    Return _NORUJUKAN
                End Get
                Set(ByVal Value As System.String)
                    _NORUJUKAN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property IDLOGIN As System.Int32
                Get
                    Return _IDLOGIN
                End Get
                Set(ByVal Value As System.Int32)
                    _IDLOGIN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property IDPREMIS As System.String
                Get
                    Return _IDPREMIS
                End Get
                Set(ByVal Value As System.String)
                    _IDPREMIS = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property NAMAPREMIS As System.String
                Get
                    Return _NAMAPREMIS
                End Get
                Set(ByVal Value As System.String)
                    _NAMAPREMIS = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TARIKH_INVENTORI As System.Object
                Get
                    Return _TARIKH_INVENTORI
                End Get
                Set(ByVal Value As System.Object)
                    _TARIKH_INVENTORI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property NEGERI As System.String
                Get
                    Return _NEGERI
                End Get
                Set(ByVal Value As System.String)
                    _NEGERI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property STATUS_S As System.String
                Get
                    Return _STATUS_S
                End Get
                Set(ByVal Value As System.String)
                    _STATUS_S = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TARIKH_WUJUD As System.Object
                Get
                    Return _TARIKH_WUJUD
                End Get
                Set(ByVal Value As System.Object)
                    _TARIKH_WUJUD = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property WUJUD_PENGGUNA As System.String
                Get
                    Return _WUJUD_PENGGUNA
                End Get
                Set(ByVal Value As System.String)
                    _WUJUD_PENGGUNA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TARIKH_KEMASKINI As System.DateTime
                Get
                    Return _TARIKH_KEMASKINI
                End Get
                Set(ByVal Value As System.DateTime)
                    _TARIKH_KEMASKINI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KEMASKINI_PENGGUNA As System.String
                Get
                    Return _KEMASKINI_PENGGUNA
                End Get
                Set(ByVal Value As System.String)
                    _KEMASKINI_PENGGUNA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property OFFICER_NAME As System.String
                Get
                    Return _OFFICER_NAME
                End Get
                Set(ByVal Value As System.String)
                    _OFFICER_NAME = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property DESIGNATION As System.String
                Get
                    Return _DESIGNATION
                End Get
                Set(ByVal Value As System.String)
                    _DESIGNATION = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>


            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TRAN As System.String
                Get
                    Return _TRAN
                End Get
                Set(ByVal Value As System.String)
                    _TRAN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property BULAN As System.String
                Get
                    Return _BULAN
                End Get
                Set(ByVal Value As System.String)
                    _BULAN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TAHUN As System.String
                Get
                    Return _TAHUN
                End Get
                Set(ByVal Value As System.String)
                    _TAHUN = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class Inventory_submitInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "ID,(SELECT TOP 1 CompanyName FROM BIZENTITY WHERE BizRegID=Inventory_submit.IDPREMIS) as CompanyName,KOD_INV,NORUJUKAN,IDLOGIN,IDPREMIS,NAMAPREMIS,TARIKH_INVENTORI,NEGERI,STATUS_S,TARIKH_WUJUD,WUJUD_PENGGUNA,TARIKH_KEMASKINI,KEMASKINI_PENGGUNA,OFFICER_NAME,DESIGNATION,DATE,BULAN,TAHUN,( SELECT TOP 1 right(convert(varchar, TARIKH_PENGELUAR, 106), 8) FROM inventori WHERE ID_INVENTORI = inventory_submit.ID)as TransDateMY"
                .CheckFields = ""
                .TableName = "Inventory_submit WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "ID,KOD_INV,NORUJUKAN,IDLOGIN,IDPREMIS,NAMAPREMIS,TARIKH_INVENTORI,NEGERI,STATUS_S,TARIKH_WUJUD,WUJUD_PENGGUNA,TARIKH_KEMASKINI,KEMASKINI_PENGGUNA,OFFICER_NAME,DESIGNATION,DATE,BULAN,TAHUN,( SELECT TOP 1 right(convert(varchar, TARIKH_PENGELUAR, 106), 8) FROM inventori WHERE ID_INVENTORI = inventory_submit.ID)as TransDateMY"
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
    Public Class Inventory_submitScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ID"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "KOD_INV"
                .Length = 30
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "NORUJUKAN"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IDLOGIN"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "IDPREMIS"
                .Length = 30
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "NAMAPREMIS"
                .Length = 150
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "TARIKH_INVENTORI"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "NEGERI"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "STATUS_S"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "TARIKH_WUJUD"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WUJUD_PENGGUNA"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "TARIKH_KEMASKINI"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "KEMASKINI_PENGGUNA"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "OFFICER_NAME"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DESIGNATION"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "DATE"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TRAN"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BULAN"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TAHUN"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)

        End Sub

        Public ReadOnly Property ID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property KOD_INV As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property

        Public ReadOnly Property NORUJUKAN As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property IDLOGIN As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property IDPREMIS As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property NAMAPREMIS As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property TARIKH_INVENTORI As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property NEGERI As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property STATUS_S As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property TARIKH_WUJUD As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property WUJUD_PENGGUNA As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property TARIKH_KEMASKINI As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property KEMASKINI_PENGGUNA As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property OFFICER_NAME As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property DESIGNATION As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property

        Public ReadOnly Property TRAN As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property BULAN As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property TAHUN As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace