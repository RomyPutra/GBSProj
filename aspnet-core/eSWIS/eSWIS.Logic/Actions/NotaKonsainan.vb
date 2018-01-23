Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class Nota_Konsainan
        Inherits Core.CoreControl
        Private Nota_konsainanInfo As Nota_konsainanInfo = New Nota_konsainanInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal conn As String)
            ConnectionString = conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal Nota_konsainanCont As Container.Nota_konsainan, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Nota_konsainanCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Nota_konsainanInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Nota_konsainanCont.ID) & "'")
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
                                .TableName = "Nota_konsainan WITH (ROWLOCK)"
                                .AddField("NEGERI", Nota_konsainanCont.NEGERI, SQLControl.EnumDataType.dtString)
                                .AddField("DAERAH", Nota_konsainanCont.DAERAH, SQLControl.EnumDataType.dtString)
                                .AddField("JENIS_TRANS", Nota_konsainanCont.JENIS_TRANS, SQLControl.EnumDataType.dtString)
                                .AddField("IDPREMIS", Nota_konsainanCont.IDPREMIS, SQLControl.EnumDataType.dtString)
                                .AddField("NOFAIL_JAS", Nota_konsainanCont.NOFAIL_JAS, SQLControl.EnumDataType.dtString)
                                .AddField("SIC", Nota_konsainanCont.SIC, SQLControl.EnumDataType.dtString)
                                .AddField("ID_PENGELUAR_BT", Nota_konsainanCont.ID_PENGELUAR_BT, SQLControl.EnumDataType.dtString)
                                .AddField("NOSIRI_BT", Nota_konsainanCont.NOSIRI_BT, SQLControl.EnumDataType.dtString)
                                .AddField("KOD_BT", Nota_konsainanCont.KOD_BT, SQLControl.EnumDataType.dtString)
                                .AddField("NAMA_BT", Nota_konsainanCont.NAMA_BT, SQLControl.EnumDataType.dtStringN)
                                .AddField("KOMPONEN_BT", Nota_konsainanCont.KOMPONEN_BT, SQLControl.EnumDataType.dtStringN)
                                .AddField("ASAL_BT", Nota_konsainanCont.ASAL_BT, SQLControl.EnumDataType.dtStringN)
                                .AddField("KOD_ASAL_BT", Nota_konsainanCont.KOD_ASAL_BT, SQLControl.EnumDataType.dtString)
                                .AddField("BENTUK_BT", Nota_konsainanCont.BENTUK_BT, SQLControl.EnumDataType.dtString)
                                .AddField("PAKEJ_BT", Nota_konsainanCont.PAKEJ_BT, SQLControl.EnumDataType.dtString)
                                .AddField("PAKEJ_LAIN", Nota_konsainanCont.PAKEJ_LAIN, SQLControl.EnumDataType.dtString)
                                .AddField("KUANTITI_PAKEJ", Nota_konsainanCont.KUANTITI_PAKEJ, SQLControl.EnumDataType.dtString)
                                .AddField("KUANTITI_BT_METRIK", Nota_konsainanCont.KUANTITI_BT_METRIK, SQLControl.EnumDataType.dtCustom)
                                .AddField("KUANTITI_BT_M3", Nota_konsainanCont.KUANTITI_BT_M3, SQLControl.EnumDataType.dtCustom)
                                .AddField("UNIT", Nota_konsainanCont.UNIT, SQLControl.EnumDataType.dtString)
                                .AddField("KOS_PERAWATAN", Nota_konsainanCont.KOS_PERAWATAN, SQLControl.EnumDataType.dtString)
                                .AddField("ALAMAT", Nota_konsainanCont.ALAMAT, SQLControl.EnumDataType.dtStringN)
                                .AddField("BANDAR", Nota_konsainanCont.BANDAR, SQLControl.EnumDataType.dtString)
                                .AddField("POSKOD", Nota_konsainanCont.POSKOD, SQLControl.EnumDataType.dtString)
                                .AddField("PEGAWAI", Nota_konsainanCont.PEGAWAI, SQLControl.EnumDataType.dtStringN)
                                .AddField("JAWATAN", Nota_konsainanCont.JAWATAN, SQLControl.EnumDataType.dtStringN)
                                .AddField("TELEFON", Nota_konsainanCont.TELEFON, SQLControl.EnumDataType.dtString)
                                .AddField("FAKS", Nota_konsainanCont.FAKS, SQLControl.EnumDataType.dtString)
                                .AddField("EMAIL", Nota_konsainanCont.EMAIL, SQLControl.EnumDataType.dtStringN)
                                .AddField("NAMA_PENGELUAR", Nota_konsainanCont.NAMA_PENGELUAR, SQLControl.EnumDataType.dtStringN)
                                .AddField("TARIKH_HANTAR", Nota_konsainanCont.TARIKH_HANTAR, SQLControl.EnumDataType.dtCustom)
                                .AddField("MASA_HANTAR", Nota_konsainanCont.MASA_HANTAR, SQLControl.EnumDataType.dtCustom)
                                .AddField("PEGAWAI_HANTAR", Nota_konsainanCont.PEGAWAI_HANTAR, SQLControl.EnumDataType.dtStringN)
                                .AddField("R_ID_DESTINASI", Nota_konsainanCont.R_ID_DESTINASI, SQLControl.EnumDataType.dtString)
                                .AddField("R_NAMA_DESTINASI", Nota_konsainanCont.R_NAMA_DESTINASI, SQLControl.EnumDataType.dtStringN)
                                .AddField("R_ALAMAT", Nota_konsainanCont.R_ALAMAT, SQLControl.EnumDataType.dtStringN)
                                .AddField("R_DAERAH", Nota_konsainanCont.R_DAERAH, SQLControl.EnumDataType.dtString)
                                .AddField("R_NEGERI", Nota_konsainanCont.R_NEGERI, SQLControl.EnumDataType.dtString)
                                .AddField("MOHON_BATAL", Nota_konsainanCont.MOHON_BATAL, SQLControl.EnumDataType.dtCustom)
                                .AddField("TARIKH_MOHON", Nota_konsainanCont.TARIKH_MOHON, SQLControl.EnumDataType.dtCustom)
                                .AddField("T1_ID_TRANSPORTER", Nota_konsainanCont.T1_ID_TRANSPORTER, SQLControl.EnumDataType.dtString)
                                .AddField("T1_NAME_TRANSPORTER", Nota_konsainanCont.T1_NAME_TRANSPORTER, SQLControl.EnumDataType.dtStringN)
                                .AddField("T1_NAME_RESPONSIBLE", Nota_konsainanCont.T1_NAME_RESPONSIBLE, SQLControl.EnumDataType.dtStringN)
                                .AddField("T1_TELEPHONE", Nota_konsainanCont.T1_TELEPHONE, SQLControl.EnumDataType.dtString)
                                .AddField("T1_FAXS", Nota_konsainanCont.T1_FAXS, SQLControl.EnumDataType.dtString)
                                .AddField("T1_EMAIL", Nota_konsainanCont.T1_EMAIL, SQLControl.EnumDataType.dtStringN)
                                .AddField("T1_VEHICLE_NO", Nota_konsainanCont.T1_VEHICLE_NO, SQLControl.EnumDataType.dtString)
                                .AddField("T1_DRIVER_NAME", Nota_konsainanCont.T1_DRIVER_NAME, SQLControl.EnumDataType.dtStringN)
                                .AddField("T1_NRIC", Nota_konsainanCont.T1_NRIC, SQLControl.EnumDataType.dtString)
                                .AddField("T1_PENSTORAN", Nota_konsainanCont.T1_PENSTORAN, SQLControl.EnumDataType.dtString)
                                .AddField("T1_TARIKH_TERIMA", Nota_konsainanCont.T1_TARIKH_TERIMA, SQLControl.EnumDataType.dtCustom)
                                .AddField("T1_MASA_TERIMA", Nota_konsainanCont.T1_MASA_TERIMA, SQLControl.EnumDataType.dtCustom)
                                .AddField("T1_PEGAWAI", Nota_konsainanCont.T1_PEGAWAI, SQLControl.EnumDataType.dtStringN)
                                .AddField("T1_NORUJUKAN", Nota_konsainanCont.T1_NORUJUKAN, SQLControl.EnumDataType.dtString)
                                .AddField("T1_NAMAPREMIS", Nota_konsainanCont.T1_NAMAPREMIS, SQLControl.EnumDataType.dtStringN)
                                .AddField("T1_VESSEL_NAME", Nota_konsainanCont.T1_VESSEL_NAME, SQLControl.EnumDataType.dtStringN)
                                .AddField("T1_VESSEL_PERSON", Nota_konsainanCont.T1_VESSEL_PERSON, SQLControl.EnumDataType.dtStringN)
                                .AddField("T1_VESSEL_PHONE", Nota_konsainanCont.T1_VESSEL_PHONE, SQLControl.EnumDataType.dtString)
                                .AddField("T1_VESSEL_FAX", Nota_konsainanCont.T1_VESSEL_FAX, SQLControl.EnumDataType.dtString)
                                .AddField("T1_VESEL_EMAIL", Nota_konsainanCont.T1_VESEL_EMAIL, SQLControl.EnumDataType.dtStringN)
                                .AddField("T1_PENSTORAN1", Nota_konsainanCont.T1_PENSTORAN1, SQLControl.EnumDataType.dtString)
                                .AddField("T1_TARIKH_TERIMA1", Nota_konsainanCont.T1_TARIKH_TERIMA1, SQLControl.EnumDataType.dtCustom)
                                .AddField("T1_MASA_TERIMA1", Nota_konsainanCont.T1_MASA_TERIMA1, SQLControl.EnumDataType.dtCustom)
                                .AddField("T1_PEGAWAI1", Nota_konsainanCont.T1_PEGAWAI1, SQLControl.EnumDataType.dtString)
                                .AddField("T_ID_PENGANGKUT", Nota_konsainanCont.T_ID_PENGANGKUT, SQLControl.EnumDataType.dtString)
                                .AddField("T_NAMA_PENGANGKUT", Nota_konsainanCont.T_NAMA_PENGANGKUT, SQLControl.EnumDataType.dtStringN)
                                .AddField("T_PEGAWAI_PENGANGKUT", Nota_konsainanCont.T_PEGAWAI_PENGANGKUT, SQLControl.EnumDataType.dtStringN)
                                .AddField("T_JAWATAN_PENGANGKUT", Nota_konsainanCont.T_JAWATAN_PENGANGKUT, SQLControl.EnumDataType.dtStringN)
                                .AddField("T_TELEFON_PENGANGKUT", Nota_konsainanCont.T_TELEFON_PENGANGKUT, SQLControl.EnumDataType.dtString)
                                .AddField("T_FAKS_PENGANGKUT", Nota_konsainanCont.T_FAKS_PENGANGKUT, SQLControl.EnumDataType.dtString)
                                .AddField("T_EMAIL", Nota_konsainanCont.T_EMAIL, SQLControl.EnumDataType.dtStringN)
                                .AddField("T_KENDERAAN", Nota_konsainanCont.T_KENDERAAN, SQLControl.EnumDataType.dtString)
                                .AddField("T_PEMANDU", Nota_konsainanCont.T_PEMANDU, SQLControl.EnumDataType.dtStringN)
                                .AddField("T_NOKP", Nota_konsainanCont.T_NOKP, SQLControl.EnumDataType.dtString)
                                .AddField("T_STORAN_SEMENTARA", Nota_konsainanCont.T_STORAN_SEMENTARA, SQLControl.EnumDataType.dtStringN)
                                .AddField("T_TARIKH", Nota_konsainanCont.T_TARIKH, SQLControl.EnumDataType.dtCustom)
                                .AddField("T_MASA", Nota_konsainanCont.T_MASA, SQLControl.EnumDataType.dtCustom)
                                .AddField("T_PEGAWAI", Nota_konsainanCont.T_PEGAWAI, SQLControl.EnumDataType.dtStringN)
                                .AddField("R_PEGAWAI_PENERIMA", Nota_konsainanCont.R_PEGAWAI_PENERIMA, SQLControl.EnumDataType.dtStringN)
                                .AddField("R_TELEFON_PENERIMA", Nota_konsainanCont.R_TELEFON_PENERIMA, SQLControl.EnumDataType.dtString)
                                .AddField("R_FAKS_PENERIMA", Nota_konsainanCont.R_FAKS_PENERIMA, SQLControl.EnumDataType.dtString)
                                .AddField("R_EMAIL", Nota_konsainanCont.R_EMAIL, SQLControl.EnumDataType.dtStringN)
                                .AddField("RP_JENIS_OPERASI", Nota_konsainanCont.RP_JENIS_OPERASI, SQLControl.EnumDataType.dtString)
                                .AddField("RP_OPERASI_LAIN", Nota_konsainanCont.RP_OPERASI_LAIN, SQLControl.EnumDataType.dtString)
                                .AddField("RP_KUANTITI_TERIMA_METRIK", Nota_konsainanCont.RP_KUANTITI_TERIMA_METRIK, SQLControl.EnumDataType.dtCustom)
                                .AddField("RP_KUANTITI_TERIMA_M3", Nota_konsainanCont.RP_KUANTITI_TERIMA_M3, SQLControl.EnumDataType.dtCustom)
                                .AddField("RP_TARIKH_TERIMA", Nota_konsainanCont.RP_TARIKH_TERIMA, SQLControl.EnumDataType.dtCustom)
                                .AddField("RP_TARIKH_TOLAK", Nota_konsainanCont.RP_TARIKH_TOLAK, SQLControl.EnumDataType.dtCustom)
                                .AddField("RP_CATATAN_PENERIMA", Nota_konsainanCont.RP_CATATAN_PENERIMA, SQLControl.EnumDataType.dtStringN)
                                .AddField("STATUS", Nota_konsainanCont.STATUS, SQLControl.EnumDataType.dtString)
                                .AddField("IDLOGIN", Nota_konsainanCont.IDLOGIN, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TARIKH_WUJUD", Nota_konsainanCont.TARIKH_WUJUD, SQLControl.EnumDataType.dtCustom)
                                .AddField("WUJUD_PENGGUNA", Nota_konsainanCont.WUJUD_PENGGUNA, SQLControl.EnumDataType.dtStringN)
                                .AddField("TARIKH_KEMASKINI", Nota_konsainanCont.TARIKH_KEMASKINI, SQLControl.EnumDataType.dtDateTime)
                                .AddField("KEMASKINI_PENGGUNA", Nota_konsainanCont.KEMASKINI_PENGGUNA, SQLControl.EnumDataType.dtStringN)
                                .AddField("KOD_NK", Nota_konsainanCont.KOD_NK, SQLControl.EnumDataType.dtString)
                                .AddField("STAT", Nota_konsainanCont.STAT, SQLControl.EnumDataType.dtString)
                                .AddField("REMAINDER", Nota_konsainanCont.REMAINDER, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TRAN", Nota_konsainanCont.TRAN, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Nota_konsainanCont.ID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("ID", Nota_konsainanCont.ID, SQLControl.EnumDataType.dtNumeric)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Nota_konsainanCont.ID) & "'")
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
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotaKonsainan", axExecute.Message & strSQL, axExecute.StackTrace)

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
                Nota_konsainanCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Nota_konsainanCont As Container.Nota_konsainan, ByRef message As String) As Boolean
            Return Save(Nota_konsainanCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal Nota_konsainanCont As Container.Nota_konsainan, ByRef message As String) As Boolean
            Return Save(Nota_konsainanCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal Nota_konsainanCont As Container.Nota_konsainan, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Nota_konsainanCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Nota_konsainanInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Nota_konsainanCont.ID) & "'")
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
                                strSQL = BuildUpdate(Nota_konsainanInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " WHERE" & _
                                " ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Nota_konsainanCont.ID) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Nota_konsainanInfo.MyInfo.TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Nota_konsainanCont.ID) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotaKonsainan", exExecute.Message & strSQL, exExecute.StackTrace)

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
                Nota_konsainanCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetNota_Konsainan(ByVal ID As System.Int32) As Container.Nota_konsainan
            Dim rNota_konsainan As Container.Nota_konsainan = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Nota_konsainanInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rNota_konsainan = New Container.Nota_konsainan
                                rNota_konsainan.ID = drRow.Item("ID")
                            Else
                                rNota_konsainan = Nothing
                            End If
                        Else
                            rNota_konsainan = Nothing
                        End If
                    End With
                End If
                Return rNota_konsainan
            Catch ex As Exception
                Throw ex
            Finally
                rNota_konsainan = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetNota_Konsainan(ByVal ID As System.Int32, DecendingOrder As Boolean) As List(Of Container.Nota_konsainan)
            Dim rNota_konsainan As Container.Nota_konsainan = Nothing
            Dim lstNota_konsainan As List(Of Container.Nota_konsainan) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Nota_konsainanInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal ID As System.Int32 DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ID) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rNota_konsainan = New Container.Nota_konsainan
                                rNota_konsainan.ID = drRow.Item("ID")
                            Next
                            lstNota_konsainan.Add(rNota_konsainan)
                        Else
                            rNota_konsainan = Nothing
                        End If
                        Return lstNota_konsainan
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rNota_konsainan = Nothing
                lstNota_konsainan = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetNota_KonsainanList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Nota_konsainanInfo.MyInfo
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

        Public Overloads Function GetNota_KonsainanShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With Nota_konsainanInfo.MyInfo
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

        Public Overloads Function GetInventoryAdditionHistory(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With Nota_konsainanInfo.MyInfo
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

        Public Overloads Function GetConsignmentNoteHistory(ByVal ShortListing As String) As Data.DataTable
            If StartConnection() = True Then
                With Nota_konsainanInfo.MyInfo
                    If ShortListing <> "" Then
                        strSQL = "select [TARIKH_WUJUD] as TargetReceiveDate,[TARIKH_HANTAR] as TargetTransportDate,'' as ReferID,[ID] as ContractNo,[KOD_BT] as WasteCode,[KUANTITI_BT_METRIK] as Qty,[TARIKH_HANTAR] as TransportDate,[T_NAMA_PENGANGKUT] as TransporterName,[STATUS] as 'Status',[TARIKH_WUJUD] as ReceiveDate,[TARIKH_KEMASKINI] as TransDate,(select top 1 namapremis from [ecn].[dbo].[login] where idpremis=nota_konsainan.IDPREMIS) as CompanyName,(select top 1 namapremis from [ecn].[dbo].[login] where idpremis=nota_konsainan.R_ID_DESTINASI) as ReceiverName FROM [ecn].[dbo].[nota_konsainan] WHERE " & ShortListing & ""
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

        Public Overloads Function GetHistoryCNCustomList(Optional ByVal ID As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Nota_konsainanInfo.MyInfo

                    strSQL = "SELECT NT.ID,ALAMAT,KOD_ASAL_BT,KUANTITI_PAKEJ,KOS_PERAWATAN, NT.STATUS as STATUS, TARIKH_HANTAR,NOSIRI_BT,ASAL_BT,BENTUK_BT,PAKEJ_BT,NT.EMAIL as EMAIL,KOMPONEN_BT,NAMA_BT,KUANTITI_BT_METRIK, " & _
                        "PEGAWAI,JAWATAN,FAKS,TELEFON,T_NAMA_PENGANGKUT,T_PEGAWAI_PENGANGKUT, T_JAWATAN_PENGANGKUT, T_TELEFON_PENGANGKUT," & _
                        "T_FAKS_PENGANGKUT, T_EMAIL, T_KENDERAAN, T_PEMANDU, T_NOKP, T_STORAN_SEMENTARA, R_NAMA_DESTINASI, R_TELEFON_PENERIMA, " & _
                        "R_PEGAWAI_PENERIMA, R_FAKS_PENERIMA, R_EMAIL, RP_JENIS_OPERASI, RP_OPERASI_LAIN, RP_KUANTITI_TERIMA_METRIK, TARIKH_WUJUD,RP_CATATAN_PENERIMA as ReceiveRemark, " & _
                        " (NT.KOD_BT+' - '+i.ItmDesc) as WasteCode, ct.Address1 as TransporterAddress, cr.Address1 as ReceiverAddress, case when nt.nama_pengeluar is null or nt.nama_pengeluar='' then nt.idpremis else nt.nama_pengeluar end as WG, " & _
                        "rc.TYPE_OPERATION, rc.OTHER_OPERATION, rc.QTY_RECEIVED_METRIC, " & _
                        "rc.CATATAN, rc.DATE_RECEIVED, rc.DATE_REJECT" & _
                        " FROM Nota_konsainan NT" & _
                        " LEFT JOIN ITEM i ON i.ItemCode=NT.KOD_BT " & _
                        " LEFT JOIN BIZENTITY ct on ct.BizRegID=NT.T_id_PENGANGKUT " & _
                        " LEFT JOIN BIZENTITY cr on cr.BizRegID=NT.R_ID_DESTINASI " & _
                        " LEFT JOIN BIZENTITY cg on cg.BizRegID=NT.IDPREMIS " & _
                        " LEFT JOIN RECEIVER rc on NT.KOD_NK=rc.KOD_NK"

                    If Not ID Is Nothing And ID <> "" Then
                        strSQL &= " WHERE NT.ID='" & ID & "'"
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


    Namespace Container

#Region "Class Container"
        Public Class Nota_konsainan
            Public fID As System.String = "ID"
            Public fNEGERI As System.String = "NEGERI"
            Public fDAERAH As System.String = "DAERAH"
            Public fJENIS_TRANS As System.String = "JENIS_TRANS"
            Public fIDPREMIS As System.String = "IDPREMIS"
            Public fNOFAIL_JAS As System.String = "NOFAIL_JAS"
            Public fSIC As System.String = "SIC"
            Public fID_PENGELUAR_BT As System.String = "ID_PENGELUAR_BT"
            Public fNOSIRI_BT As System.String = "NOSIRI_BT"
            Public fKOD_BT As System.String = "KOD_BT"
            Public fNAMA_BT As System.String = "NAMA_BT"
            Public fKOMPONEN_BT As System.String = "KOMPONEN_BT"
            Public fASAL_BT As System.String = "ASAL_BT"
            Public fKOD_ASAL_BT As System.String = "KOD_ASAL_BT"
            Public fBENTUK_BT As System.String = "BENTUK_BT"
            Public fPAKEJ_BT As System.String = "PAKEJ_BT"
            Public fPAKEJ_LAIN As System.String = "PAKEJ_LAIN"
            Public fKUANTITI_PAKEJ As System.String = "KUANTITI_PAKEJ"
            Public fKUANTITI_BT_METRIK As System.String = "KUANTITI_BT_METRIK"
            Public fKUANTITI_BT_M3 As System.String = "KUANTITI_BT_M3"
            Public fUNIT As System.String = "UNIT"
            Public fKOS_PERAWATAN As System.String = "KOS_PERAWATAN"
            Public fALAMAT As System.String = "ALAMAT"
            Public fBANDAR As System.String = "BANDAR"
            Public fPOSKOD As System.String = "POSKOD"
            Public fPEGAWAI As System.String = "PEGAWAI"
            Public fJAWATAN As System.String = "JAWATAN"
            Public fTELEFON As System.String = "TELEFON"
            Public fFAKS As System.String = "FAKS"
            Public fEMAIL As System.String = "EMAIL"
            Public fNAMA_PENGELUAR As System.String = "NAMA_PENGELUAR"
            Public fTARIKH_HANTAR As System.String = "TARIKH_HANTAR"
            Public fMASA_HANTAR As System.String = "MASA_HANTAR"
            Public fPEGAWAI_HANTAR As System.String = "PEGAWAI_HANTAR"
            Public fR_ID_DESTINASI As System.String = "R_ID_DESTINASI"
            Public fR_NAMA_DESTINASI As System.String = "R_NAMA_DESTINASI"
            Public fR_ALAMAT As System.String = "R_ALAMAT"
            Public fR_DAERAH As System.String = "R_DAERAH"
            Public fR_NEGERI As System.String = "R_NEGERI"
            Public fMOHON_BATAL As System.String = "MOHON_BATAL"
            Public fTARIKH_MOHON As System.String = "TARIKH_MOHON"
            Public fT1_ID_TRANSPORTER As System.String = "T1_ID_TRANSPORTER"
            Public fT1_NAME_TRANSPORTER As System.String = "T1_NAME_TRANSPORTER"
            Public fT1_NAME_RESPONSIBLE As System.String = "T1_NAME_RESPONSIBLE"
            Public fT1_TELEPHONE As System.String = "T1_TELEPHONE"
            Public fT1_FAXS As System.String = "T1_FAXS"
            Public fT1_EMAIL As System.String = "T1_EMAIL"
            Public fT1_VEHICLE_NO As System.String = "T1_VEHICLE_NO"
            Public fT1_DRIVER_NAME As System.String = "T1_DRIVER_NAME"
            Public fT1_NRIC As System.String = "T1_NRIC"
            Public fT1_PENSTORAN As System.String = "T1_PENSTORAN"
            Public fT1_TARIKH_TERIMA As System.String = "T1_TARIKH_TERIMA"
            Public fT1_MASA_TERIMA As System.String = "T1_MASA_TERIMA"
            Public fT1_PEGAWAI As System.String = "T1_PEGAWAI"
            Public fT1_NORUJUKAN As System.String = "T1_NORUJUKAN"
            Public fT1_NAMAPREMIS As System.String = "T1_NAMAPREMIS"
            Public fT1_VESSEL_NAME As System.String = "T1_VESSEL_NAME"
            Public fT1_VESSEL_PERSON As System.String = "T1_VESSEL_PERSON"
            Public fT1_VESSEL_PHONE As System.String = "T1_VESSEL_PHONE"
            Public fT1_VESSEL_FAX As System.String = "T1_VESSEL_FAX"
            Public fT1_VESEL_EMAIL As System.String = "T1_VESEL_EMAIL"
            Public fT1_PENSTORAN1 As System.String = "T1_PENSTORAN1"
            Public fT1_TARIKH_TERIMA1 As System.String = "T1_TARIKH_TERIMA1"
            Public fT1_MASA_TERIMA1 As System.String = "T1_MASA_TERIMA1"
            Public fT1_PEGAWAI1 As System.String = "T1_PEGAWAI1"
            Public fT_ID_PENGANGKUT As System.String = "T_ID_PENGANGKUT"
            Public fT_NAMA_PENGANGKUT As System.String = "T_NAMA_PENGANGKUT"
            Public fT_PEGAWAI_PENGANGKUT As System.String = "T_PEGAWAI_PENGANGKUT"
            Public fT_JAWATAN_PENGANGKUT As System.String = "T_JAWATAN_PENGANGKUT"
            Public fT_TELEFON_PENGANGKUT As System.String = "T_TELEFON_PENGANGKUT"
            Public fT_FAKS_PENGANGKUT As System.String = "T_FAKS_PENGANGKUT"
            Public fT_EMAIL As System.String = "T_EMAIL"
            Public fT_KENDERAAN As System.String = "T_KENDERAAN"
            Public fT_PEMANDU As System.String = "T_PEMANDU"
            Public fT_NOKP As System.String = "T_NOKP"
            Public fT_STORAN_SEMENTARA As System.String = "T_STORAN_SEMENTARA"
            Public fT_TARIKH As System.String = "T_TARIKH"
            Public fT_MASA As System.String = "T_MASA"
            Public fT_PEGAWAI As System.String = "T_PEGAWAI"
            Public fR_PEGAWAI_PENERIMA As System.String = "R_PEGAWAI_PENERIMA"
            Public fR_TELEFON_PENERIMA As System.String = "R_TELEFON_PENERIMA"
            Public fR_FAKS_PENERIMA As System.String = "R_FAKS_PENERIMA"
            Public fR_EMAIL As System.String = "R_EMAIL"
            Public fRP_JENIS_OPERASI As System.String = "RP_JENIS_OPERASI"
            Public fRP_OPERASI_LAIN As System.String = "RP_OPERASI_LAIN"
            Public fRP_KUANTITI_TERIMA_METRIK As System.String = "RP_KUANTITI_TERIMA_METRIK"
            Public fRP_KUANTITI_TERIMA_M3 As System.String = "RP_KUANTITI_TERIMA_M3"
            Public fRP_TARIKH_TERIMA As System.String = "RP_TARIKH_TERIMA"
            Public fRP_TARIKH_TOLAK As System.String = "RP_TARIKH_TOLAK"
            Public fRP_CATATAN_PENERIMA As System.String = "RP_CATATAN_PENERIMA"
            Public fSTATUS As System.String = "STATUS"
            Public fIDLOGIN As System.String = "IDLOGIN"
            Public fTARIKH_WUJUD As System.String = "TARIKH_WUJUD"
            Public fWUJUD_PENGGUNA As System.String = "WUJUD_PENGGUNA"
            Public fTARIKH_KEMASKINI As System.String = "TARIKH_KEMASKINI"
            Public fKEMASKINI_PENGGUNA As System.String = "KEMASKINI_PENGGUNA"
            Public fKOD_NK As System.String = "KOD_NK"
            Public fSTAT As System.String = "STAT"
            Public fREMAINDER As System.String = "REMAINDER"
            Public fTRAN As System.String = "TRAN"

            Protected _ID As System.Int32
            Private _NEGERI As System.String
            Private _DAERAH As System.String
            Private _JENIS_TRANS As System.String
            Private _IDPREMIS As System.String
            Private _NOFAIL_JAS As System.String
            Private _SIC As System.String
            Private _ID_PENGELUAR_BT As System.String
            Private _NOSIRI_BT As System.String
            Private _KOD_BT As System.String
            Private _NAMA_BT As System.String
            Private _KOMPONEN_BT As System.String
            Private _ASAL_BT As System.String
            Private _KOD_ASAL_BT As System.String
            Private _BENTUK_BT As System.String
            Private _PAKEJ_BT As System.String
            Private _PAKEJ_LAIN As System.String
            Private _KUANTITI_PAKEJ As System.String
            Private _KUANTITI_BT_METRIK As System.Single
            Private _KUANTITI_BT_M3 As System.Single
            Private _UNIT As System.String
            Private _KOS_PERAWATAN As System.String
            Private _ALAMAT As System.String
            Private _BANDAR As System.String
            Private _POSKOD As System.String
            Private _PEGAWAI As System.String
            Private _JAWATAN As System.String
            Private _TELEFON As System.String
            Private _FAKS As System.String
            Private _EMAIL As System.String
            Private _NAMA_PENGELUAR As System.String
            Private _TARIKH_HANTAR As System.Object
            Private _MASA_HANTAR As System.Object
            Private _PEGAWAI_HANTAR As System.String
            Private _R_ID_DESTINASI As System.String
            Private _R_NAMA_DESTINASI As System.String
            Private _R_ALAMAT As System.String
            Private _R_DAERAH As System.String
            Private _R_NEGERI As System.String
            Private _MOHON_BATAL As System.Int16
            Private _TARIKH_MOHON As System.Object
            Private _T1_ID_TRANSPORTER As System.String
            Private _T1_NAME_TRANSPORTER As System.String
            Private _T1_NAME_RESPONSIBLE As System.String
            Private _T1_TELEPHONE As System.String
            Private _T1_FAXS As System.String
            Private _T1_EMAIL As System.String
            Private _T1_VEHICLE_NO As System.String
            Private _T1_DRIVER_NAME As System.String
            Private _T1_NRIC As System.String
            Private _T1_PENSTORAN As System.String
            Private _T1_TARIKH_TERIMA As System.Object
            Private _T1_MASA_TERIMA As System.Object
            Private _T1_PEGAWAI As System.String
            Private _T1_NORUJUKAN As System.String
            Private _T1_NAMAPREMIS As System.String
            Private _T1_VESSEL_NAME As System.String
            Private _T1_VESSEL_PERSON As System.String
            Private _T1_VESSEL_PHONE As System.String
            Private _T1_VESSEL_FAX As System.String
            Private _T1_VESEL_EMAIL As System.String
            Private _T1_PENSTORAN1 As System.String
            Private _T1_TARIKH_TERIMA1 As System.Object
            Private _T1_MASA_TERIMA1 As System.Object
            Private _T1_PEGAWAI1 As System.String
            Private _T_ID_PENGANGKUT As System.String
            Private _T_NAMA_PENGANGKUT As System.String
            Private _T_PEGAWAI_PENGANGKUT As System.String
            Private _T_JAWATAN_PENGANGKUT As System.String
            Private _T_TELEFON_PENGANGKUT As System.String
            Private _T_FAKS_PENGANGKUT As System.String
            Private _T_EMAIL As System.String
            Private _T_KENDERAAN As System.String
            Private _T_PEMANDU As System.String
            Private _T_NOKP As System.String
            Private _T_STORAN_SEMENTARA As System.String
            Private _T_TARIKH As System.Object
            Private _T_MASA As System.Object
            Private _T_PEGAWAI As System.String
            Private _R_PEGAWAI_PENERIMA As System.String
            Private _R_TELEFON_PENERIMA As System.String
            Private _R_FAKS_PENERIMA As System.String
            Private _R_EMAIL As System.String
            Private _RP_JENIS_OPERASI As System.String
            Private _RP_OPERASI_LAIN As System.String
            Private _RP_KUANTITI_TERIMA_METRIK As System.Single
            Private _RP_KUANTITI_TERIMA_M3 As System.Single
            Private _RP_TARIKH_TERIMA As System.Object
            Private _RP_TARIKH_TOLAK As System.Object
            Private _RP_CATATAN_PENERIMA As System.String
            Private _STATUS As System.String
            Private _IDLOGIN As System.Int32
            Private _TARIKH_WUJUD As System.Object
            Private _WUJUD_PENGGUNA As System.String
            Private _TARIKH_KEMASKINI As System.DateTime
            Private _KEMASKINI_PENGGUNA As System.String
            Private _KOD_NK As System.String
            Private _STAT As System.String
            Private _REMAINDER As System.Int32
            Private _TRAN As System.String

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
            Public Property DAERAH As System.String
                Get
                    Return _DAERAH
                End Get
                Set(ByVal Value As System.String)
                    _DAERAH = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property JENIS_TRANS As System.String
                Get
                    Return _JENIS_TRANS
                End Get
                Set(ByVal Value As System.String)
                    _JENIS_TRANS = Value
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
            Public Property NOFAIL_JAS As System.String
                Get
                    Return _NOFAIL_JAS
                End Get
                Set(ByVal Value As System.String)
                    _NOFAIL_JAS = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property SIC As System.String
                Get
                    Return _SIC
                End Get
                Set(ByVal Value As System.String)
                    _SIC = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ID_PENGELUAR_BT As System.String
                Get
                    Return _ID_PENGELUAR_BT
                End Get
                Set(ByVal Value As System.String)
                    _ID_PENGELUAR_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property NOSIRI_BT As System.String
                Get
                    Return _NOSIRI_BT
                End Get
                Set(ByVal Value As System.String)
                    _NOSIRI_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KOD_BT As System.String
                Get
                    Return _KOD_BT
                End Get
                Set(ByVal Value As System.String)
                    _KOD_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property NAMA_BT As System.String
                Get
                    Return _NAMA_BT
                End Get
                Set(ByVal Value As System.String)
                    _NAMA_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KOMPONEN_BT As System.String
                Get
                    Return _KOMPONEN_BT
                End Get
                Set(ByVal Value As System.String)
                    _KOMPONEN_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ASAL_BT As System.String
                Get
                    Return _ASAL_BT
                End Get
                Set(ByVal Value As System.String)
                    _ASAL_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KOD_ASAL_BT As System.String
                Get
                    Return _KOD_ASAL_BT
                End Get
                Set(ByVal Value As System.String)
                    _KOD_ASAL_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property BENTUK_BT As System.String
                Get
                    Return _BENTUK_BT
                End Get
                Set(ByVal Value As System.String)
                    _BENTUK_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property PAKEJ_BT As System.String
                Get
                    Return _PAKEJ_BT
                End Get
                Set(ByVal Value As System.String)
                    _PAKEJ_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property PAKEJ_LAIN As System.String
                Get
                    Return _PAKEJ_LAIN
                End Get
                Set(ByVal Value As System.String)
                    _PAKEJ_LAIN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KUANTITI_PAKEJ As System.String
                Get
                    Return _KUANTITI_PAKEJ
                End Get
                Set(ByVal Value As System.String)
                    _KUANTITI_PAKEJ = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KUANTITI_BT_METRIK As System.Single
                Get
                    Return _KUANTITI_BT_METRIK
                End Get
                Set(ByVal Value As System.Single)
                    _KUANTITI_BT_METRIK = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KUANTITI_BT_M3 As System.Single
                Get
                    Return _KUANTITI_BT_M3
                End Get
                Set(ByVal Value As System.Single)
                    _KUANTITI_BT_M3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property UNIT As System.String
                Get
                    Return _UNIT
                End Get
                Set(ByVal Value As System.String)
                    _UNIT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KOS_PERAWATAN As System.String
                Get
                    Return _KOS_PERAWATAN
                End Get
                Set(ByVal Value As System.String)
                    _KOS_PERAWATAN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ALAMAT As System.String
                Get
                    Return _ALAMAT
                End Get
                Set(ByVal Value As System.String)
                    _ALAMAT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property BANDAR As System.String
                Get
                    Return _BANDAR
                End Get
                Set(ByVal Value As System.String)
                    _BANDAR = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property POSKOD As System.String
                Get
                    Return _POSKOD
                End Get
                Set(ByVal Value As System.String)
                    _POSKOD = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property PEGAWAI As System.String
                Get
                    Return _PEGAWAI
                End Get
                Set(ByVal Value As System.String)
                    _PEGAWAI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property JAWATAN As System.String
                Get
                    Return _JAWATAN
                End Get
                Set(ByVal Value As System.String)
                    _JAWATAN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TELEFON As System.String
                Get
                    Return _TELEFON
                End Get
                Set(ByVal Value As System.String)
                    _TELEFON = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property FAKS As System.String
                Get
                    Return _FAKS
                End Get
                Set(ByVal Value As System.String)
                    _FAKS = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property EMAIL As System.String
                Get
                    Return _EMAIL
                End Get
                Set(ByVal Value As System.String)
                    _EMAIL = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property NAMA_PENGELUAR As System.String
                Get
                    Return _NAMA_PENGELUAR
                End Get
                Set(ByVal Value As System.String)
                    _NAMA_PENGELUAR = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TARIKH_HANTAR As System.Object
                Get
                    Return _TARIKH_HANTAR
                End Get
                Set(ByVal Value As System.Object)
                    _TARIKH_HANTAR = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property MASA_HANTAR As System.Object
                Get
                    Return _MASA_HANTAR
                End Get
                Set(ByVal Value As System.Object)
                    _MASA_HANTAR = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property PEGAWAI_HANTAR As System.String
                Get
                    Return _PEGAWAI_HANTAR
                End Get
                Set(ByVal Value As System.String)
                    _PEGAWAI_HANTAR = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property R_ID_DESTINASI As System.String
                Get
                    Return _R_ID_DESTINASI
                End Get
                Set(ByVal Value As System.String)
                    _R_ID_DESTINASI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property R_NAMA_DESTINASI As System.String
                Get
                    Return _R_NAMA_DESTINASI
                End Get
                Set(ByVal Value As System.String)
                    _R_NAMA_DESTINASI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property R_ALAMAT As System.String
                Get
                    Return _R_ALAMAT
                End Get
                Set(ByVal Value As System.String)
                    _R_ALAMAT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property R_DAERAH As System.String
                Get
                    Return _R_DAERAH
                End Get
                Set(ByVal Value As System.String)
                    _R_DAERAH = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property R_NEGERI As System.String
                Get
                    Return _R_NEGERI
                End Get
                Set(ByVal Value As System.String)
                    _R_NEGERI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property MOHON_BATAL As System.Int16
                Get
                    Return _MOHON_BATAL
                End Get
                Set(ByVal Value As System.Int16)
                    _MOHON_BATAL = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TARIKH_MOHON As System.Object
                Get
                    Return _TARIKH_MOHON
                End Get
                Set(ByVal Value As System.Object)
                    _TARIKH_MOHON = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_ID_TRANSPORTER As System.String
                Get
                    Return _T1_ID_TRANSPORTER
                End Get
                Set(ByVal Value As System.String)
                    _T1_ID_TRANSPORTER = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_NAME_TRANSPORTER As System.String
                Get
                    Return _T1_NAME_TRANSPORTER
                End Get
                Set(ByVal Value As System.String)
                    _T1_NAME_TRANSPORTER = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_NAME_RESPONSIBLE As System.String
                Get
                    Return _T1_NAME_RESPONSIBLE
                End Get
                Set(ByVal Value As System.String)
                    _T1_NAME_RESPONSIBLE = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_TELEPHONE As System.String
                Get
                    Return _T1_TELEPHONE
                End Get
                Set(ByVal Value As System.String)
                    _T1_TELEPHONE = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_FAXS As System.String
                Get
                    Return _T1_FAXS
                End Get
                Set(ByVal Value As System.String)
                    _T1_FAXS = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_EMAIL As System.String
                Get
                    Return _T1_EMAIL
                End Get
                Set(ByVal Value As System.String)
                    _T1_EMAIL = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_VEHICLE_NO As System.String
                Get
                    Return _T1_VEHICLE_NO
                End Get
                Set(ByVal Value As System.String)
                    _T1_VEHICLE_NO = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_DRIVER_NAME As System.String
                Get
                    Return _T1_DRIVER_NAME
                End Get
                Set(ByVal Value As System.String)
                    _T1_DRIVER_NAME = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_NRIC As System.String
                Get
                    Return _T1_NRIC
                End Get
                Set(ByVal Value As System.String)
                    _T1_NRIC = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_PENSTORAN As System.String
                Get
                    Return _T1_PENSTORAN
                End Get
                Set(ByVal Value As System.String)
                    _T1_PENSTORAN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_TARIKH_TERIMA As System.Object
                Get
                    Return _T1_TARIKH_TERIMA
                End Get
                Set(ByVal Value As System.Object)
                    _T1_TARIKH_TERIMA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_MASA_TERIMA As System.Object
                Get
                    Return _T1_MASA_TERIMA
                End Get
                Set(ByVal Value As System.Object)
                    _T1_MASA_TERIMA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_PEGAWAI As System.String
                Get
                    Return _T1_PEGAWAI
                End Get
                Set(ByVal Value As System.String)
                    _T1_PEGAWAI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_NORUJUKAN As System.String
                Get
                    Return _T1_NORUJUKAN
                End Get
                Set(ByVal Value As System.String)
                    _T1_NORUJUKAN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_NAMAPREMIS As System.String
                Get
                    Return _T1_NAMAPREMIS
                End Get
                Set(ByVal Value As System.String)
                    _T1_NAMAPREMIS = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_VESSEL_NAME As System.String
                Get
                    Return _T1_VESSEL_NAME
                End Get
                Set(ByVal Value As System.String)
                    _T1_VESSEL_NAME = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_VESSEL_PERSON As System.String
                Get
                    Return _T1_VESSEL_PERSON
                End Get
                Set(ByVal Value As System.String)
                    _T1_VESSEL_PERSON = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_VESSEL_PHONE As System.String
                Get
                    Return _T1_VESSEL_PHONE
                End Get
                Set(ByVal Value As System.String)
                    _T1_VESSEL_PHONE = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_VESSEL_FAX As System.String
                Get
                    Return _T1_VESSEL_FAX
                End Get
                Set(ByVal Value As System.String)
                    _T1_VESSEL_FAX = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_VESEL_EMAIL As System.String
                Get
                    Return _T1_VESEL_EMAIL
                End Get
                Set(ByVal Value As System.String)
                    _T1_VESEL_EMAIL = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_PENSTORAN1 As System.String
                Get
                    Return _T1_PENSTORAN1
                End Get
                Set(ByVal Value As System.String)
                    _T1_PENSTORAN1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_TARIKH_TERIMA1 As System.Object
                Get
                    Return _T1_TARIKH_TERIMA1
                End Get
                Set(ByVal Value As System.Object)
                    _T1_TARIKH_TERIMA1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_MASA_TERIMA1 As System.Object
                Get
                    Return _T1_MASA_TERIMA1
                End Get
                Set(ByVal Value As System.Object)
                    _T1_MASA_TERIMA1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T1_PEGAWAI1 As System.String
                Get
                    Return _T1_PEGAWAI1
                End Get
                Set(ByVal Value As System.String)
                    _T1_PEGAWAI1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_ID_PENGANGKUT As System.String
                Get
                    Return _T_ID_PENGANGKUT
                End Get
                Set(ByVal Value As System.String)
                    _T_ID_PENGANGKUT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_NAMA_PENGANGKUT As System.String
                Get
                    Return _T_NAMA_PENGANGKUT
                End Get
                Set(ByVal Value As System.String)
                    _T_NAMA_PENGANGKUT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_PEGAWAI_PENGANGKUT As System.String
                Get
                    Return _T_PEGAWAI_PENGANGKUT
                End Get
                Set(ByVal Value As System.String)
                    _T_PEGAWAI_PENGANGKUT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_JAWATAN_PENGANGKUT As System.String
                Get
                    Return _T_JAWATAN_PENGANGKUT
                End Get
                Set(ByVal Value As System.String)
                    _T_JAWATAN_PENGANGKUT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_TELEFON_PENGANGKUT As System.String
                Get
                    Return _T_TELEFON_PENGANGKUT
                End Get
                Set(ByVal Value As System.String)
                    _T_TELEFON_PENGANGKUT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_FAKS_PENGANGKUT As System.String
                Get
                    Return _T_FAKS_PENGANGKUT
                End Get
                Set(ByVal Value As System.String)
                    _T_FAKS_PENGANGKUT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_EMAIL As System.String
                Get
                    Return _T_EMAIL
                End Get
                Set(ByVal Value As System.String)
                    _T_EMAIL = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_KENDERAAN As System.String
                Get
                    Return _T_KENDERAAN
                End Get
                Set(ByVal Value As System.String)
                    _T_KENDERAAN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_PEMANDU As System.String
                Get
                    Return _T_PEMANDU
                End Get
                Set(ByVal Value As System.String)
                    _T_PEMANDU = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_NOKP As System.String
                Get
                    Return _T_NOKP
                End Get
                Set(ByVal Value As System.String)
                    _T_NOKP = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_STORAN_SEMENTARA As System.String
                Get
                    Return _T_STORAN_SEMENTARA
                End Get
                Set(ByVal Value As System.String)
                    _T_STORAN_SEMENTARA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_TARIKH As System.Object
                Get
                    Return _T_TARIKH
                End Get
                Set(ByVal Value As System.Object)
                    _T_TARIKH = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_MASA As System.Object
                Get
                    Return _T_MASA
                End Get
                Set(ByVal Value As System.Object)
                    _T_MASA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property T_PEGAWAI As System.String
                Get
                    Return _T_PEGAWAI
                End Get
                Set(ByVal Value As System.String)
                    _T_PEGAWAI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property R_PEGAWAI_PENERIMA As System.String
                Get
                    Return _R_PEGAWAI_PENERIMA
                End Get
                Set(ByVal Value As System.String)
                    _R_PEGAWAI_PENERIMA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property R_TELEFON_PENERIMA As System.String
                Get
                    Return _R_TELEFON_PENERIMA
                End Get
                Set(ByVal Value As System.String)
                    _R_TELEFON_PENERIMA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property R_FAKS_PENERIMA As System.String
                Get
                    Return _R_FAKS_PENERIMA
                End Get
                Set(ByVal Value As System.String)
                    _R_FAKS_PENERIMA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property R_EMAIL As System.String
                Get
                    Return _R_EMAIL
                End Get
                Set(ByVal Value As System.String)
                    _R_EMAIL = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property RP_JENIS_OPERASI As System.String
                Get
                    Return _RP_JENIS_OPERASI
                End Get
                Set(ByVal Value As System.String)
                    _RP_JENIS_OPERASI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property RP_OPERASI_LAIN As System.String
                Get
                    Return _RP_OPERASI_LAIN
                End Get
                Set(ByVal Value As System.String)
                    _RP_OPERASI_LAIN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property RP_KUANTITI_TERIMA_METRIK As System.Single
                Get
                    Return _RP_KUANTITI_TERIMA_METRIK
                End Get
                Set(ByVal Value As System.Single)
                    _RP_KUANTITI_TERIMA_METRIK = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property RP_KUANTITI_TERIMA_M3 As System.Single
                Get
                    Return _RP_KUANTITI_TERIMA_M3
                End Get
                Set(ByVal Value As System.Single)
                    _RP_KUANTITI_TERIMA_M3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property RP_TARIKH_TERIMA As System.Object
                Get
                    Return _RP_TARIKH_TERIMA
                End Get
                Set(ByVal Value As System.Object)
                    _RP_TARIKH_TERIMA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property RP_TARIKH_TOLAK As System.Object
                Get
                    Return _RP_TARIKH_TOLAK
                End Get
                Set(ByVal Value As System.Object)
                    _RP_TARIKH_TOLAK = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property RP_CATATAN_PENERIMA As System.String
                Get
                    Return _RP_CATATAN_PENERIMA
                End Get
                Set(ByVal Value As System.String)
                    _RP_CATATAN_PENERIMA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property STATUS As System.String
                Get
                    Return _STATUS
                End Get
                Set(ByVal Value As System.String)
                    _STATUS = Value
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
            Public Property KOD_NK As System.String
                Get
                    Return _KOD_NK
                End Get
                Set(ByVal Value As System.String)
                    _KOD_NK = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property STAT As System.String
                Get
                    Return _STAT
                End Get
                Set(ByVal Value As System.String)
                    _STAT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property REMAINDER As System.Int32
                Get
                    Return _REMAINDER
                End Get
                Set(ByVal Value As System.Int32)
                    _REMAINDER = Value
                End Set
            End Property

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

        End Class
#End Region

    End Namespace

#Region "Class Info"
    Public Class Nota_konsainanInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "(select top 1 companyname from bizentity where bizregid=nota_konsainan.IDPREMIS) as CompanyName,(select top 1 companyname from bizentity where bizregid=nota_konsainan.R_ID_DESTINASI) as ReceiverName,ID,NEGERI,DAERAH,JENIS_TRANS,IDPREMIS,NOFAIL_JAS,SIC,ID_PENGELUAR_BT,NOSIRI_BT,KOD_BT,NAMA_BT,KOMPONEN_BT,ASAL_BT,KOD_ASAL_BT,BENTUK_BT,PAKEJ_BT,PAKEJ_LAIN,KUANTITI_PAKEJ,KUANTITI_BT_METRIK,KUANTITI_BT_M3,UNIT,KOS_PERAWATAN,ALAMAT,BANDAR,POSKOD,PEGAWAI,JAWATAN,TELEFON,FAKS,EMAIL,NAMA_PENGELUAR,TARIKH_HANTAR,MASA_HANTAR,PEGAWAI_HANTAR,R_ID_DESTINASI,R_NAMA_DESTINASI,R_ALAMAT,R_DAERAH,R_NEGERI,MOHON_BATAL,TARIKH_MOHON,T1_ID_TRANSPORTER,T1_NAME_TRANSPORTER,T1_NAME_RESPONSIBLE,T1_TELEPHONE,T1_FAXS,T1_EMAIL,T1_VEHICLE_NO,T1_DRIVER_NAME,T1_NRIC,T1_PENSTORAN,T1_TARIKH_TERIMA,T1_MASA_TERIMA,T1_PEGAWAI,T1_NORUJUKAN,T1_NAMAPREMIS,T1_VESSEL_NAME,T1_VESSEL_PERSON,T1_VESSEL_PHONE,T1_VESSEL_FAX,T1_VESEL_EMAIL,T1_PENSTORAN1,T1_TARIKH_TERIMA1,T1_MASA_TERIMA1,T1_PEGAWAI1,T_ID_PENGANGKUT,T_NAMA_PENGANGKUT,T_PEGAWAI_PENGANGKUT,T_JAWATAN_PENGANGKUT,T_TELEFON_PENGANGKUT,T_FAKS_PENGANGKUT,T_EMAIL,T_KENDERAAN,T_PEMANDU,T_NOKP,T_STORAN_SEMENTARA,T_TARIKH,T_MASA,T_PEGAWAI,R_PEGAWAI_PENERIMA,R_TELEFON_PENERIMA,R_FAKS_PENERIMA,R_EMAIL,RP_JENIS_OPERASI,RP_OPERASI_LAIN,RP_KUANTITI_TERIMA_METRIK,RP_KUANTITI_TERIMA_M3,RP_TARIKH_TERIMA,RP_TARIKH_TOLAK,RP_CATATAN_PENERIMA,STATUS,IDLOGIN,TARIKH_WUJUD,WUJUD_PENGGUNA,TARIKH_KEMASKINI,KEMASKINI_PENGGUNA,KOD_NK,STAT,REMAINDER"
                .CheckFields = ""
                .TableName = "Nota_konsainan WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "(select top 1 companyname from bizentity where bizregid=nota_konsainan.IDPREMIS) as CompanyName,(select top 1 companyname from bizentity where bizregid=nota_konsainan.R_ID_DESTINASI) as ReceiverName,ID,NEGERI,DAERAH,JENIS_TRANS,IDPREMIS,NOFAIL_JAS,SIC,ID_PENGELUAR_BT,NOSIRI_BT,KOD_BT,NAMA_BT,KOMPONEN_BT,ASAL_BT,KOD_ASAL_BT,BENTUK_BT,PAKEJ_BT,PAKEJ_LAIN,KUANTITI_PAKEJ,KUANTITI_BT_METRIK,KUANTITI_BT_M3,UNIT,KOS_PERAWATAN,ALAMAT,BANDAR,POSKOD,PEGAWAI,JAWATAN,TELEFON,FAKS,EMAIL,NAMA_PENGELUAR,TARIKH_HANTAR,MASA_HANTAR,PEGAWAI_HANTAR,R_ID_DESTINASI,R_NAMA_DESTINASI,R_ALAMAT,R_DAERAH,R_NEGERI,MOHON_BATAL,TARIKH_MOHON,T1_ID_TRANSPORTER,T1_NAME_TRANSPORTER,T1_NAME_RESPONSIBLE,T1_TELEPHONE,T1_FAXS,T1_EMAIL,T1_VEHICLE_NO,T1_DRIVER_NAME,T1_NRIC,T1_PENSTORAN,T1_TARIKH_TERIMA,T1_MASA_TERIMA,T1_PEGAWAI,T1_NORUJUKAN,T1_NAMAPREMIS,T1_VESSEL_NAME,T1_VESSEL_PERSON,T1_VESSEL_PHONE,T1_VESSEL_FAX,T1_VESEL_EMAIL,T1_PENSTORAN1,T1_TARIKH_TERIMA1,T1_MASA_TERIMA1,T1_PEGAWAI1,T_ID_PENGANGKUT,T_NAMA_PENGANGKUT,T_PEGAWAI_PENGANGKUT,T_JAWATAN_PENGANGKUT,T_TELEFON_PENGANGKUT,T_FAKS_PENGANGKUT,T_EMAIL,T_KENDERAAN,T_PEMANDU,T_NOKP,T_STORAN_SEMENTARA,T_TARIKH,T_MASA,T_PEGAWAI,R_PEGAWAI_PENERIMA,R_TELEFON_PENERIMA,R_FAKS_PENERIMA,R_EMAIL,RP_JENIS_OPERASI,RP_OPERASI_LAIN,RP_KUANTITI_TERIMA_METRIK,RP_KUANTITI_TERIMA_M3,RP_TARIKH_TERIMA,RP_TARIKH_TOLAK,RP_CATATAN_PENERIMA,STATUS,IDLOGIN,TARIKH_WUJUD,WUJUD_PENGGUNA,TARIKH_KEMASKINI,KEMASKINI_PENGGUNA,KOD_NK,STAT,REMAINDER"
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
    Public Class Nota_KonsainanScheme
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
                .FieldName = "NEGERI"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DAERAH"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "JENIS_TRANS"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "IDPREMIS"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "NOFAIL_JAS"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "SIC"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ID_PENGELUAR_BT"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "NOSIRI_BT"
                .Length = 25
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "KOD_BT"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "NAMA_BT"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "KOMPONEN_BT"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ASAL_BT"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "KOD_ASAL_BT"
                .Length = 15
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BENTUK_BT"
                .Length = 30
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PAKEJ_BT"
                .Length = 30
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PAKEJ_LAIN"
                .Length = 30
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "KUANTITI_PAKEJ"
                .Length = 30
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "KUANTITI_BT_METRIK"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "KUANTITI_BT_M3"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UNIT"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "KOS_PERAWATAN"
                .Length = 15
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ALAMAT"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BANDAR"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "POSKOD"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "PEGAWAI"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "JAWATAN"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TELEFON"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "FAKS"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "EMAIL"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "NAMA_PENGELUAR"
                .Length = 100
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "TARIKH_HANTAR"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "MASA_HANTAR"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "PEGAWAI_HANTAR"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "R_ID_DESTINASI"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "R_NAMA_DESTINASI"
                .Length = 100
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "R_ALAMAT"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "R_DAERAH"
                .Length = 30
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(37, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "R_NEGERI"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(38, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "MOHON_BATAL"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(39, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "TARIKH_MOHON"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(40, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T1_ID_TRANSPORTER"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(41, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T1_NAME_TRANSPORTER"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(42, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T1_NAME_RESPONSIBLE"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(43, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T1_TELEPHONE"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(44, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T1_FAXS"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(45, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T1_EMAIL"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(46, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T1_VEHICLE_NO"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(47, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T1_DRIVER_NAME"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(48, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T1_NRIC"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(49, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T1_PENSTORAN"
                .Length = -1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(50, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "T1_TARIKH_TERIMA"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(51, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "T1_MASA_TERIMA"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(52, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T1_PEGAWAI"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(53, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T1_NORUJUKAN"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(54, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T1_NAMAPREMIS"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(55, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T1_VESSEL_NAME"
                .Length = 100
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(56, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T1_VESSEL_PERSON"
                .Length = 100
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(57, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T1_VESSEL_PHONE"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(58, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T1_VESSEL_FAX"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(59, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T1_VESEL_EMAIL"
                .Length = 150
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(60, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T1_PENSTORAN1"
                .Length = -1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(61, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "T1_TARIKH_TERIMA1"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(62, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "T1_MASA_TERIMA1"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(63, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T1_PEGAWAI1"
                .Length = -1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(64, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T_ID_PENGANGKUT"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(65, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T_NAMA_PENGANGKUT"
                .Length = 100
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(66, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T_PEGAWAI_PENGANGKUT"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(67, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T_JAWATAN_PENGANGKUT"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(68, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T_TELEFON_PENGANGKUT"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(69, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T_FAKS_PENGANGKUT"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(70, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T_EMAIL"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(71, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T_KENDERAAN"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(72, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T_PEMANDU"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(73, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "T_NOKP"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(74, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T_STORAN_SEMENTARA"
                .Length = 250
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(75, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "T_TARIKH"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(76, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "T_MASA"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(77, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "T_PEGAWAI"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(78, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "R_PEGAWAI_PENERIMA"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(79, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "R_TELEFON_PENERIMA"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(80, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "R_FAKS_PENERIMA"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(81, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "R_EMAIL"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(82, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RP_JENIS_OPERASI"
                .Length = 30
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(83, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RP_OPERASI_LAIN"
                .Length = 30
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(84, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "RP_KUANTITI_TERIMA_METRIK"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(85, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "RP_KUANTITI_TERIMA_M3"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(86, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "RP_TARIKH_TERIMA"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(87, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "RP_TARIKH_TOLAK"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(88, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "RP_CATATAN_PENERIMA"
                .Length = 100
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(89, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "STATUS"
                .Length = 15
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(90, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IDLOGIN"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(91, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "TARIKH_WUJUD"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(92, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WUJUD_PENGGUNA"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(93, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "TARIKH_KEMASKINI"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(94, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "KEMASKINI_PENGGUNA"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(95, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "KOD_NK"
                .Length = 30
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(96, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "STAT"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(97, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "REMAINDER"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(98, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TRAN"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(99, this)

        End Sub

        Public ReadOnly Property ID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property NEGERI As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property DAERAH As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property JENIS_TRANS As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property IDPREMIS As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property NOFAIL_JAS As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property SIC As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property ID_PENGELUAR_BT As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property NOSIRI_BT As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property KOD_BT As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property NAMA_BT As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property KOMPONEN_BT As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property ASAL_BT As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property KOD_ASAL_BT As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property BENTUK_BT As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property PAKEJ_BT As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property PAKEJ_LAIN As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property KUANTITI_PAKEJ As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property KUANTITI_BT_METRIK As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property KUANTITI_BT_M3 As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property UNIT As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property KOS_PERAWATAN As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property ALAMAT As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property BANDAR As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property POSKOD As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property PEGAWAI As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property JAWATAN As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property TELEFON As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property FAKS As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property EMAIL As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property NAMA_PENGELUAR As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property TARIKH_HANTAR As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property MASA_HANTAR As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property PEGAWAI_HANTAR As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property
        Public ReadOnly Property R_ID_DESTINASI As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property R_NAMA_DESTINASI As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property R_ALAMAT As StrucElement
            Get
                Return MyBase.GetItem(36)
            End Get
        End Property
        Public ReadOnly Property R_DAERAH As StrucElement
            Get
                Return MyBase.GetItem(37)
            End Get
        End Property
        Public ReadOnly Property R_NEGERI As StrucElement
            Get
                Return MyBase.GetItem(38)
            End Get
        End Property
        Public ReadOnly Property MOHON_BATAL As StrucElement
            Get
                Return MyBase.GetItem(39)
            End Get
        End Property
        Public ReadOnly Property TARIKH_MOHON As StrucElement
            Get
                Return MyBase.GetItem(40)
            End Get
        End Property
        Public ReadOnly Property T1_ID_TRANSPORTER As StrucElement
            Get
                Return MyBase.GetItem(41)
            End Get
        End Property
        Public ReadOnly Property T1_NAME_TRANSPORTER As StrucElement
            Get
                Return MyBase.GetItem(42)
            End Get
        End Property
        Public ReadOnly Property T1_NAME_RESPONSIBLE As StrucElement
            Get
                Return MyBase.GetItem(43)
            End Get
        End Property
        Public ReadOnly Property T1_TELEPHONE As StrucElement
            Get
                Return MyBase.GetItem(44)
            End Get
        End Property
        Public ReadOnly Property T1_FAXS As StrucElement
            Get
                Return MyBase.GetItem(45)
            End Get
        End Property
        Public ReadOnly Property T1_EMAIL As StrucElement
            Get
                Return MyBase.GetItem(46)
            End Get
        End Property
        Public ReadOnly Property T1_VEHICLE_NO As StrucElement
            Get
                Return MyBase.GetItem(47)
            End Get
        End Property
        Public ReadOnly Property T1_DRIVER_NAME As StrucElement
            Get
                Return MyBase.GetItem(48)
            End Get
        End Property
        Public ReadOnly Property T1_NRIC As StrucElement
            Get
                Return MyBase.GetItem(49)
            End Get
        End Property
        Public ReadOnly Property T1_PENSTORAN As StrucElement
            Get
                Return MyBase.GetItem(50)
            End Get
        End Property
        Public ReadOnly Property T1_TARIKH_TERIMA As StrucElement
            Get
                Return MyBase.GetItem(51)
            End Get
        End Property
        Public ReadOnly Property T1_MASA_TERIMA As StrucElement
            Get
                Return MyBase.GetItem(52)
            End Get
        End Property
        Public ReadOnly Property T1_PEGAWAI As StrucElement
            Get
                Return MyBase.GetItem(53)
            End Get
        End Property
        Public ReadOnly Property T1_NORUJUKAN As StrucElement
            Get
                Return MyBase.GetItem(54)
            End Get
        End Property
        Public ReadOnly Property T1_NAMAPREMIS As StrucElement
            Get
                Return MyBase.GetItem(55)
            End Get
        End Property
        Public ReadOnly Property T1_VESSEL_NAME As StrucElement
            Get
                Return MyBase.GetItem(56)
            End Get
        End Property
        Public ReadOnly Property T1_VESSEL_PERSON As StrucElement
            Get
                Return MyBase.GetItem(57)
            End Get
        End Property
        Public ReadOnly Property T1_VESSEL_PHONE As StrucElement
            Get
                Return MyBase.GetItem(58)
            End Get
        End Property
        Public ReadOnly Property T1_VESSEL_FAX As StrucElement
            Get
                Return MyBase.GetItem(59)
            End Get
        End Property
        Public ReadOnly Property T1_VESEL_EMAIL As StrucElement
            Get
                Return MyBase.GetItem(60)
            End Get
        End Property
        Public ReadOnly Property T1_PENSTORAN1 As StrucElement
            Get
                Return MyBase.GetItem(61)
            End Get
        End Property
        Public ReadOnly Property T1_TARIKH_TERIMA1 As StrucElement
            Get
                Return MyBase.GetItem(62)
            End Get
        End Property
        Public ReadOnly Property T1_MASA_TERIMA1 As StrucElement
            Get
                Return MyBase.GetItem(63)
            End Get
        End Property
        Public ReadOnly Property T1_PEGAWAI1 As StrucElement
            Get
                Return MyBase.GetItem(64)
            End Get
        End Property
        Public ReadOnly Property T_ID_PENGANGKUT As StrucElement
            Get
                Return MyBase.GetItem(65)
            End Get
        End Property
        Public ReadOnly Property T_NAMA_PENGANGKUT As StrucElement
            Get
                Return MyBase.GetItem(66)
            End Get
        End Property
        Public ReadOnly Property T_PEGAWAI_PENGANGKUT As StrucElement
            Get
                Return MyBase.GetItem(67)
            End Get
        End Property
        Public ReadOnly Property T_JAWATAN_PENGANGKUT As StrucElement
            Get
                Return MyBase.GetItem(68)
            End Get
        End Property
        Public ReadOnly Property T_TELEFON_PENGANGKUT As StrucElement
            Get
                Return MyBase.GetItem(69)
            End Get
        End Property
        Public ReadOnly Property T_FAKS_PENGANGKUT As StrucElement
            Get
                Return MyBase.GetItem(70)
            End Get
        End Property
        Public ReadOnly Property T_EMAIL As StrucElement
            Get
                Return MyBase.GetItem(71)
            End Get
        End Property
        Public ReadOnly Property T_KENDERAAN As StrucElement
            Get
                Return MyBase.GetItem(72)
            End Get
        End Property
        Public ReadOnly Property T_PEMANDU As StrucElement
            Get
                Return MyBase.GetItem(73)
            End Get
        End Property
        Public ReadOnly Property T_NOKP As StrucElement
            Get
                Return MyBase.GetItem(74)
            End Get
        End Property
        Public ReadOnly Property T_STORAN_SEMENTARA As StrucElement
            Get
                Return MyBase.GetItem(75)
            End Get
        End Property
        Public ReadOnly Property T_TARIKH As StrucElement
            Get
                Return MyBase.GetItem(76)
            End Get
        End Property
        Public ReadOnly Property T_MASA As StrucElement
            Get
                Return MyBase.GetItem(77)
            End Get
        End Property
        Public ReadOnly Property T_PEGAWAI As StrucElement
            Get
                Return MyBase.GetItem(78)
            End Get
        End Property
        Public ReadOnly Property R_PEGAWAI_PENERIMA As StrucElement
            Get
                Return MyBase.GetItem(79)
            End Get
        End Property
        Public ReadOnly Property R_TELEFON_PENERIMA As StrucElement
            Get
                Return MyBase.GetItem(80)
            End Get
        End Property
        Public ReadOnly Property R_FAKS_PENERIMA As StrucElement
            Get
                Return MyBase.GetItem(81)
            End Get
        End Property
        Public ReadOnly Property R_EMAIL As StrucElement
            Get
                Return MyBase.GetItem(82)
            End Get
        End Property
        Public ReadOnly Property RP_JENIS_OPERASI As StrucElement
            Get
                Return MyBase.GetItem(83)
            End Get
        End Property
        Public ReadOnly Property RP_OPERASI_LAIN As StrucElement
            Get
                Return MyBase.GetItem(84)
            End Get
        End Property
        Public ReadOnly Property RP_KUANTITI_TERIMA_METRIK As StrucElement
            Get
                Return MyBase.GetItem(85)
            End Get
        End Property
        Public ReadOnly Property RP_KUANTITI_TERIMA_M3 As StrucElement
            Get
                Return MyBase.GetItem(86)
            End Get
        End Property
        Public ReadOnly Property RP_TARIKH_TERIMA As StrucElement
            Get
                Return MyBase.GetItem(87)
            End Get
        End Property
        Public ReadOnly Property RP_TARIKH_TOLAK As StrucElement
            Get
                Return MyBase.GetItem(88)
            End Get
        End Property
        Public ReadOnly Property RP_CATATAN_PENERIMA As StrucElement
            Get
                Return MyBase.GetItem(89)
            End Get
        End Property
        Public ReadOnly Property STATUS As StrucElement
            Get
                Return MyBase.GetItem(90)
            End Get
        End Property
        Public ReadOnly Property IDLOGIN As StrucElement
            Get
                Return MyBase.GetItem(91)
            End Get
        End Property
        Public ReadOnly Property TARIKH_WUJUD As StrucElement
            Get
                Return MyBase.GetItem(92)
            End Get
        End Property
        Public ReadOnly Property WUJUD_PENGGUNA As StrucElement
            Get
                Return MyBase.GetItem(93)
            End Get
        End Property
        Public ReadOnly Property TARIKH_KEMASKINI As StrucElement
            Get
                Return MyBase.GetItem(94)
            End Get
        End Property
        Public ReadOnly Property KEMASKINI_PENGGUNA As StrucElement
            Get
                Return MyBase.GetItem(95)
            End Get
        End Property
        Public ReadOnly Property KOD_NK As StrucElement
            Get
                Return MyBase.GetItem(96)
            End Get
        End Property
        Public ReadOnly Property STAT As StrucElement
            Get
                Return MyBase.GetItem(97)
            End Get
        End Property
        Public ReadOnly Property REMAINDER As StrucElement
            Get
                Return MyBase.GetItem(98)
            End Get
        End Property
        Public ReadOnly Property TRAN As StrucElement
            Get
                Return MyBase.GetItem(99)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace
