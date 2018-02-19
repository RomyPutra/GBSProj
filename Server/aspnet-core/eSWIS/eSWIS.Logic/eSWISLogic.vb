
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports System.IO 'For log

Imports System.Web

Imports System.Data.OleDb

Imports System.Configuration
Imports Microsoft.VisualBasic

Imports System.Threading

Imports System.Linq
Imports System.Text.RegularExpressions

Imports eSWIS.Logic
Imports eSWIS.Shared.General


Namespace eSWISLogics
    Public NotInheritable Class eSWISLogic
        Inherits Core.CoreControl
        ''
        Private Log As New SystemLog()


        'Private ConsignhdrInfo As Actions.ConsignhdrInfo = New Actions.ConsignhdrInfo

        'Notification
        Private NotifyhdrInfo As Actions.NotifyhdrInfo = New Actions.NotifyhdrInfo
        Private NotifydtlInfo As Actions.NotifydtlInfo = New Actions.NotifydtlInfo
        Private ItemLocInfo As Actions.ItemlocInfo = New Actions.ItemlocInfo

        'Inventory
        Private ItmtranshdrInfo As Actions.ItmtranshdrInfo = New Actions.ItmtranshdrInfo
        Private ItmtransdtlInfo As Actions.ItmtransdtlInfo = New Actions.ItmtransdtlInfo

        Private contItemLock As Actions.Container.Itemloc = New Actions.Container.Itemloc
        Private schemeItemLock As Actions.ItemLocScheme = New Actions.ItemLocScheme
        Private objItemLock As Actions.ItemLoc

        'added by Antoni Bernad 10272014 - BatchExecute Registration
        'Company
        Private CompanyInfo As Profiles.CompanyInfo = New Profiles.CompanyInfo
        Private BizlocateInfo As Profiles.BizlocateInfo = New Profiles.BizlocateInfo
        Private UserProfileInfo As UserSecurity.UserProfileInfo = New UserSecurity.UserProfileInfo
        Private EmpBranchInfo As Profiles.EmpbranchInfo = New Profiles.EmpbranchInfo
        Private EmployeeInfo As Profiles.EmployeeInfo = New Profiles.EmployeeInfo

        'end added

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Selection"


        'amended by diana 20150120, check by Waste Code and Name, ongoing, not yet check flag and transdate of draft to submit
        Public Function IsLatestNotification(ByVal TransNo As String, ByVal LocID As String, ByVal ItemCode As String, ByVal ItemName As String) As Boolean
            Dim dtTemp As DataTable
            StartSQLControl()
            Try
                strSQL = "SELECT TOP 1 TransNo FROM NOTIFYDTL WITH (NOLOCK) WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' " & _
                            "AND ItemCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCode) & "' AND ItmName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemName) & "' AND Posted=1 " & _
                            "ORDER BY CreateDate desc"
                'strSQL = "SELECT TOP 1 TransNo " & _
                '         " FROM NOTIFYHDR " & _
                '         " WHERE Flag=1 and LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "'" & _
                '         " ORDER BY TransDate desc"

                dtTemp = objConn.Execute(strSQL, CommandType.Text)

                If dtTemp Is Nothing = False Then
                    If dtTemp.Rows.Count > 0 Then
                        If TransNo = dtTemp.Rows(0).Item("TransNo") Then
                            Return True
                        Else
                            Return False
                        End If

                    Else
                        Return False
                    End If
                Else
                    Return False
                End If

            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic", ex.Message & strSQL, ex.StackTrace)
                'Throw New Exception(ex.Message)
            End Try
        End Function

        Public Function IsUsed(ByVal ItemCode As String, ByVal LocID As String) As Boolean
            Dim dtTemp As DataTable
            Try
                StartSQLControl()
                strSQL = "select distinct TransNo " & _
                         " from NOTIFYDTL WITH (NOLOCK) " & _
                         " where LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' and ItemCode='" & ItemCode & "'"

                dtTemp = objConn.Execute(strSQL, CommandType.Text)

                If dtTemp Is Nothing = False Then
                    If dtTemp.Rows.Count > 0 Then
                        Return True
                    Else
                        Return False
                    End If

                Else
                    Return False
                End If


            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic", ex.Message & strSQL, ex.StackTrace)
                'Throw New Exception(ex.Message)
            End Try
        End Function


        Public Function IsExistEmail(ByVal TransNo As String, ByVal Email As String) As Boolean
            Dim dtTemp As DataTable
            Try
                StartSQLControl()
                strSQL = "SELECT EmailAddress FROM Email WITH (NOLOCK) WHERE EmailAddress = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Email) & "' and EmailType=6 and EmailBody like '%" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, TransNo) & "%'"

                dtTemp = objConn.Execute(strSQL, CommandType.Text)

                If dtTemp Is Nothing = False Then
                    If dtTemp.Rows.Count > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
                EndSQLControl()

            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic", ex.Message & strSQL, ex.StackTrace)
                'Throw New Exception(ex.Message)
            End Try
        End Function

        Public Function ParseValue(ByVal value As String) As String
            Dim result As String
            Try
                StartSQLControl()

                result = objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, value)
                Return result

                EndSQLControl()

            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic", ex.Message & strSQL, ex.StackTrace)
                'Throw New Exception(ex.Message)
            End Try
        End Function
#End Region

#Region "Data Manipulation-Add,Edit,Del"
        Public Function Delete(ByVal ListNotifydtlCont As List(Of Actions.Container.Notifydtl), ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            Delete = False
            Try
                If ListNotifydtlCont Is Nothing Then
                    'Error Message
                Else
                    StartSQLControl()

                    If ListNotifydtlCont IsNot Nothing Then
                        StartSQLControl()
                        For Each NotifyDtlCont In ListNotifydtlCont
                            With ItemLocInfo.MyInfo
                                .CheckFields = "RecType,DisplayType,IsCal,BehvType,TransVoid,Posted,Status"
                                .TableName = "NOTIFYDTL WITH (ROWLOCK)"
                                strSQL = BuildSelect(.CheckFields, .TableName, "TransNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyDtlCont.TransNo) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyDtlCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyDtlCont.ItemCode) & "' AND ItmDESC = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyDtlCont.ItmDesc) & "'")
                            End With
                            rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                            If rdr Is Nothing = False Then
                                With rdr
                                    If .Read Then
                                        blnFound = True
                                        With objSQL
                                            .TableName = "NOTIFYDTL WITH (ROWLOCK)"
                                            strSQL = BuildDelete(.TableName, "TransNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyDtlCont.TransNo) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyDtlCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyDtlCont.ItemCode) & "' AND ItmDESC = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyDtlCont.ItmDesc) & "'")
                                            ListSQL.Add(strSQL)
                                        End With
                                        With objSQL
                                            .TableName = "ITEMLOC WITH (ROWLOCK)"
                                            strSQL = BuildDelete(.TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyDtlCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyDtlCont.ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyDtlCont.ItmName) & "'")
                                            ListSQL.Add(strSQL)
                                        End With
                                    End If
                                    .Close()

                                End With
                            End If
                        Next
                        If ListSQL IsNot Nothing AndAlso ListSQL.Count > 0 Then
                            Try
                                'execute
                                objConn.BatchExecute(ListSQL, CommandType.Text)
                                Return True
                            Catch exExecute As Exception
                                message = exExecute.Message.ToString()
                                Log.Notifier.Notify(exExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic", exExecute.Message & strSQL, exExecute.StackTrace)
                                Return False
                                'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                            End Try
                        End If
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", axDelete.Message, axDelete.StackTrace)
                Return False
                'Throw axDelete
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", exDelete.Message, exDelete.StackTrace)
                Return False
                'Throw exDelete
            Finally
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function SaveMobileApproval(ByVal UserId As String, ByVal ApprovalStatus As String, ByRef message As String) As Boolean
            Dim strSQL As String = ""

            Dim ListSQL As ArrayList = New ArrayList()
            SaveMobileApproval = False

            Try

                StartSQLControl()


                strSQL = "UPDATE usrverify WITH (ROWLOCK) SET status='" & ApprovalStatus & "', synclastupd=getDate() WHERE userid='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserId) & "'"
                ListSQL.Add(strSQL)


                Try
                    'execute
                    objConn.BatchExecute(ListSQL, CommandType.Text)
                    Return True
                Catch axExecute As Exception
                    Dim sqlStatement As String = " "
                    If objConn.FailedSQLStatement.Count > 0 Then
                        sqlStatement &= objConn.FailedSQLStatement.Item(0)
                    End If

                    message = axExecute.Message.ToString()
                    'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                    Log.Notifier.Notify(axExecute)

                    Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
                    Return False
                Finally
                    objSQL.Dispose()
                End Try
                'Return True


            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally

                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Add for log
        Public Function WriteLog(ByVal Msg As String, _
                                 Optional ByVal strPath As String = "C:\Inetpub\wwwroot\ecn\Logs") As Boolean

            WriteLog = True
            Dim objStreamWriter As StreamWriter
            Dim WriteStr As String = ""
            Try
                WriteStr &= "SQL     : " & Msg & vbCrLf
                WriteStr &= "DATE/TIME : " & String.Format("{0:dd-MM-yyyy HH:mm:ss}", Now) & vbCrLf

                objStreamWriter = New StreamWriter(strPath & "\Error " & Format(Now(), "yyyyMMdd") & ".log", True)
                objStreamWriter.WriteLine(WriteStr)
                objStreamWriter.Flush()
                objStreamWriter.Close()

            Catch Meexp As Exception
                WriteLog = False
                Log.Notifier.Notify(Meexp)
                Gibraltar.Agent.Log.Error("eSWISLogic", Meexp.Message, Meexp.StackTrace)
            Finally
                objStreamWriter = Nothing
            End Try
        End Function

        'Start Batch Suspend Branch / Location
        Public Function SuspendLocation(ByVal ParentID As String, ByVal LocID As String, ByRef message As String) As Boolean
            Dim strSQL As String = ""

            Dim ListSQL As ArrayList = New ArrayList()
            SuspendLocation = False

            Try

                StartSQLControl()

                '1. Update BizLocate set Flag = 0
                strSQL = "UPDATE BIZLOCATE WITH (ROWLOCK) SET Flag=0, Active=0, LastUpdate=getDate() WHERE BizLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "'"
                ListSQL.Add(strSQL)

                '2. Update UsrProfile set status=0
                strSQL = "UPDATE USRPROFILE WITH (ROWLOCK) SET Status=0, LastUpdate=getDate() WHERE RefID IN (SELECT EmployeeID FROM EMPLOYEE WITH (NOLOCK) WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "')"
                ListSQL.Add(strSQL)

                '3. Update EMPLOYEE set Flag=0
                strSQL = "UPDATE EMPLOYEE WITH (ROWLOCK) SET Flag=0, LastUpdate=getDate() WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "'"
                ListSQL.Add(strSQL)


                Try
                    'execute
                    objConn.BatchExecute(ListSQL, CommandType.Text)
                    Return True
                Catch axExecute As Exception
                    Dim sqlStatement As String = " "
                    If objConn.FailedSQLStatement.Count > 0 Then
                        sqlStatement &= objConn.FailedSQLStatement.Item(0)
                    End If

                    message = axExecute.Message.ToString()
                    'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                    Log.Notifier.Notify(axExecute)
                    Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
                    Return False
                Finally
                    objSQL.Dispose()
                End Try
                'Return True


            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally

                EndSQLControl()
                EndConnection()
            End Try
        End Function

        ' Start Batch Reactive Suspended Branch/Location
        Public Function ReactivateLocation(ByVal ParentID As String, ByVal LocID As String, ByRef message As String) As Boolean
            Dim strSQL As String = ""

            Dim ListSQL As ArrayList = New ArrayList()
            ReactivateLocation = False

            Try

                StartSQLControl()

                '1. Update BizLocate set Flag = 1
                strSQL = "UPDATE BIZLOCATE WITH (ROWLOCK) SET Flag=1, Active=1 WHERE BizLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "'"
                ListSQL.Add(strSQL)

                '2. Update UsrProfile set status=1
                strSQL = "UPDATE USRPROFILE WITH (ROWLOCK) SET Status=1 WHERE RefID IN (SELECT EmployeeID FROM EMPLOYEE WITH (NOLOCK) WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "')"
                ListSQL.Add(strSQL)

                '3. Update EMPLOYEE set Flag=1
                strSQL = "UPDATE EMPLOYEE WITH (ROWLOCK) SET Flag=1 WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "'"
                ListSQL.Add(strSQL)


                Try
                    'execute
                    objConn.BatchExecute(ListSQL, CommandType.Text)
                    Return True

                Catch axExecute As Exception
                    Dim sqlStatement As String = " "
                    If objConn.FailedSQLStatement.Count > 0 Then
                        sqlStatement &= objConn.FailedSQLStatement.Item(0)
                    End If


                    message = axExecute.Message.ToString()
                    'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                    Log.Notifier.Notify(axExecute)
                    Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
                    Return False

                Finally
                    objSQL.Dispose()
                End Try
                'Return True


            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally

                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'End

        'Start Batch UpdateBalance
        Public Function UpdateBalance(ByVal dt As DataTable, ByVal LocID As String, ByRef message As String) As Boolean
            Dim strSQL As String = ""

            Dim ListSQL As ArrayList = New ArrayList()
            UpdateBalance = False

            Try
                If dt Is Nothing Then

                    'Message return
                Else

                    StartSQLControl()

                    Dim YearCode, MthCode As String
                    YearCode = Now.Year
                    MthCode = Now.Month

                    For i = 0 To dt.Rows.Count - 1
                        '1. Update ItemLoc
                        strSQL = "UPDATE ITEMLOC WITH (ROWLOCK) SET QtyOnHand='" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, dt.Rows(i).Item("QtyOnHand")) & "', " & _
                                 " QtySellable='" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, dt.Rows(i).Item("QtyOnHand")) & "'" & _
                                 " WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' and ItemCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemCode")) & "'" & _
                                 " and ItemName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemName")) & "'"
                        ListSQL.Add(strSQL)

                        '2. Insert ItemSummary
                        strSQL = "SELECT LocID, ItemCode, ItemName, YearCode, MthCode FROM ITEMSUMMARY WITH (NOLOCK) " & _
                                 " WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' and ItemCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemCode")) & "'" & _
                                 " And YearCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, YearCode) & "' and MthCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, MthCode) & "' and ItemName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemName")) & "'"
                        Dim dt2 As New DataTable
                        dt2 = objConn.Execute(strSQL, CommandType.Text, False)
                        If dt2.Rows.Count = 0 Then
                            'strSQL = "INSERT INTO ITEMSUMMARY WITH (ROWLOCK) (LocID,ItemCode,ItemName,YearCode,MthCode,Opening,Closing,FirstGenerated) VALUES " & _
                            '    " ('" & LocID & "','" & dt.Rows(i).Item("ItemCode") & "','" & dt.Rows(i).Item("ItemName") & "','" & _
                            '   YearCode & "','" & MthCode & "','" & dt.Rows(i).Item("QtyOnHand") & "','" & dt.Rows(i).Item("QtyOnHand") & _
                            '   "','" & Now & "')"

                            ' modified by Lily (18062015): add ParseValue function
                            strSQL = "INSERT INTO ITEMSUMMARY WITH (ROWLOCK) (LocID,ItemCode,ItemName,YearCode,MthCode,Opening,Closing,FirstGenerated) VALUES " & " ('" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "','" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemCode")) & "','" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemName")) & "','" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, YearCode) & "','" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, MthCode) & "','" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, dt.Rows(i).Item("QtyOnHand")) & "','" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, dt.Rows(i).Item("QtyOnHand")) & "','" & _
                                Now & "')"
                        Else
                            strSQL = "UPDATE ITEMSUMMARY WITH (ROWLOCK) SET Opening='" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, dt.Rows(i).Item("QtyOnHand")) & "'," & _
                                     " Closing='" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, dt.Rows(i).Item("QtyOnHand")) & "',FirstGenerated=" & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & "" & _
                                     " WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' and ItemCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemCode")) & "'" & _
                                     " And YearCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, YearCode) & "' and MthCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, MthCode) & "' and ItemName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemName")) & "'"

                        End If

                        ListSQL.Add(strSQL)


                        Dim LastMonth = Date.Parse(YearCode & "-" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, MthCode) & "-01").AddMonths(-1)


                        strSQL = "SELECT LocID, ItemCode, ItemName, YearCode, MthCode FROM ITEMSUMMARY WITH (NOLOCK) " & _
                                 " WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' and ItemCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemCode")) & "'" & _
                                 " And YearCode='" & LastMonth.Year() & "' and MthCode='" & LastMonth.Month() & "' and ItemName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemName")) & "'"

                        Dim dtLastMonth As New DataTable

                        dtLastMonth = objConn.Execute(strSQL, CommandType.Text, False)
                        If dtLastMonth.Rows.Count = 0 Then
                            'strSQL = "INSERT INTO ITEMSUMMARY WITH (ROWLOCK) (LocID,ItemCode,ItemName,YearCode,MthCode,Opening,Closing,FirstGenerated,LastUpdate,SyncLastUpd) VALUES " & _
                            '    " ('" & LocID & "','" & dt.Rows(i).Item("ItemCode") & "','" & dt.Rows(i).Item("ItemName") & "','" & _
                            '   YearCode & "','" & LastMonth.Month() & "','" & dt.Rows(i).Item("QtyOnHand") & "','" & dt.Rows(i).Item("QtyOnHand") & _
                            '   "','" & Now & "','" & Now.Date & "','" & Now & "')"

                            'modified by Lily (18062015): Add ParseValue function
                            strSQL = "INSERT INTO ITEMSUMMARY WITH (ROWLOCK) (LocID,ItemCode,ItemName,YearCode,MthCode,Opening,Closing,FirstGenerated,LastUpdate,SyncLastUpd) VALUES " & " ('" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "','" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemCode")) & "','" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemName")) & "','" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, LastMonth.Year()) & "','" & _
                                LastMonth.Month() & "','" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, dt.Rows(i).Item("QtyOnHand")) & "','" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, dt.Rows(i).Item("QtyOnHand")) & "','" & _
                                Now & "','" & Now.Date & "','" & Now & "')"

                        Else
                            'ongoing added by diana 20150120, put to last month closing
                            strSQL = "UPDATE ITEMSUMMARY WITH (ROWLOCK) SET Closing='" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, dt.Rows(i).Item("QtyOnHand")) & "'," & _
                                     "Opening='" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, dt.Rows(i).Item("QtyOnHand")) & "'," & _
                                     "LastUpdate=" & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & "," & _
                                     "SyncLastUpd=" & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " " & _
                                     " WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' and ItemCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemCode")) & "'" & _
                                     " And YearCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LastMonth.Year()) & "' and MthCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LastMonth.Month()) & "' and ItemName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, dt.Rows(i).Item("ItemName")) & "'"
                        End If



                        ListSQL.Add(strSQL)

                    Next

                    '3. Update BizLocate
                    strSQL = "UPDATE BIZLOCATE WITH (ROWLOCK) SET IsStockTake=1 " & _
                             " WHERE BizLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "'"
                    ListSQL.Add(strSQL)

                    Try
                        'execute
                        objConn.BatchExecute(ListSQL, CommandType.Text)
                        Return True

                    Catch axExecute As Exception
                        Dim sqlStatement As String = " "
                        If objConn.FailedSQLStatement.Count > 0 Then
                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
                        End If

                        message = axExecute.Message.ToString()
                        'Throw New ApplicationException("210004 " & axExecute.Message.ToString())

                        Log.Notifier.Notify(axExecute)
                        Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
                        Return False
                    Finally
                        objSQL.Dispose()
                    End Try
                    'Return True
                End If


            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally

                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'End

        'Start Batch Notification
        Public Function SaveNotification(ByVal NotifyhdrCont As Actions.Container.Notifyhdr, ByVal ListNotifydtlCont As List(Of Actions.Container.Notifydtl), ByVal ListContItemLoc As List(Of Actions.Container.Itemloc), ByVal pType As SQLControl.EnumSQLType, ByVal SaveType As Integer, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveNotification = False

            Try
                If NotifyhdrCont Is Nothing OrElse ListNotifydtlCont Is Nothing Then
                    'If NotifyhdrCont Is Nothing OrElse ListNotifydtlCont Is Nothing OrElse ListNotifydtlCont.Count <= 0 Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With NotifyhdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TransNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyhdrCont.TransNo) & "'")
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

                            Return False
                        Else
                            StartSQLControl()

                            'save header
                            With objSQL
                                .TableName = "NOTIFYHDR WITH (ROWLOCK)"
                                .AddField("TransType", NotifyhdrCont.TransType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CompanyID", NotifyhdrCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("LocID", NotifyhdrCont.LocID, SQLControl.EnumDataType.dtString)
                                .AddField("TermID", NotifyhdrCont.TermID, SQLControl.EnumDataType.dtCustom)
                                .AddField("PBT", NotifyhdrCont.PBT, SQLControl.EnumDataType.dtString)
                                .AddField("NoteID", NotifyhdrCont.NoteID, SQLControl.EnumDataType.dtString)
                                .AddField("ReferID", NotifyhdrCont.ReferID, SQLControl.EnumDataType.dtString)
                                .AddField("TransDate", NotifyhdrCont.TransDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("TransReasonCode", NotifyhdrCont.TransReasonCode, SQLControl.EnumDataType.dtString)
                                .AddField("TransRemark", NotifyhdrCont.TransRemark, SQLControl.EnumDataType.dtString)
                                .AddField("TransStatus", NotifyhdrCont.TransStatus, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Posted", NotifyhdrCont.Posted, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PostDate", NotifyhdrCont.PostDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("EntryID", NotifyhdrCont.EntryID, SQLControl.EnumDataType.dtString)
                                .AddField("AuthID", NotifyhdrCont.AuthID, SQLControl.EnumDataType.dtString)
                                .AddField("AuthPOS", NotifyhdrCont.AuthPOS, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", NotifyhdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", NotifyhdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("ApproveDate", NotifyhdrCont.ApproveDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ApproveBy", NotifyhdrCont.ApproveBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", NotifyhdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", NotifyhdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", NotifyhdrCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", NotifyhdrCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                '.AddField("RowGuid", NotifyhdrCont.RowGuid, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyhdrCont.TransNo) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("TransNo", NotifyhdrCont.TransNo, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyhdrCont.TransNo) & "'")
                                End Select

                                ListSQL.Add(strSQL)
                            End With

                            'save all details

                            'Delete first
                            If StartConnection(EnumIsoState.StateUpdatetable) = True Then 'if update then delete
                                StartSQLControl()

                                If ListNotifydtlCont.Count > 0 Then
                                    With objSQL
                                        .TableName = "NOTIFYDTL WITH (ROWLOCK)"
                                        strSQL = BuildDelete(.TableName, "TransNo = '" & ListNotifydtlCont(0).TransNo & "'")
                                        ListSQL.Add(strSQL)
                                    End With
                                End If

                            End If

                            For Each NotifyDtlCont In ListNotifydtlCont



                                With objSQL
                                    .TableName = "NOTIFYDTL WITH (ROWLOCK)"
                                    .AddField("CompanyID", NotifyDtlCont.CompanyID, SQLControl.EnumDataType.dtString)
                                    .AddField("LocID", NotifyDtlCont.LocID, SQLControl.EnumDataType.dtString)
                                    .AddField("StorageID", NotifyDtlCont.StorageID, SQLControl.EnumDataType.dtString)
                                    .AddField("TermID", NotifyDtlCont.TermID, SQLControl.EnumDataType.dtCustom)
                                    .AddField("RecType", NotifyDtlCont.RecType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RefSeq", NotifyDtlCont.RefSeq, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("DisplayType", NotifyDtlCont.DisplayType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("IsCal", NotifyDtlCont.IsCal, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RefNo", NotifyDtlCont.RefNo, SQLControl.EnumDataType.dtString)
                                    .AddField("ItemCode", NotifyDtlCont.ItemCode, SQLControl.EnumDataType.dtString)
                                    .AddField("ItmDesc", NotifyDtlCont.ItmDesc, SQLControl.EnumDataType.dtString)
                                    .AddField("TypeCode", NotifyDtlCont.TypeCode, SQLControl.EnumDataType.dtString)

                                    .AddField("BehvType", NotifyDtlCont.BehvType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Qty", NotifyDtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("PackQty", NotifyDtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("UOM", NotifyDtlCont.UOM, SQLControl.EnumDataType.dtString)
                                    .AddField("ItmName", NotifyDtlCont.ItmName, SQLControl.EnumDataType.dtString)
                                    .AddField("ItmSource", NotifyDtlCont.ItmSource, SQLControl.EnumDataType.dtString)
                                    .AddField("ItmComponent", NotifyDtlCont.ItmComponent, SQLControl.EnumDataType.dtString)
                                    .AddField("SerialNo", NotifyDtlCont.SerialNo, SQLControl.EnumDataType.dtString)
                                    .AddField("TransVoid", NotifyDtlCont.TransVoid, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Posted", NotifyDtlCont.Posted, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Status", NotifyDtlCont.Status, SQLControl.EnumDataType.dtNumeric)
                                    '.AddField("RowGuid", NotifydtlCont.RowGuid, SQLControl.EnumDataType.dtString)
                                    .AddField("CreateDate", NotifyDtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", NotifyDtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", NotifyDtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", NotifyDtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                    'After delete then Insert
                                    .AddField("TransNo", NotifyDtlCont.TransNo, SQLControl.EnumDataType.dtString)
                                    .AddField("TransSeq", NotifyDtlCont.TransSeq, SQLControl.EnumDataType.dtNumeric)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)


                                    ' Select Case pType
                                    'Case SQLControl.EnumSQLType.stInsert
                                    '    .AddField("TransNo", NotifyDtlCont.TransNo, SQLControl.EnumDataType.dtString)
                                    '    .AddField("TransSeq", NotifyDtlCont.TransSeq, SQLControl.EnumDataType.dtNumeric)
                                    '    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    'Case SQLControl.EnumSQLType.stUpdate
                                    '    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransNo = '" & NotifyDtlCont.TransNo & "' AND TransSeq = '" & NotifyDtlCont.TransSeq & "'")
                                    'End Select

                                End With

                                ListSQL.Add(strSQL)

                            Next

                            If SaveType = 1 Then 'if submit then save itemLoc

                                'Save ItemLoc
                                For Each ItemLocCont In ListContItemLoc
                                    '1. Delete
                                    Dim blnInUse As Boolean
                                    blnFlag = False
                                    blnFound = False
                                    blnInUse = False

                                    If ItemLocCont Is Nothing Then
                                        'Error Message
                                    Else
                                        If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                                            StartSQLControl()

                                            With ItemLocInfo.MyInfo

                                                strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "'")
                                            End With
                                            rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                                            If rdr Is Nothing = False Then
                                                With rdr
                                                    If .Read Then
                                                        blnFound = True
                                                        If Convert.ToInt16(.Item("Inuse")) = 1 Then
                                                            blnInUse = True
                                                        End If
                                                    End If
                                                    .Close()
                                                End With
                                            End If

                                            If blnFound = True And blnInUse = True Then

                                                With objSQL
                                                    .TableName = "ITEMLOC WITH (ROWLOCK)"
                                                    strSQL = BuildUpdate(.TableName, " SET Flag = 0" & _
                                                    " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                                    objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.UpdateBy) & "' WHERE" & _
                                                    " LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "'")
                                                End With
                                            End If

                                            If blnFound = True And blnInUse = False Then
                                                With objSQL
                                                    .TableName = "ITEMLOC WITH (ROWLOCK)"
                                                    strSQL = BuildDelete(.TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "'")
                                                End With
                                            End If
                                        End If
                                    End If

                                    StartSQLControl()
                                    '2. Save
                                    With objSQL
                                        .TableName = "ITEMLOC WITH (ROWLOCK)"
                                        .AddField("ItemDesc", ItemLocCont.ItemDesc, SQLControl.EnumDataType.dtStringN)
                                        .AddField("ItemComponent", ItemLocCont.ItemComponent, SQLControl.EnumDataType.dtStringN)
                                        .AddField("ShortDesc", ItemLocCont.ShortDesc, SQLControl.EnumDataType.dtStringN)
                                        .AddField("TariffCode", ItemLocCont.TariffCode, SQLControl.EnumDataType.dtStringN)
                                        .AddField("OrgCountry", ItemLocCont.OrgCountry, SQLControl.EnumDataType.dtString)
                                        .AddField("MATNo", ItemLocCont.MATNo, SQLControl.EnumDataType.dtStringN)
                                        .AddField("MarkNo", ItemLocCont.MarkNo, SQLControl.EnumDataType.dtStringN)
                                        .AddField("ItmSize2", ItemLocCont.ItmSize2, SQLControl.EnumDataType.dtString)
                                        .AddField("ItmSize1", ItemLocCont.ItmSize1, SQLControl.EnumDataType.dtString)
                                        .AddField("ItmSize", ItemLocCont.ItmSize, SQLControl.EnumDataType.dtString)
                                        .AddField("ConSize", ItemLocCont.ConSize, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("ConUOM", ItemLocCont.ConUOM, SQLControl.EnumDataType.dtString)
                                        .AddField("DefUOM", ItemLocCont.DefUOM, SQLControl.EnumDataType.dtString)
                                        .AddField("ClassType", ItemLocCont.ClassType, SQLControl.EnumDataType.dtString)
                                        .AddField("TypeCode", ItemLocCont.TypeCode, SQLControl.EnumDataType.dtString)
                                        .AddField("BehvType", ItemLocCont.BehvType, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("BehvShow", ItemLocCont.BehvShow, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("ComboItem", ItemLocCont.ComboItem, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("ItmCatgCode", ItemLocCont.ItmCatgCode, SQLControl.EnumDataType.dtString)
                                        .AddField("ItmBrand", ItemLocCont.ItmBrand, SQLControl.EnumDataType.dtString)
                                        .AddField("LooseUOM", ItemLocCont.LooseUOM, SQLControl.EnumDataType.dtString)
                                        .AddField("PackUOM", ItemLocCont.PackUOM, SQLControl.EnumDataType.dtString)
                                        .AddField("PackQty", ItemLocCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("IsSales", ItemLocCont.IsSales, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("IsEmpDisc", ItemLocCont.IsEmpDisc, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("IsRtnable", ItemLocCont.IsRtnable, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("IsDisc", ItemLocCont.IsDisc, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("IsFOC", ItemLocCont.IsFOC, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("IsTaxable", ItemLocCont.IsTaxable, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("AvgCost", ItemLocCont.AvgCost, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("StdCost", ItemLocCont.StdCost, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("StdMarkup", ItemLocCont.StdMarkup, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("StdSellPrice", ItemLocCont.StdSellPrice, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("IsSelected", ItemLocCont.IsSelected, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("IsBestBuy", ItemLocCont.IsBestBuy, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("IsPurchase", ItemLocCont.IsPurchase, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("IsWLength", ItemLocCont.IsWLength, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("TrackSerial", ItemLocCont.TrackSerial, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("MinQty", ItemLocCont.MinQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("MaxQty", ItemLocCont.MaxQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("ImageName", ItemLocCont.ImageName, SQLControl.EnumDataType.dtStringN)
                                        .AddField("IncomingQty", ItemLocCont.IncomingQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("ReOrderLvl", ItemLocCont.ReOrderLvl, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("ReOrderQty", ItemLocCont.ReOrderQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("QtyOnHand", ItemLocCont.QtyOnHand, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("QtySellable", ItemLocCont.QtySellable, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("QtyBalance", ItemLocCont.QtyBalance, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("POQty", ItemLocCont.POQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("OutgoingQty", ItemLocCont.OutgoingQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("FirstIn", ItemLocCont.FirstIn, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("LastIn", ItemLocCont.LastIn, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("LastPO", ItemLocCont.LastPO, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("LastOut", ItemLocCont.LastOut, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("LastRV", ItemLocCont.LastRV, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("CummQty", ItemLocCont.CummQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("DayQty", ItemLocCont.DayQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("LDayQty", ItemLocCont.LDayQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("MthQty", ItemLocCont.MthQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("LMthQty", ItemLocCont.LMthQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("MtdQty", ItemLocCont.MtdQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("YrQty", ItemLocCont.YrQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("LYrQty", ItemLocCont.LYrQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("YtdQty", ItemLocCont.YtdQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("ProcessDate", ItemLocCont.ProcessDate, SQLControl.EnumDataType.dtDateTime)
                                        .AddField("CreateDate", ItemLocCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                        .AddField("CreateBy", ItemLocCont.CreateBy, SQLControl.EnumDataType.dtString)
                                        .AddField("LastUpdate", ItemLocCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                        .AddField("UpdateBy", ItemLocCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                        .AddField("Active", ItemLocCont.Active, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("Inuse", ItemLocCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                        '.AddField("rowguid", ItemlocCont.rowguid, SQLControl.EnumDataType.dtString)
                                        .AddField("Flag", ItemLocCont.Flag, SQLControl.EnumDataType.dtNumeric)

                                        .AddField("StorageID", ItemLocCont.StorageID, SQLControl.EnumDataType.dtString)

                                        'Select Case pType
                                        'Case SQLControl.EnumSQLType.stInsert

                                        If blnFound = True Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "'AND ItemName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "'")
                                            'strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND StorageID = '" & ItemlocCont.StorageID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("LocID", ItemLocCont.LocID, SQLControl.EnumDataType.dtString)
                                                '.AddField("StorageID", ItemlocCont.StorageID, SQLControl.EnumDataType.dtString)
                                                .AddField("ItemCode", ItemLocCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                .AddField("ItemName", ItemLocCont.ItemName, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                        'Case SQLControl.EnumSQLType.stUpdate
                                        '    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "'")
                                        '    'strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND StorageID = '" & ItemlocCont.StorageID & "'")
                                        'End Select
                                    End With
                                    ListSQL.Add(strSQL)

                                    'Update STORAGEMASTER if any
                                    strSQL = "UPDATE STORAGEMASTER WITH (ROWLOCK) SET Flag=1 WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND ItemName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "'"
                                    ListSQL.Add(strSQL)

                                    'Update ITEMSUMMARY if any
                                    strSQL = "UPDATE ITEMSUMMARY WITH (ROWLOCK) SET IsHost=0 WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND ItemName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "'"
                                    ListSQL.Add(strSQL)

                                    'execute store proc
                                    strSQL = "DECLARE @ExecDate DATETIME; SET @ExecDate = GETDATE(); EXEC sp_SummaryInitUpdate @ExecDate,'" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "','" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "','" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "'"
                                    ListSQL.Add(strSQL)
                                Next

                            Else
                                For Each ItemLocCont In ListContItemLoc
                                    '1. Delete
                                    Dim blnInUse As Boolean
                                    blnFlag = False
                                    blnFound = False
                                    blnInUse = False
                                    If ItemLocCont Is Nothing Then
                                        'Error Message
                                    Else
                                        If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                                            StartSQLControl()

                                            With ItemLocInfo.MyInfo

                                                strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "'")
                                            End With
                                            rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                                            If rdr Is Nothing = False Then
                                                With rdr
                                                    If .Read Then
                                                        blnFound = True
                                                        If Convert.ToInt16(.Item("Inuse")) = 1 Then
                                                            blnInUse = True
                                                        End If
                                                    End If
                                                    .Close()
                                                End With
                                            End If

                                            If blnFound = False Then
                                                StartSQLControl()
                                                '2. Save
                                                With objSQL
                                                    .TableName = "ITEMLOC WITH (ROWLOCK)"
                                                    .AddField("ItemDesc", ItemLocCont.ItemDesc, SQLControl.EnumDataType.dtStringN)
                                                    .AddField("ItemComponent", ItemLocCont.ItemComponent, SQLControl.EnumDataType.dtStringN)
                                                    .AddField("ShortDesc", ItemLocCont.ShortDesc, SQLControl.EnumDataType.dtStringN)
                                                    .AddField("TariffCode", ItemLocCont.TariffCode, SQLControl.EnumDataType.dtStringN)
                                                    .AddField("OrgCountry", ItemLocCont.OrgCountry, SQLControl.EnumDataType.dtString)
                                                    .AddField("MATNo", ItemLocCont.MATNo, SQLControl.EnumDataType.dtStringN)
                                                    .AddField("MarkNo", ItemLocCont.MarkNo, SQLControl.EnumDataType.dtStringN)
                                                    .AddField("ItmSize2", ItemLocCont.ItmSize2, SQLControl.EnumDataType.dtString)
                                                    .AddField("ItmSize1", ItemLocCont.ItmSize1, SQLControl.EnumDataType.dtString)
                                                    .AddField("ItmSize", ItemLocCont.ItmSize, SQLControl.EnumDataType.dtString)
                                                    .AddField("ConSize", ItemLocCont.ConSize, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("ConUOM", ItemLocCont.ConUOM, SQLControl.EnumDataType.dtString)
                                                    .AddField("DefUOM", ItemLocCont.DefUOM, SQLControl.EnumDataType.dtString)
                                                    .AddField("ClassType", ItemLocCont.ClassType, SQLControl.EnumDataType.dtString)
                                                    .AddField("TypeCode", ItemLocCont.TypeCode, SQLControl.EnumDataType.dtString)
                                                    .AddField("BehvType", ItemLocCont.BehvType, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("BehvShow", ItemLocCont.BehvShow, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("ComboItem", ItemLocCont.ComboItem, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("ItmCatgCode", ItemLocCont.ItmCatgCode, SQLControl.EnumDataType.dtString)
                                                    .AddField("ItmBrand", ItemLocCont.ItmBrand, SQLControl.EnumDataType.dtString)
                                                    .AddField("LooseUOM", ItemLocCont.LooseUOM, SQLControl.EnumDataType.dtString)
                                                    .AddField("PackUOM", ItemLocCont.PackUOM, SQLControl.EnumDataType.dtString)
                                                    .AddField("PackQty", ItemLocCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("IsSales", ItemLocCont.IsSales, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("IsEmpDisc", ItemLocCont.IsEmpDisc, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("IsRtnable", ItemLocCont.IsRtnable, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("IsDisc", ItemLocCont.IsDisc, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("IsFOC", ItemLocCont.IsFOC, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("IsTaxable", ItemLocCont.IsTaxable, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("AvgCost", ItemLocCont.AvgCost, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("StdCost", ItemLocCont.StdCost, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("StdMarkup", ItemLocCont.StdMarkup, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("StdSellPrice", ItemLocCont.StdSellPrice, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("IsSelected", ItemLocCont.IsSelected, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("IsBestBuy", ItemLocCont.IsBestBuy, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("IsPurchase", ItemLocCont.IsPurchase, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("IsWLength", ItemLocCont.IsWLength, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("TrackSerial", ItemLocCont.TrackSerial, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("MinQty", ItemLocCont.MinQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("MaxQty", ItemLocCont.MaxQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("ImageName", ItemLocCont.ImageName, SQLControl.EnumDataType.dtStringN)
                                                    .AddField("IncomingQty", ItemLocCont.IncomingQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("ReOrderLvl", ItemLocCont.ReOrderLvl, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("ReOrderQty", ItemLocCont.ReOrderQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("QtyOnHand", ItemLocCont.QtyOnHand, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("QtySellable", ItemLocCont.QtySellable, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("QtyBalance", ItemLocCont.QtyBalance, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("POQty", ItemLocCont.POQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("OutgoingQty", ItemLocCont.OutgoingQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("FirstIn", ItemLocCont.FirstIn, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("LastIn", ItemLocCont.LastIn, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("LastPO", ItemLocCont.LastPO, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("LastOut", ItemLocCont.LastOut, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("LastRV", ItemLocCont.LastRV, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("CummQty", ItemLocCont.CummQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("DayQty", ItemLocCont.DayQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("LDayQty", ItemLocCont.LDayQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("MthQty", ItemLocCont.MthQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("LMthQty", ItemLocCont.LMthQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("MtdQty", ItemLocCont.MtdQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("YrQty", ItemLocCont.YrQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("LYrQty", ItemLocCont.LYrQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("YtdQty", ItemLocCont.YtdQty, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("ProcessDate", ItemLocCont.ProcessDate, SQLControl.EnumDataType.dtDateTime)
                                                    .AddField("CreateDate", ItemLocCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                                    .AddField("CreateBy", ItemLocCont.CreateBy, SQLControl.EnumDataType.dtString)
                                                    .AddField("LastUpdate", ItemLocCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                                    .AddField("UpdateBy", ItemLocCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                                    .AddField("Active", ItemLocCont.Active, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("Inuse", ItemLocCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                                    '.AddField("rowguid", ItemlocCont.rowguid, SQLControl.EnumDataType.dtString)
                                                    .AddField("Flag", ItemLocCont.Flag, SQLControl.EnumDataType.dtNumeric)

                                                    .AddField("StorageID", ItemLocCont.StorageID, SQLControl.EnumDataType.dtString)

                                                    'Select Case pType
                                                    'Case SQLControl.EnumSQLType.stInsert

                                                    'If blnFound = True Then
                                                    '    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "'AND ItemName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "'")
                                                    '    'strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND StorageID = '" & ItemlocCont.StorageID & "'")
                                                    'Else
                                                    If blnFound = False Then
                                                        .AddField("LocID", ItemLocCont.LocID, SQLControl.EnumDataType.dtString)
                                                        '.AddField("StorageID", ItemlocCont.StorageID, SQLControl.EnumDataType.dtString)
                                                        .AddField("ItemCode", ItemLocCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                        .AddField("ItemName", ItemLocCont.ItemName, SQLControl.EnumDataType.dtString)
                                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                                    End If
                                                    'End If
                                                    'Case SQLControl.EnumSQLType.stUpdate
                                                    '    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "'")
                                                    '    'strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND StorageID = '" & ItemlocCont.StorageID & "'")
                                                    'End Select
                                                End With
                                                ListSQL.Add(strSQL)

                                            End If


                                        End If
                                    End If
                                Next
                            End If 'End if saveType = 1



                            Try
                                'execute
                                'objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                objConn.BatchExecute(ListSQL, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If

                                Dim sqlStatement As String = " "
                                If objConn.FailedSQLStatement.Count > 0 Then
                                    sqlStatement &= objConn.FailedSQLStatement.Item(0)
                                End If

                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
                                Return False
                            Finally
                                objSQL.Dispose()
                            End Try
                            'Return True
                        End If

                    End If
                End If


            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                NotifyhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function InsertNotification(ByVal NotifyhdrCont As Actions.Container.Notifyhdr, ByVal ListNotifydtlCont As List(Of Actions.Container.Notifydtl), ByVal ListContItemLoc As List(Of Actions.Container.Itemloc), ByVal SaveType As Integer, ByRef message As String) As Boolean
            Return SaveNotification(NotifyhdrCont, ListNotifydtlCont, ListContItemLoc, SQLControl.EnumSQLType.stInsert, SaveType, message)
        End Function

        Public Function UpdateNotification(ByVal NotifyhdrCont As Actions.Container.Notifyhdr, ByVal ListNotifydtlCont As List(Of Actions.Container.Notifydtl), ByVal ListContItemLoc As List(Of Actions.Container.Itemloc), ByVal SaveType As Integer, ByRef message As String) As Boolean
            Return SaveNotification(NotifyhdrCont, ListNotifydtlCont, ListContItemLoc, SQLControl.EnumSQLType.stUpdate, SaveType, message)
        End Function

        Public Function CancelSubmit(ByVal NotifyhdrCont As Actions.Container.Notifyhdr, ByVal ListContItemLoc As List(Of Actions.Container.Itemloc), Optional ByVal message As String = Nothing, Optional ByVal isRequest As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim ListSQL As ArrayList = New ArrayList()
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            CancelSubmit = False
            blnFound = False
            blnInUse = False
            Try
                '1. Set flag = 0 for NotifyHdr
                If NotifyhdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        'If blnFound = True Then 'And blnInUse = True Then
                        With objSQL
                            If isRequest Then
                                .TableName = "NOTIFYHDR WITH (ROWLOCK)"
                                strSQL = BuildUpdate(.TableName, " SET Flag = 2, Posted = 2, TransRemark = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyhdrCont.TransRemark) & "'" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyhdrCont.UpdateBy) & "' WHERE " & _
                                "TransNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyhdrCont.TransNo) & "'")
                            Else
                                .TableName = "NOTIFYHDR WITH (ROWLOCK)"
                                strSQL = BuildUpdate(.TableName, " SET Flag = 0, Posted = 3" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyhdrCont.UpdateBy) & "' WHERE " & _
                                "TransNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyhdrCont.TransNo) & "'")
                            End If
                        End With
                        ListSQL.Add(strSQL)
                        If Not isRequest Then
                            For Each ItemLocCont In ListContItemLoc

                                If ItemLocCont Is Nothing Then
                                    'Error Message
                                Else

                                    'Check if cancelSubmit latestNotification
                                    If IsLatestNotification(NotifyhdrCont.TransNo, NotifyhdrCont.LocID, ItemLocCont.ItemCode, ItemLocCont.ItemName) Then

                                        'remarked by diana 20150120, if notif is cancelled, means cannot be shown in new trans anymore
                                        'if used in inventory
                                        'If IsUsed(ItemLocCont.ItemCode, ItemLocCont.LocID, itemLocCont.ItemName) Then
                                        '2. Update ItemLoc set Flag=0
                                        With objSQL
                                            .TableName = "ITEMLOC WITH (ROWLOCK)"
                                            strSQL = BuildUpdate(.TableName, " SET Flag = 0" & _
                                            " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                            objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.UpdateBy) & "' WHERE" & _
                                            " LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "'")
                                        End With
                                        ListSQL.Add(strSQL)

                                        ''3. Update ItmTransHDR set Posted=12 (cancel submit) 'remark by Roslyn, 14/08/2015, no need to change posted to 12 and Flag to 0
                                        'strSQL = "UPDATE ITMTRANSHDR WITH (ROWLOCK) SET Posted=12,Flag=1 WHERE DocCode in" & _
                                        '   " (SELECT DISTINCT DocCode FROM ITMTRANSDTL WITH (NOLOCK) WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' and ItemCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND ItemName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "')"

                                        'ListSQL.Add(strSQL)

                                        ''4. Update CONSIGNHDR set Posted=12 (cancel submit) 'remark by Roslyn, 14/08/2015, no need to change posted to 12 and Flag to 0
                                        'strSQL = "UPDATE CONSIGNHDR WITH (ROWLOCK) SET Status=12,Flag=1 WHERE ContractNo in" & _
                                        '    " (SELECT DISTINCT ContractNo FROM CONSIGNDTL WITH (NOLOCK) WHERE GeneratorLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' and WasteCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' and WasteDescription='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "')"
                                        'ListSQL.Add(strSQL)

                                        '5. Update STORAGEMASTER
                                        strSQL = "UPDATE STORAGEMASTER WITH (ROWLOCK) SET Flag=0 WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND ItemName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "'"
                                        ListSQL.Add(strSQL)

                                        '6. Update ITEMSUMMARY
                                        strSQL = "UPDATE ITEMSUMMARY WITH (ROWLOCK) SET IsHost=1 WHERE LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND ItemName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "'"
                                        ListSQL.Add(strSQL)

                                        'End If 'end if isUsed

                                    End If 'end if isLatestNotification



                                End If 'end If ItemLocCont Is Nothing Then

                            Next
                        End If

                        'Else

                        'End If

                        Try
                            'execute
                            'objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            objConn.BatchExecute(ListSQL, CommandType.Text)

                            Return True
                        Catch exExecute As Exception
                            Dim sqlStatement As String = " "
                            If objConn.FailedSQLStatement.Count > 0 Then
                                sqlStatement &= objConn.FailedSQLStatement.Item(0)
                            End If

                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic", exExecute.Message & sqlStatement, exExecute.StackTrace)
                            Return False
                            'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic", axDelete.Message, axDelete.StackTrace)
                Return False
                'Throw axDelete
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic", exDelete.Message, exDelete.StackTrace)
                Return False
                'Throw exDelete
            Finally
                NotifyhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'End Batch Notification

        'Start Batch Inventory
        Public Function SaveInventory(ByVal ItmtranshdrCont As Actions.Container.Itmtranshdr, ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal pType As SQLControl.EnumSQLType, ByVal SaveType As Integer, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveInventory = False

            Try
                If ItmtranshdrCont Is Nothing OrElse ListItmtransdtlCont Is Nothing Then
                    Return True
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItmtranshdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
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

                            Return False
                        Else
                            StartSQLControl()

                            'save header
                            With objSQL
                                .TableName = "ITMTRANSHDR WITH (ROWLOCK)"
                                .AddField("LocID", ItmtranshdrCont.LocID, SQLControl.EnumDataType.dtString)
                                .AddField("RequestCode", ItmtranshdrCont.RequestCode, SQLControl.EnumDataType.dtString)
                                .AddField("BatchCode", ItmtranshdrCont.BatchCode, SQLControl.EnumDataType.dtString)
                                .AddField("TermID", ItmtranshdrCont.TermID, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RegistedSupp", ItmtranshdrCont.RegistedSupp, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CompanyID", ItmtranshdrCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("CompanyName", ItmtranshdrCont.CompanyName, SQLControl.EnumDataType.dtString)
                                .AddField("CompanyType", ItmtranshdrCont.CompanyType, SQLControl.EnumDataType.dtString)
                                .AddField("TransType", ItmtranshdrCont.TransType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TransDate", ItmtranshdrCont.TransDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("TransInit", ItmtranshdrCont.TransInit, SQLControl.EnumDataType.dtString)
                                .AddField("TransSrc", ItmtranshdrCont.TransSrc, SQLControl.EnumDataType.dtString)
                                .AddField("TransDest", ItmtranshdrCont.TransDest, SQLControl.EnumDataType.dtString)
                                .AddField("InterTrans", ItmtranshdrCont.InterTrans, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Remark", ItmtranshdrCont.Remark, SQLControl.EnumDataType.dtString)
                                .AddField("Reason", ItmtranshdrCont.Reason, SQLControl.EnumDataType.dtString)
                                .AddField("PostDate", ItmtranshdrCont.PostDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Posted", ItmtranshdrCont.Posted, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", ItmtranshdrCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ConfirmDate", ItmtranshdrCont.ConfirmDate, SQLControl.EnumDataType.dtDateTime)
                                '.AddField("ExpiredDate", ItmtranshdrCont.ExpiredDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("AuthID", ItmtranshdrCont.AuthID, SQLControl.EnumDataType.dtString)
                                .AddField("AuthPOS", ItmtranshdrCont.AuthPOS, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", ItmtranshdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ItmtranshdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("ApproveDate", ItmtranshdrCont.ApproveDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ApproveBy", ItmtranshdrCont.ApproveBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ItmtranshdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ItmtranshdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ItmtranshdrCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", ItmtranshdrCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LiveCal", ItmtranshdrCont.LiveCal, SQLControl.EnumDataType.dtNumeric)
                                '.AddField("RowGuid", ItmtranshdrCont.RowGuid, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("DocCode", ItmtranshdrCont.DocCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                                End Select

                                ListSQL.Add(strSQL)

                            End With




                            'save all details

                            'Delete first
                            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                                StartSQLControl()

                                If ListItmtransdtlCont.Count > 0 Then
                                    With objSQL
                                        .TableName = "ITMTRANSDTL WITH (ROWLOCK)"
                                        strSQL = BuildDelete(.TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListItmtransdtlCont(0).DocCode) & "'")
                                        ListSQL.Add(strSQL)
                                    End With
                                End If

                            End If

                            For Each ItmTransDtlCont In ListItmtransdtlCont
                                With objSQL

                                    .TableName = "ITMTRANSDTL WITH (ROWLOCK)"
                                    .AddField("StorageID", ItmTransDtlCont.StorageID, SQLControl.EnumDataType.dtString)
                                    .AddField("TermID", ItmTransDtlCont.TermID, SQLControl.EnumDataType.dtCustom)
                                    .AddField("SeqNo", ItmTransDtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Status", ItmTransDtlCont.Status, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("TransType", ItmTransDtlCont.TransType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OperationType", ItmTransDtlCont.OperationType, SQLControl.EnumDataType.dtString)
                                    .AddField("ItmPrice", ItmTransDtlCont.ItmPrice, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ReqQty", ItmTransDtlCont.ReqQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ReqPackQty", ItmTransDtlCont.ReqPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpeningQty", ItmTransDtlCont.OpeningQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpeningPackQty", ItmTransDtlCont.OpeningPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Qty", ItmTransDtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("PackQty", ItmTransDtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingQty", ItmTransDtlCont.HandlingQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingPackQty", ItmTransDtlCont.HandlingPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ClosingQty", ItmTransDtlCont.ClosingQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ClosingPackQty", ItmTransDtlCont.ClosingPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastInQty", ItmTransDtlCont.LastInQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastInPackQty", ItmTransDtlCont.LastInPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastOutQty", ItmTransDtlCont.LastOutQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastOutPackQty", ItmTransDtlCont.LastOutPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RtnPackQty", ItmTransDtlCont.RtnPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RtnQty", ItmTransDtlCont.RtnQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RecPackQty", ItmTransDtlCont.RecPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RecQty", ItmTransDtlCont.RecQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpiredQty", ItmTransDtlCont.ExpiredQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpiryDate", ItmTransDtlCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("Remark", ItmTransDtlCont.Remark, SQLControl.EnumDataType.dtString)
                                    .AddField("LotNo", ItmTransDtlCont.LotNo, SQLControl.EnumDataType.dtString)
                                    .AddField("CityCode", ItmTransDtlCont.CityCode, SQLControl.EnumDataType.dtString)
                                    .AddField("SecCode", ItmTransDtlCont.SecCode, SQLControl.EnumDataType.dtString)
                                    .AddField("BinCode", ItmTransDtlCont.BinCode, SQLControl.EnumDataType.dtString)
                                    '.AddField("RowGuid", ItmTransDtlCont.RowGuid, SQLControl.EnumDataType.dtString)
                                    .AddField("CreateDate", ItmTransDtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", ItmTransDtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", ItmTransDtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", ItmTransDtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                    .AddField("DocCode", ItmTransDtlCont.DocCode, SQLControl.EnumDataType.dtString)
                                    .AddField("ItemCode", ItmTransDtlCont.ItemCode, SQLControl.EnumDataType.dtString)
                                    .AddField("ItemName", ItmTransDtlCont.ItemName, SQLControl.EnumDataType.dtString)
                                    .AddField("LocID", ItmTransDtlCont.LocID, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)


                                End With

                                ListSQL.Add(strSQL)
                            Next

                            If SaveType = 1 Then 'if submit then save itemLoc
                                StartSQLControl()
                                'execute store proc
                                strSQL = "Exec sp_LiveInv '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'"
                                ListSQL.Add(strSQL)

                            End If 'End if saveType = 1



                            Try
                                'execute
                                'objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                objConn.BatchExecute(ListSQL, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If

                                Dim sqlStatement As String = " "
                                If objConn.FailedSQLStatement.Count > 0 Then
                                    sqlStatement &= objConn.FailedSQLStatement.Item(0)
                                End If

                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
                                Return False

                            Finally
                                objSQL.Dispose()
                            End Try
                            'Return True
                        End If

                    End If
                End If


            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ItmtranshdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function SaveInventoryWR(ByVal ItmtranshdrCont As Actions.Container.Itmtranshdr, ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal ContractNo As String, ByVal pType As SQLControl.EnumSQLType, ByVal SaveType As Integer, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveInventoryWR = False

            Try
                If ItmtranshdrCont Is Nothing OrElse ListItmtransdtlCont Is Nothing Then

                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItmtranshdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
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

                            Return False
                        Else
                            StartSQLControl()

                            'save header
                            With objSQL
                                .TableName = "ITMTRANSHDR WITH (ROWLOCK)"
                                .AddField("LocID", ItmtranshdrCont.LocID, SQLControl.EnumDataType.dtString)
                                .AddField("RequestCode", ItmtranshdrCont.RequestCode, SQLControl.EnumDataType.dtString)
                                .AddField("BatchCode", ItmtranshdrCont.BatchCode, SQLControl.EnumDataType.dtString)
                                .AddField("TermID", ItmtranshdrCont.TermID, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RegistedSupp", ItmtranshdrCont.RegistedSupp, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CompanyID", ItmtranshdrCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("CompanyName", ItmtranshdrCont.CompanyName, SQLControl.EnumDataType.dtString)
                                .AddField("CompanyType", ItmtranshdrCont.CompanyType, SQLControl.EnumDataType.dtString)
                                .AddField("TransType", ItmtranshdrCont.TransType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TransDate", ItmtranshdrCont.TransDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("TransInit", ItmtranshdrCont.TransInit, SQLControl.EnumDataType.dtString)
                                .AddField("TransSrc", ItmtranshdrCont.TransSrc, SQLControl.EnumDataType.dtString)
                                .AddField("TransDest", ItmtranshdrCont.TransDest, SQLControl.EnumDataType.dtString)
                                .AddField("InterTrans", ItmtranshdrCont.InterTrans, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Remark", ItmtranshdrCont.Remark, SQLControl.EnumDataType.dtString)
                                .AddField("Reason", ItmtranshdrCont.Reason, SQLControl.EnumDataType.dtString)
                                .AddField("PostDate", ItmtranshdrCont.PostDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Posted", ItmtranshdrCont.Posted, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", ItmtranshdrCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ConfirmDate", ItmtranshdrCont.ConfirmDate, SQLControl.EnumDataType.dtDateTime)
                                '.AddField("ExpiredDate", ItmtranshdrCont.ExpiredDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("AuthID", ItmtranshdrCont.AuthID, SQLControl.EnumDataType.dtString)
                                .AddField("AuthPOS", ItmtranshdrCont.AuthPOS, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", ItmtranshdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ItmtranshdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("ApproveDate", ItmtranshdrCont.ApproveDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ApproveBy", ItmtranshdrCont.ApproveBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ItmtranshdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ItmtranshdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ItmtranshdrCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", ItmtranshdrCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LiveCal", ItmtranshdrCont.LiveCal, SQLControl.EnumDataType.dtNumeric)
                                '.AddField("RowGuid", ItmtranshdrCont.RowGuid, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("DocCode", ItmtranshdrCont.DocCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                                End Select

                                ListSQL.Add(strSQL)

                            End With




                            'save all details

                            'Delete first
                            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                                StartSQLControl()

                                If ListItmtransdtlCont.Count > 0 Then
                                    With objSQL
                                        .TableName = "ITMTRANSDTL WITH (ROWLOCK)"
                                        strSQL = BuildDelete(.TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListItmtransdtlCont(0).DocCode) & "'")
                                        ListSQL.Add(strSQL)
                                    End With
                                End If

                            End If

                            For Each ItmTransDtlCont In ListItmtransdtlCont
                                With objSQL

                                    .TableName = "ITMTRANSDTL WITH (ROWLOCK)"
                                    .AddField("StorageID", ItmTransDtlCont.StorageID, SQLControl.EnumDataType.dtString)
                                    .AddField("TermID", ItmTransDtlCont.TermID, SQLControl.EnumDataType.dtCustom)
                                    .AddField("SeqNo", ItmTransDtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Status", ItmTransDtlCont.Status, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("TransType", ItmTransDtlCont.TransType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OperationType", ItmTransDtlCont.OperationType, SQLControl.EnumDataType.dtString)
                                    .AddField("ItmPrice", ItmTransDtlCont.ItmPrice, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ReqQty", ItmTransDtlCont.ReqQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ReqPackQty", ItmTransDtlCont.ReqPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpeningQty", ItmTransDtlCont.OpeningQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpeningPackQty", ItmTransDtlCont.OpeningPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Qty", ItmTransDtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("PackQty", ItmTransDtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingQty", ItmTransDtlCont.HandlingQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingPackQty", ItmTransDtlCont.HandlingPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ClosingQty", ItmTransDtlCont.ClosingQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ClosingPackQty", ItmTransDtlCont.ClosingPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastInQty", ItmTransDtlCont.LastInQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastInPackQty", ItmTransDtlCont.LastInPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastOutQty", ItmTransDtlCont.LastOutQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastOutPackQty", ItmTransDtlCont.LastOutPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RtnPackQty", ItmTransDtlCont.RtnPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RtnQty", ItmTransDtlCont.RtnQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RecPackQty", ItmTransDtlCont.RecPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RecQty", ItmTransDtlCont.RecQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpiredQty", ItmTransDtlCont.ExpiredQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpiryDate", ItmTransDtlCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("Remark", ItmTransDtlCont.Remark, SQLControl.EnumDataType.dtString)
                                    .AddField("LotNo", ItmTransDtlCont.LotNo, SQLControl.EnumDataType.dtString)
                                    .AddField("CityCode", ItmTransDtlCont.CityCode, SQLControl.EnumDataType.dtString)
                                    .AddField("SecCode", ItmTransDtlCont.SecCode, SQLControl.EnumDataType.dtString)
                                    .AddField("BinCode", ItmTransDtlCont.BinCode, SQLControl.EnumDataType.dtString)
                                    '.AddField("RowGuid", ItmTransDtlCont.RowGuid, SQLControl.EnumDataType.dtString)
                                    .AddField("CreateDate", ItmTransDtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", ItmTransDtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", ItmTransDtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", ItmTransDtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                    .AddField("DocCode", ItmTransDtlCont.DocCode, SQLControl.EnumDataType.dtString)
                                    .AddField("ItemCode", ItmTransDtlCont.ItemCode, SQLControl.EnumDataType.dtString)
                                    .AddField("ItemName", ItmTransDtlCont.ItemName, SQLControl.EnumDataType.dtString)
                                    .AddField("LocID", ItmTransDtlCont.LocID, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)


                                End With

                                ListSQL.Add(strSQL)
                            Next

                            If SaveType = 1 Then 'if submit then save itemLoc
                                StartSQLControl()
                                'execute store proc
                                strSQL = "Exec sp_LiveInv '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'"
                                ListSQL.Add(strSQL)

                            End If 'End if saveType = 1

                            strSQL = "Exec sp_LiveInvDeductByCN '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ContractNo) & "'"

                            ListSQL.Add(strSQL)

                            Try
                                'execute
                                'objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                objConn.BatchExecute(ListSQL, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If

                                Dim sqlStatement As String = " "
                                If objConn.FailedSQLStatement.Count > 0 Then
                                    sqlStatement &= objConn.FailedSQLStatement.Item(0)
                                End If

                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
                                Return False

                            Finally
                                objSQL.Dispose()
                            End Try
                            'Return True
                        End If

                    End If
                End If


            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ItmtranshdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function InsertInventory(ByVal ItmtranshdrCont As Actions.Container.Itmtranshdr, ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal SaveType As Integer, ByRef message As String) As Boolean
            Return SaveInventory(ItmtranshdrCont, ListItmtransdtlCont, SQLControl.EnumSQLType.stInsert, SaveType, message)
        End Function

        Public Function InsertInventoryWR(ByVal ItmtranshdrCont As Actions.Container.Itmtranshdr, ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal ContractNo As String, ByVal SaveType As Integer, ByRef message As String) As Boolean
            Return SaveInventoryWR(ItmtranshdrCont, ListItmtransdtlCont, ContractNo, SQLControl.EnumSQLType.stInsert, SaveType, message)
        End Function

        Public Function UpdateInventory(ByVal ItmtranshdrCont As Actions.Container.Itmtranshdr, ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal SaveType As Integer, ByRef message As String) As Boolean
            Return SaveInventory(ItmtranshdrCont, ListItmtransdtlCont, SQLControl.EnumSQLType.stUpdate, SaveType, message)
        End Function
        'End Batch Inventory

        'Start Batch ReusedInventory
        Public Function SaveReusedInventory(ByVal ItmtranshdrCont As Actions.Container.Itmtranshdr, ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal pType As SQLControl.EnumSQLType, ByVal SaveType As Integer, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveReusedInventory = False

            Try
                If ItmtranshdrCont Is Nothing OrElse ListItmtransdtlCont Is Nothing Then

                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItmtranshdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
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

                            Return False
                        Else
                            StartSQLControl()

                            'save header
                            With objSQL
                                .TableName = "ITMTRANSHDR WITH (ROWLOCK)"
                                .AddField("LocID", ItmtranshdrCont.LocID, SQLControl.EnumDataType.dtString)
                                .AddField("RequestCode", ItmtranshdrCont.RequestCode, SQLControl.EnumDataType.dtString)
                                .AddField("BatchCode", ItmtranshdrCont.BatchCode, SQLControl.EnumDataType.dtString)
                                .AddField("TermID", ItmtranshdrCont.TermID, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RegistedSupp", ItmtranshdrCont.RegistedSupp, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CompanyID", ItmtranshdrCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("CompanyName", ItmtranshdrCont.CompanyName, SQLControl.EnumDataType.dtStringN)
                                .AddField("CompanyType", ItmtranshdrCont.CompanyType, SQLControl.EnumDataType.dtString)
                                .AddField("TransType", ItmtranshdrCont.TransType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TransDate", ItmtranshdrCont.TransDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("TransInit", ItmtranshdrCont.TransInit, SQLControl.EnumDataType.dtString)
                                .AddField("TransSrc", ItmtranshdrCont.TransSrc, SQLControl.EnumDataType.dtString)
                                .AddField("TransDest", ItmtranshdrCont.TransDest, SQLControl.EnumDataType.dtString)
                                .AddField("InterTrans", ItmtranshdrCont.InterTrans, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Remark", ItmtranshdrCont.Remark, SQLControl.EnumDataType.dtStringN)
                                .AddField("Reason", ItmtranshdrCont.Reason, SQLControl.EnumDataType.dtStringN)
                                .AddField("PostDate", ItmtranshdrCont.PostDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Posted", ItmtranshdrCont.Posted, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", ItmtranshdrCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ConfirmDate", ItmtranshdrCont.ConfirmDate, SQLControl.EnumDataType.dtDateTime)
                                '.AddField("ExpiredDate", ItmtranshdrCont.ExpiredDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("AuthID", ItmtranshdrCont.AuthID, SQLControl.EnumDataType.dtString)
                                .AddField("AuthPOS", ItmtranshdrCont.AuthPOS, SQLControl.EnumDataType.dtStringN)
                                .AddField("CreateDate", ItmtranshdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ItmtranshdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("ApproveDate", ItmtranshdrCont.ApproveDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ApproveBy", ItmtranshdrCont.ApproveBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ItmtranshdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ItmtranshdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ItmtranshdrCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", ItmtranshdrCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LiveCal", ItmtranshdrCont.LiveCal, SQLControl.EnumDataType.dtNumeric)
                                '.AddField("RowGuid", ItmtranshdrCont.RowGuid, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("DocCode", ItmtranshdrCont.DocCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                                End Select

                                ListSQL.Add(strSQL)

                            End With

                            'save all details

                            'Delete first
                            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                                StartSQLControl()

                                If ListItmtransdtlCont.Count > 0 Then
                                    With objSQL
                                        .TableName = "ITMTRANSDTL WITH (ROWLOCK)"
                                        strSQL = BuildDelete(.TableName, "DocCode = '" & ListItmtransdtlCont(0).DocCode & "'")
                                        ListSQL.Add(strSQL)
                                    End With
                                End If

                            End If

                            For Each ItmTransDtlCont In ListItmtransdtlCont
                                With objSQL

                                    .TableName = "ITMTRANSDTL WITH (ROWLOCK)"
                                    .AddField("StorageID", ItmTransDtlCont.StorageID, SQLControl.EnumDataType.dtString)
                                    .AddField("TermID", ItmTransDtlCont.TermID, SQLControl.EnumDataType.dtCustom)
                                    .AddField("SeqNo", ItmTransDtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Status", ItmTransDtlCont.Status, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("TransType", ItmTransDtlCont.TransType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OperationType", ItmTransDtlCont.OperationType, SQLControl.EnumDataType.dtString)
                                    .AddField("ItmPrice", ItmTransDtlCont.ItmPrice, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ReqQty", ItmTransDtlCont.ReqQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ReqPackQty", ItmTransDtlCont.ReqPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpeningQty", ItmTransDtlCont.OpeningQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpeningPackQty", ItmTransDtlCont.OpeningPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Qty", ItmTransDtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("PackQty", ItmTransDtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingQty", ItmTransDtlCont.HandlingQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingPackQty", ItmTransDtlCont.HandlingPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ClosingQty", ItmTransDtlCont.ClosingQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ClosingPackQty", ItmTransDtlCont.ClosingPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastInQty", ItmTransDtlCont.LastInQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastInPackQty", ItmTransDtlCont.LastInPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastOutQty", ItmTransDtlCont.LastOutQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastOutPackQty", ItmTransDtlCont.LastOutPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RtnPackQty", ItmTransDtlCont.RtnPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RtnQty", ItmTransDtlCont.RtnQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RecPackQty", ItmTransDtlCont.RecPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RecQty", ItmTransDtlCont.RecQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpiredQty", ItmTransDtlCont.ExpiredQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpiryDate", ItmTransDtlCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("Remark", ItmTransDtlCont.Remark, SQLControl.EnumDataType.dtStringN)
                                    .AddField("LotNo", ItmTransDtlCont.LotNo, SQLControl.EnumDataType.dtStringN)
                                    .AddField("CityCode", ItmTransDtlCont.CityCode, SQLControl.EnumDataType.dtStringN)
                                    .AddField("SecCode", ItmTransDtlCont.SecCode, SQLControl.EnumDataType.dtStringN)
                                    .AddField("BinCode", ItmTransDtlCont.BinCode, SQLControl.EnumDataType.dtStringN)
                                    '.AddField("RowGuid", ItmTransDtlCont.RowGuid, SQLControl.EnumDataType.dtString)
                                    .AddField("CreateDate", ItmTransDtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", ItmTransDtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", ItmTransDtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", ItmTransDtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                    .AddField("DocCode", ItmTransDtlCont.DocCode, SQLControl.EnumDataType.dtString)
                                    .AddField("ItemCode", ItmTransDtlCont.ItemCode, SQLControl.EnumDataType.dtString)
                                    .AddField("ItemName", ItmTransDtlCont.ItemName, SQLControl.EnumDataType.dtString)
                                    .AddField("LocID", ItmTransDtlCont.LocID, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)


                                End With

                                ListSQL.Add(strSQL)
                            Next

                            If SaveType = 1 Then 'if submit then save itemLoc
                                StartSQLControl()
                                'execute store proc
                                strSQL = "Exec sp_LiveInv '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'"
                                ListSQL.Add(strSQL)

                            End If 'End if saveType = 1



                            Try
                                'execute
                                'objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                objConn.BatchExecute(ListSQL, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If

                                Dim sqlStatement As String = " "
                                If objConn.FailedSQLStatement.Count > 0 Then
                                    sqlStatement &= objConn.FailedSQLStatement.Item(0)
                                End If

                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
                                Return False
                            Finally
                                objSQL.Dispose()
                            End Try
                            'Return True
                        End If

                    End If
                End If


            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ItmtranshdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function InsertReusedInventory(ByVal ItmtranshdrCont As Actions.Container.Itmtranshdr, ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal SaveType As Integer, ByRef message As String) As Boolean
            Return SaveReusedInventory(ItmtranshdrCont, ListItmtransdtlCont, SQLControl.EnumSQLType.stInsert, SaveType, message)
        End Function

        Public Function UpdateReusedInventory(ByVal ItmtranshdrCont As Actions.Container.Itmtranshdr, ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal SaveType As Integer, ByRef message As String) As Boolean
            Return SaveReusedInventory(ItmtranshdrCont, ListItmtransdtlCont, SQLControl.EnumSQLType.stUpdate, SaveType, message)
        End Function
        'End Batch ReusedInventory

        'Start Batch Inventory Adjustment 19 Nov 2014
        Public Function SaveAdjustment(ByVal ListItmtranshdrCont As List(Of Actions.Container.Itmtranshdr), ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal pType As SQLControl.EnumSQLType, ByVal SaveType As Integer, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveAdjustment = False

            Try
                If ListItmtranshdrCont Is Nothing OrElse ListItmtransdtlCont Is Nothing Then

                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    StartSQLControl()

                    With objSQL
                        Select Case pType
                            Case SQLControl.EnumSQLType.stInsert

                            Case SQLControl.EnumSQLType.stUpdate
                                With objSQL
                                    .TableName = "ITMTRANSHDR WITH (ROWLOCK)"
                                    strSQL = BuildDelete(.TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListItmtranshdrCont(0).DocCode) & "'")
                                    ListSQL.Add(strSQL)
                                End With
                        End Select
                    End With


                    'save header
                    For Each ItmtranshdrCont In ListItmtranshdrCont
                        With objSQL
                            .TableName = "ITMTRANSHDR WITH (ROWLOCK)"
                            .AddField("LocID", ItmtranshdrCont.LocID, SQLControl.EnumDataType.dtString)
                            .AddField("RequestCode", ItmtranshdrCont.RequestCode, SQLControl.EnumDataType.dtString)
                            .AddField("BatchCode", ItmtranshdrCont.BatchCode, SQLControl.EnumDataType.dtString)
                            .AddField("TermID", ItmtranshdrCont.TermID, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RegistedSupp", ItmtranshdrCont.RegistedSupp, SQLControl.EnumDataType.dtNumeric)
                            .AddField("CompanyID", ItmtranshdrCont.CompanyID, SQLControl.EnumDataType.dtString)
                            .AddField("CompanyName", ItmtranshdrCont.CompanyName, SQLControl.EnumDataType.dtStringN)
                            .AddField("CompanyType", ItmtranshdrCont.CompanyType, SQLControl.EnumDataType.dtString)
                            .AddField("TransType", ItmtranshdrCont.TransType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("TransDate", ItmtranshdrCont.TransDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("TransInit", ItmtranshdrCont.TransInit, SQLControl.EnumDataType.dtString)
                            .AddField("TransSrc", ItmtranshdrCont.TransSrc, SQLControl.EnumDataType.dtString)
                            .AddField("TransDest", ItmtranshdrCont.TransDest, SQLControl.EnumDataType.dtString)
                            .AddField("InterTrans", ItmtranshdrCont.InterTrans, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Remark", ItmtranshdrCont.Remark, SQLControl.EnumDataType.dtStringN)
                            .AddField("Reason", ItmtranshdrCont.Reason, SQLControl.EnumDataType.dtStringN)
                            .AddField("PostDate", ItmtranshdrCont.PostDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("Posted", ItmtranshdrCont.Posted, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Status", ItmtranshdrCont.Status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ConfirmDate", ItmtranshdrCont.ConfirmDate, SQLControl.EnumDataType.dtDateTime)
                            '.AddField("ExpiredDate", ItmtranshdrCont.ExpiredDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("AuthID", ItmtranshdrCont.AuthID, SQLControl.EnumDataType.dtString)
                            .AddField("AuthPOS", ItmtranshdrCont.AuthPOS, SQLControl.EnumDataType.dtStringN)
                            .AddField("CreateDate", ItmtranshdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", ItmtranshdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("ApproveDate", ItmtranshdrCont.ApproveDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("ApproveBy", ItmtranshdrCont.ApproveBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", ItmtranshdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", ItmtranshdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                            .AddField("Active", ItmtranshdrCont.Active, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Inuse", ItmtranshdrCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LiveCal", ItmtranshdrCont.LiveCal, SQLControl.EnumDataType.dtNumeric)
                            '.AddField("RowGuid", ItmtranshdrCont.RowGuid, SQLControl.EnumDataType.dtString)

                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    If blnFound = True And blnFlag = False Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                                    Else
                                        If blnFound = False Then
                                            .AddField("DocCode", ItmtranshdrCont.DocCode, SQLControl.EnumDataType.dtString)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                        End If
                                    End If
                                Case SQLControl.EnumSQLType.stUpdate
                                    'strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                                    .AddField("DocCode", ItmtranshdrCont.DocCode, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            End Select
                        End With
                        ListSQL.Add(strSQL)
                    Next


                    'save all details

                    'Delete first
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()

                        If ListItmtransdtlCont.Count > 0 Then
                            With objSQL
                                .TableName = "ITMTRANSDTL WITH (ROWLOCK)"
                                strSQL = BuildDelete(.TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListItmtransdtlCont(0).DocCode) & "'")
                                ListSQL.Add(strSQL)
                            End With
                        End If

                    End If

                    For Each ItmTransDtlCont In ListItmtransdtlCont
                        With objSQL

                            .TableName = "ITMTRANSDTL WITH (ROWLOCK)"
                            .AddField("StorageID", ItmTransDtlCont.StorageID, SQLControl.EnumDataType.dtString)
                            .AddField("TermID", ItmTransDtlCont.TermID, SQLControl.EnumDataType.dtCustom)
                            .AddField("SeqNo", ItmTransDtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Status", ItmTransDtlCont.Status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("TransType", ItmTransDtlCont.TransType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OperationType", ItmTransDtlCont.OperationType, SQLControl.EnumDataType.dtString)
                            .AddField("ItmPrice", ItmTransDtlCont.ItmPrice, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ReqQty", ItmTransDtlCont.ReqQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ReqPackQty", ItmTransDtlCont.ReqPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpeningQty", ItmTransDtlCont.OpeningQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpeningPackQty", ItmTransDtlCont.OpeningPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Qty", ItmTransDtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("PackQty", ItmTransDtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("HandlingQty", ItmTransDtlCont.HandlingQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("HandlingPackQty", ItmTransDtlCont.HandlingPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ClosingQty", ItmTransDtlCont.ClosingQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ClosingPackQty", ItmTransDtlCont.ClosingPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastInQty", ItmTransDtlCont.LastInQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastInPackQty", ItmTransDtlCont.LastInPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastOutQty", ItmTransDtlCont.LastOutQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastOutPackQty", ItmTransDtlCont.LastOutPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RtnPackQty", ItmTransDtlCont.RtnPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RtnQty", ItmTransDtlCont.RtnQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RecPackQty", ItmTransDtlCont.RecPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RecQty", ItmTransDtlCont.RecQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ExpiredQty", ItmTransDtlCont.ExpiredQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ExpiryDate", ItmTransDtlCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("Remark", ItmTransDtlCont.Remark, SQLControl.EnumDataType.dtStringN)
                            .AddField("LotNo", ItmTransDtlCont.LotNo, SQLControl.EnumDataType.dtStringN)
                            .AddField("CityCode", ItmTransDtlCont.CityCode, SQLControl.EnumDataType.dtStringN)
                            .AddField("SecCode", ItmTransDtlCont.SecCode, SQLControl.EnumDataType.dtStringN)
                            .AddField("BinCode", ItmTransDtlCont.BinCode, SQLControl.EnumDataType.dtStringN)
                            '.AddField("RowGuid", ItmTransDtlCont.RowGuid, SQLControl.EnumDataType.dtString)
                            .AddField("CreateDate", ItmTransDtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", ItmTransDtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", ItmTransDtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", ItmTransDtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

                            .AddField("DocCode", ItmTransDtlCont.DocCode, SQLControl.EnumDataType.dtString)
                            .AddField("ItemCode", ItmTransDtlCont.ItemCode, SQLControl.EnumDataType.dtString)
                            .AddField("LocID", ItmTransDtlCont.LocID, SQLControl.EnumDataType.dtString)
                            .AddField("ItemName", ItmTransDtlCont.ItemName, SQLControl.EnumDataType.dtString)

                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)


                        End With

                        ListSQL.Add(strSQL)
                    Next

                    If SaveType = 1 Then 'if submit then save itemLoc
                        StartSQLControl()
                        'execute store proc
                        strSQL = "Exec sp_LiveInv '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListItmtranshdrCont(0).DocCode) & "'"
                        ListSQL.Add(strSQL)

                    End If 'End if saveType = 1


                    Try
                        'execute
                        'objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                        objConn.BatchExecute(ListSQL, CommandType.Text)
                        Return True

                    Catch axExecute As Exception
                        If pType = SQLControl.EnumSQLType.stInsert Then
                            message = axExecute.Message.ToString()
                            'Throw New ApplicationException("210002 " & axExecute.Message.ToString())

                        Else
                            message = axExecute.Message.ToString()
                            'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                        End If

                        Dim sqlStatement As String = " "
                        If objConn.FailedSQLStatement.Count > 0 Then
                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
                        End If

                        Log.Notifier.Notify(axExecute)
                        Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
                        Return False

                    Finally
                        objSQL.Dispose()
                    End Try

                End If



            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                'ItmtranshdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function InsertAdjustment(ByVal ListItmtranshdrCont As List(Of Actions.Container.Itmtranshdr), ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal SaveType As Integer, ByRef message As String) As Boolean
            Return SaveAdjustment(ListItmtranshdrCont, ListItmtransdtlCont, SQLControl.EnumSQLType.stInsert, SaveType, message)
        End Function

        Public Function UpdateAdjustment(ByVal ListItmtranshdrCont As List(Of Actions.Container.Itmtranshdr), ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal SaveType As Integer, ByRef message As String) As Boolean
            Return SaveAdjustment(ListItmtranshdrCont, ListItmtransdtlCont, SQLControl.EnumSQLType.stUpdate, SaveType, message)
        End Function
        'End Batch Inventory Adjustment

        'function for Consignment Note
        'Public Function SaveConsignment(ByVal ConsignhdrCont As Actions.Container.Consignhdr, ByVal ListCNdtlCont As List(Of Actions.Container.Consigndtl), ByVal EmployeeCont As Profiles.Container.Employee, ByVal VehicleCont As Profiles.Container.Vehicle, ByVal ListCNLblCont As List(Of Actions.Container.Consignlabel), ByVal ListCNLogCont As List(Of Actions.Container.Consignlog), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, ByVal Deduct As Boolean, ByVal isBackdated As Boolean, Optional ByVal isDeleteLabel As Integer = 0, Optional ByVal MaxSeqno As Integer = 0) As Boolean
        '    Dim strSQL As String = ""
        '    Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
        '    Dim rdr As System.Data.SqlClient.SqlDataReader
        '    Dim ListSQL As ArrayList = New ArrayList()
        '    SaveConsignment = False

        '    Try
        '        If ConsignhdrCont Is Nothing OrElse ListCNdtlCont Is Nothing OrElse ListCNdtlCont.Count <= 0 Then
        '            'Message return
        '        Else
        '            blnExec = False
        '            blnFound = False
        '            blnFlag = False
        '            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                StartSQLControl()
        '                With ConsignhdrInfo.MyInfo
        '                    strSQL = BuildSelect(.CheckFields, .TableName, "TransID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                End With
        '                rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
        '                blnExec = True

        '                If rdr Is Nothing = False Then
        '                    With rdr
        '                        If .Read Then
        '                            blnFound = True
        '                            If Convert.ToInt16(.Item("Flag")) = 0 Then
        '                                'Found but deleted
        '                                blnFlag = False
        '                            Else
        '                                'Found and active
        '                                blnFlag = True
        '                            End If
        '                        End If
        '                        .Close()
        '                    End With
        '                End If
        '            End If

        '            If blnExec Then
        '                If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
        '                    message = "Record already exist"
        '                    'Throw New ApplicationException("210011: Record already exist")
        '                    'Item found & active
        '                    'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIconInformation,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
        '                    'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
        '                    Return False
        '                Else
        '                    StartSQLControl()

        '                    'save header
        '                    With objSQL
        '                        .TableName = "CONSIGNHDR WITH (ROWLOCK)"
        '                        .AddField("ReferID", ConsignhdrCont.ReferID, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransType", ConsignhdrCont.TransType, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransDate", ConsignhdrCont.TransDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("Status", ConsignhdrCont.Status, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("TransportStatus", ConsignhdrCont.TransportStatus, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("ReceiveStatus", ConsignhdrCont.ReceiveStatus, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("GeneratorID", ConsignhdrCont.GeneratorID, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorLocID", ConsignhdrCont.GeneratorLocID, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorAddress", ConsignhdrCont.GeneratorAddress, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorPIC", ConsignhdrCont.GeneratorPIC, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("GeneratorPos", ConsignhdrCont.GeneratorPos, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("GeneratorTelNo", ConsignhdrCont.GeneratorTelNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorFaxNo", ConsignhdrCont.GeneratorFaxNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorEmailAddress", ConsignhdrCont.GeneratorEmailAddress, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("TransporterID", ConsignhdrCont.TransporterID, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterLocID", ConsignhdrCont.TransporterLocID, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterAddress", ConsignhdrCont.TransporterAddress, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterPIC", ConsignhdrCont.TransporterPIC, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("TransporterPos", ConsignhdrCont.TransporterPos, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("TransporterTelNo", ConsignhdrCont.TransporterTelNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterFaxNo", ConsignhdrCont.TransporterFaxNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterEmailAddress", ConsignhdrCont.TransporterEmailAddress, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("VehicleID", ConsignhdrCont.VehicleID, SQLControl.EnumDataType.dtString)
        '                        .AddField("DriverID", ConsignhdrCont.DriverID, SQLControl.EnumDataType.dtString)
        '                        .AddField("DriverName", ConsignhdrCont.DriverName, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ICNo", ConsignhdrCont.ICNo, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ReceiverID", ConsignhdrCont.ReceiverID, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverLocID", ConsignhdrCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverAddress", ConsignhdrCont.ReceiverAddress, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverPIC", ConsignhdrCont.ReceiverPIC, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ReceiverPos", ConsignhdrCont.ReceiverPos, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ReceiverTelNo", ConsignhdrCont.ReceiverTelNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverFaxNo", ConsignhdrCont.ReceiverFaxNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverEmailAddress", ConsignhdrCont.ReceiverEmailAddress, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("PremiseID", ConsignhdrCont.PremiseID, SQLControl.EnumDataType.dtString)
        '                        .AddField("TempStorage1", ConsignhdrCont.TempStorage1, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("TempStorage2", ConsignhdrCont.TempStorage2, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("DeliveryDate", ConsignhdrCont.DeliveryDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("TargetTransportDate", ConsignhdrCont.TargetTransportDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("TransportDate", ConsignhdrCont.TransportDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("TargetReceiveDate", ConsignhdrCont.TargetReceiveDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("ReceiveDate", ConsignhdrCont.ReceiveDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("DeliveryRemark", ConsignhdrCont.DeliveryRemark, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ReceiveRemark", ConsignhdrCont.ReceiveRemark, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("CreateDate", ConsignhdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("CreateBy", ConsignhdrCont.CreateBy, SQLControl.EnumDataType.dtString)
        '                        .AddField("LastUpdate", ConsignhdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("UpdateBy", ConsignhdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
        '                        '.AddField("rowguid", ConsignhdrCont.rowguid, SQLControl.EnumDataType.dtString)
        '                        .AddField("IsHost", ConsignhdrCont.IsHost, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("IsConfirm", ConsignhdrCont.IsConfirm, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("IsNew", ConsignhdrCont.IsNew, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("LastSyncBy", ConsignhdrCont.LastSyncBy, SQLControl.EnumDataType.dtString)
        '                        .AddField("Active", ConsignhdrCont.Active, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("Flag", ConsignhdrCont.Flag, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("LiveCal", ConsignhdrCont.LiveCal, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("SubmissionDate", ConsignhdrCont.SubmissionDate, SQLControl.EnumDataType.dtDateTime)

        '                        Select Case pType
        '                            Case SQLControl.EnumSQLType.stInsert
        '                                If blnFound = True And blnFlag = False Then
        '                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                                Else
        '                                    If blnFound = False Then
        '                                        .AddField("TransID", ConsignhdrCont.TransID, SQLControl.EnumDataType.dtString)
        '                                        .AddField("ContractNo", ConsignhdrCont.ContractNo, SQLControl.EnumDataType.dtString)
        '                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                                    End If
        '                                End If
        '                            Case SQLControl.EnumSQLType.stUpdate
        '                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                        End Select

        '                        ListSQL.Add(strSQL)
        '                    End With

        '                    'save all details
        '                    For Each CNDtlCont In ListCNdtlCont
        '                        With objSQL
        '                            .TableName = "CONSIGNDTL WITH (ROWLOCK)"

        '                            .AddField("WasteCode", CNDtlCont.WasteCode, SQLControl.EnumDataType.dtString)
        '                            .AddField("WasteDescription", CNDtlCont.WasteDescription, SQLControl.EnumDataType.dtString)
        '                            .AddField("WasteComponent", CNDtlCont.WasteComponent, SQLControl.EnumDataType.dtString)
        '                            .AddField("WasteType", CNDtlCont.WasteType, SQLControl.EnumDataType.dtString)
        '                            .AddField("WastePackage", CNDtlCont.WastePackage, SQLControl.EnumDataType.dtString)
        '                            .AddField("OriginCode", CNDtlCont.OriginCode, SQLControl.EnumDataType.dtString)
        '                            .AddField("OriginDescription", CNDtlCont.OriginDescription, SQLControl.EnumDataType.dtString)
        '                            .AddField("SerialNo", CNDtlCont.SerialNo, SQLControl.EnumDataType.dtString)
        '                            .AddField("Qty", CNDtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("PackQty", CNDtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("StorageID", CNDtlCont.StorageID, SQLControl.EnumDataType.dtString)
        '                            .AddField("CountryCode", CNDtlCont.CountryCode, SQLControl.EnumDataType.dtString)
        '                            .AddField("PackagingQty", CNDtlCont.PackagingQty, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("PackagingQtyKg", CNDtlCont.PackagingQtyKg, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("RcvQty", CNDtlCont.RcvQty, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("RcvPackQty", CNDtlCont.RcvPackQty, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("TreatmentCost", CNDtlCont.TreatmentCost, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("Remark", CNDtlCont.Remark, SQLControl.EnumDataType.dtString)
        '                            .AddField("OperationType", CNDtlCont.OperationType, SQLControl.EnumDataType.dtString)

        '                            '.AddField("RowGuid", ItmTransDtlCont.RowGuid, SQLControl.EnumDataType.dtString)
        '                            .AddField("CreateDate", CNDtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
        '                            .AddField("CreateBy", CNDtlCont.CreateBy, SQLControl.EnumDataType.dtString)
        '                            .AddField("AuthorisedBy", CNDtlCont.AuthorisedBy, SQLControl.EnumDataType.dtString)
        '                            .AddField("AuthorisedDate", CNDtlCont.AuthorisedDate, SQLControl.EnumDataType.dtDateTime)
        '                            .AddField("LastSyncBy", CNDtlCont.LastSyncBy, SQLControl.EnumDataType.dtString)
        '                            .AddField("LastUpdate", CNDtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
        '                            .AddField("UpdateBy", CNDtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

        '                            .AddField("ReceivedDate", CNDtlCont.ReceivedDate, SQLControl.EnumDataType.dtDateTime)
        '                            .AddField("ReceivedBy", CNDtlCont.ReceivedBy, SQLControl.EnumDataType.dtString)

        '                            Select Case pType
        '                                Case SQLControl.EnumSQLType.stInsert
        '                                    If blnFound = True And blnFlag = False Then
        '                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CNDtlCont.ContractNo) & "' AND SeqNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, CNDtlCont.SeqNo) & "'")
        '                                    Else
        '                                        If blnFound = False Then
        '                                            .AddField("TransID", CNDtlCont.TransID, SQLControl.EnumDataType.dtString)
        '                                            .AddField("ContractNo", CNDtlCont.ContractNo, SQLControl.EnumDataType.dtString)
        '                                            .AddField("SeqNo", CNDtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
        '                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                                        End If
        '                                    End If
        '                                Case SQLControl.EnumSQLType.stUpdate
        '                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CNDtlCont.ContractNo) & "' AND SeqNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, CNDtlCont.SeqNo) & "'")
        '                            End Select

        '                        End With

        '                        ListSQL.Add(strSQL)
        '                    Next

        '                    'save consignlabel
        '                    If isDeleteLabel > 0 Then   'delete remain label                               
        '                        For i As Integer = MaxSeqno To MaxSeqno - isDeleteLabel + 1 Step -1
        '                            strSQL = "DELETE CONSIGNLABEL WHERE TransID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' and seqno=" & i
        '                            ListSQL.Add(strSQL)
        '                        Next

        '                    End If

        '                    If ListCNLblCont IsNot Nothing Then
        '                        For Each ConsignlabelCont In ListCNLblCont
        '                            With objSQL
        '                                .TableName = "CONSIGNLABEL WITH (ROWLOCK)"
        '                                .AddField("GeneratorID", ConsignlabelCont.GeneratorID, SQLControl.EnumDataType.dtString)
        '                                .AddField("ReceiverID", ConsignlabelCont.ReceiverID, SQLControl.EnumDataType.dtString)
        '                                .AddField("SeqNo", ConsignlabelCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("AuthCode", ConsignlabelCont.AuthCode, SQLControl.EnumDataType.dtString)
        '                                .AddField("TransferDate", ConsignlabelCont.TransferDate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("ExpiryDate", ConsignlabelCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("ValidateDate", ConsignlabelCont.ValidateDate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("ValidateBy", ConsignlabelCont.ValidateBy, SQLControl.EnumDataType.dtString)
        '                                .AddField("Validated", ConsignlabelCont.Validated, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("ShortNote", ConsignlabelCont.ShortNote, SQLControl.EnumDataType.dtString)
        '                                .AddField("Active", ConsignlabelCont.Active, SQLControl.EnumDataType.dtNumeric)
        '                                '.AddField("rowguid", ConsignlabelCont.rowguid, SQLControl.EnumDataType.dtString)
        '                                .AddField("IsHost", ConsignlabelCont.IsHost, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("LastSyncBy", ConsignlabelCont.LastSyncBy, SQLControl.EnumDataType.dtString)
        '                                .AddField("Flag", ConsignlabelCont.Flag, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("TransID", ConsignlabelCont.TransID, SQLControl.EnumDataType.dtString)
        '                                .AddField("ContractNo", ConsignlabelCont.ContractNo, SQLControl.EnumDataType.dtString)
        '                                .AddField("ActivedDate", ConsignlabelCont.ActivedDate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("LabelID", ConsignlabelCont.LabelID, SQLControl.EnumDataType.dtString)
        '                                .AddField("LabelType", ConsignlabelCont.LabelType, SQLControl.EnumDataType.dtNumeric)

        '                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                            End With
        '                            ListSQL.Add(strSQL)
        '                        Next
        '                    End If

        '                    strSQL = "DELETE FROM CONSIGNLOG WHERE ContractNo='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "' AND TransID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "'"
        '                    ListSQL.Add(strSQL)

        '                    'save consignlog for backdatedcn
        '                    If isBackdated Then
        '                        If ListCNLogCont IsNot Nothing Then
        '                            For Each ConsignlogCont In ListCNLogCont
        '                                With objSQL
        '                                    .TableName = "Consignlog"
        '                                    .AddField("ReferID", ConsignlogCont.ReferID, SQLControl.EnumDataType.dtString)
        '                                    .AddField("TransType", ConsignlogCont.TransType, SQLControl.EnumDataType.dtString)
        '                                    .AddField("TransDate", ConsignlogCont.TransDate, SQLControl.EnumDataType.dtDateTime)
        '                                    .AddField("Status", ConsignlogCont.Status, SQLControl.EnumDataType.dtNumeric)
        '                                    .AddField("CreateDate", ConsignlogCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
        '                                    .AddField("CreateBy", ConsignlogCont.CreateBy, SQLControl.EnumDataType.dtString)
        '                                    .AddField("LastUpdate", ConsignlogCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
        '                                    .AddField("UpdateBy", ConsignlogCont.UpdateBy, SQLControl.EnumDataType.dtString)
        '                                    .AddField("Active", ConsignlogCont.Active, SQLControl.EnumDataType.dtNumeric)
        '                                    .AddField("TransID", ConsignlogCont.TransID, SQLControl.EnumDataType.dtString)
        '                                    .AddField("ContractNo", ConsignlogCont.ContractNo, SQLControl.EnumDataType.dtString)
        '                                    .AddField("GeneratorID", ConsignlogCont.GeneratorID, SQLControl.EnumDataType.dtString)
        '                                    .AddField("GeneratorLocID", ConsignlogCont.GeneratorLocID, SQLControl.EnumDataType.dtString)

        '                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                                End With
        '                            Next
        '                        End If
        '                        ListSQL.Add(strSQL)
        '                    End If

        '                    If Deduct Then
        '                        'execute store proc
        '                        strSQL = "Exec sp_LiveInvDeductByCN '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'"
        '                        ListSQL.Add(strSQL)
        '                    End If


        '                    'save employee
        '                    If EmployeeCont Is Nothing Then
        '                        'Message return
        '                    Else

        '                        blnExec = False
        '                        blnFound = False
        '                        blnFlag = False

        '                        Dim EmployeeInfo As Profiles.EmployeeInfo = New Profiles.EmployeeInfo
        '                        With EmployeeInfo.MyInfo
        '                            strSQL = BuildSelect(.CheckFields, .TableName, "NRICNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.NRICNo) & "'")
        '                        End With
        '                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
        '                        blnExec = True

        '                        If rdr Is Nothing = False Then
        '                            With rdr
        '                                If .Read Then
        '                                    blnFound = True
        '                                End If
        '                                .Close()
        '                            End With
        '                        End If

        '                        'Dim codeEmpID As String = RandomCode()
        '                        'codeEmpID = String.Format("{0:yyyyMMddHH}", Now) & codeEmpID

        '                        With objSQL
        '                            .TableName = "EMPLOYEE WITH (ROWLOCK)"
        '                            .AddField("NickName", EmployeeCont.NickName, SQLControl.EnumDataType.dtString) 'used
        '                            .AddField("NRICNo", EmployeeCont.NRICNo, SQLControl.EnumDataType.dtStringN) 'used
        '                            .AddField("Status", EmployeeCont.Status, SQLControl.EnumDataType.dtNumeric) 'used
        '                            .AddField("CreateDate", EmployeeCont.CreateDate, SQLControl.EnumDataType.dtDateTime) 'used
        '                            .AddField("CreateBy", EmployeeCont.CreateBy, SQLControl.EnumDataType.dtString) 'used
        '                            .AddField("CompanyID", EmployeeCont.CompanyID, SQLControl.EnumDataType.dtString) 'used
        '                            .AddField("locID", EmployeeCont.LocID, SQLControl.EnumDataType.dtString) 'used
        '                            .AddField("Flag", EmployeeCont.Flag, SQLControl.EnumDataType.dtNumeric) 'used
        '                            .AddField("Designation", EmployeeCont.Designation, SQLControl.EnumDataType.dtString) 'used

        '                            If blnFound = True Then
        '                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
        '                            Else
        '                                'EmployeeCont.EmployeeID = codeEmpID
        '                                .AddField("EmployeeID", EmployeeCont.EmployeeID, SQLControl.EnumDataType.dtString)
        '                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                            End If
        '                        End With
        '                        ListSQL.Add(strSQL)
        '                    End If

        '                    'save vehicle
        '                    If VehicleCont Is Nothing Then
        '                        'Message return
        '                    Else
        '                        blnExec = False
        '                        blnFound = False
        '                        blnFlag = False

        '                        objSQL.ClearFields()
        '                        'objSQL = Nothing
        '                        Dim VehicleInfo As Profiles.VehicleInfo = New Profiles.VehicleInfo
        '                        With VehicleInfo.MyInfo
        '                            strSQL = BuildSelect(.CheckFields, .TableName, "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleCont.VehicleID) & "'")
        '                        End With
        '                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
        '                        blnExec = True

        '                        If rdr Is Nothing = False Then
        '                            With rdr
        '                                If .Read Then
        '                                    blnFound = True
        '                                End If
        '                                .Close()
        '                            End With
        '                        End If

        '                        With objSQL
        '                            .TableName = "VEHICLE WITH (ROWLOCK)"
        '                            .AddField("RegNo", VehicleCont.RegNo, SQLControl.EnumDataType.dtString) 'used
        '                            .AddField("CompanyID", VehicleCont.CompanyID, SQLControl.EnumDataType.dtString) 'used
        '                            .AddField("CreateDate", VehicleCont.CreateDate, SQLControl.EnumDataType.dtDateTime) 'used
        '                            .AddField("CreateBy", VehicleCont.CreateBy, SQLControl.EnumDataType.dtString) 'used
        '                            .AddField("LastSyncBy", VehicleCont.LastSyncBy, SQLControl.EnumDataType.dtString) 'used
        '                            .AddField("Flag", VehicleCont.Flag, SQLControl.EnumDataType.dtString) 'used

        '                            If blnFound = True Then
        '                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleCont.VehicleID) & "'")
        '                            Else
        '                                .AddField("VehicleID", VehicleCont.VehicleID, SQLControl.EnumDataType.dtString)
        '                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                            End If
        '                        End With
        '                        ListSQL.Add(strSQL)
        '                    End If

        '                    Try
        '                        'execute
        '                        'objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
        '                        objConn.BatchExecute(ListSQL, CommandType.Text)
        '                        Return True
        '                    Catch axExecute As Exception
        '                        If pType = SQLControl.EnumSQLType.stInsert Then
        '                            message = axExecute.Message.ToString()
        '                            'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
        '                        Else
        '                            message = axExecute.Message.ToString()
        '                            'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
        '                        End If

        '                        Dim sqlStatement As String = " "
        '                        If objConn.FailedSQLStatement.Count > 0 Then
        '                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
        '                        End If

        '                        Log.Notifier.Notify(axExecute)
        '                        Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
        '                        Return False
        '                    Finally
        '                        objSQL.Dispose()
        '                    End Try
        '                End If
        '            End If
        '        End If
        '    Catch axAssign As ApplicationException
        '        'Throw axAssign
        '        message = axAssign.Message.ToString()
        '        Log.Notifier.Notify(axAssign)
        '        Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
        '        Return False
        '    Catch exAssign As SystemException
        '        'Throw exAssign
        '        message = exAssign.Message.ToString()
        '        Log.Notifier.Notify(exAssign)
        '        Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
        '        Return False
        '    Finally
        '        ConsignhdrCont = Nothing
        '        rdr = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        'function for rejected Consigment Note => Automatic Generate Inventory Adjustment
        'Public Function SaveConsignmentReceiver(ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal ConsignhdrCont As Actions.Container.Consignhdr, ByVal ListCNdtlCont As List(Of Actions.Container.Consigndtl), ByVal ContItmTransHDR As Actions.Container.Itmtranshdr, ByVal ContItmTransDtl As Actions.Container.Itmtransdtl, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal Reject As Boolean = False, Optional ByVal ContractNo As String = "", Optional ByVal ExecTWG As String = "0") As Boolean
        '    Dim strSQL As String = ""
        '    Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
        '    Dim rdr As System.Data.SqlClient.SqlDataReader
        '    Dim ListSQL As ArrayList = New ArrayList()
        '    SaveConsignmentReceiver = False

        '    Try
        '        If ConsignhdrCont Is Nothing OrElse ListCNdtlCont Is Nothing OrElse ListCNdtlCont.Count <= 0 Then
        '            'Message return
        '        Else
        '            blnExec = False
        '            blnFound = False
        '            blnFlag = False
        '            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                StartSQLControl()
        '                With ConsignhdrInfo.MyInfo
        '                    strSQL = BuildSelect(.CheckFields, .TableName, "TransID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")

        '                End With
        '                rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
        '                blnExec = True

        '                If rdr Is Nothing = False Then
        '                    With rdr
        '                        If .Read Then
        '                            blnFound = True
        '                            If Convert.ToInt16(.Item("Flag")) = 0 Then
        '                                'Found but deleted
        '                                blnFlag = False
        '                            Else
        '                                'Found and active
        '                                blnFlag = True
        '                            End If
        '                        End If
        '                        .Close()
        '                    End With
        '                End If
        '            End If

        '            If blnExec Then
        '                If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
        '                    message = "Record already exist"
        '                    'Throw New ApplicationException("210011: Record already exist")
        '                    'Item found & active
        '                    'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIconInformation,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
        '                    'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
        '                    Return False
        '                Else
        '                    StartSQLControl()

        '                    'save header
        '                    With objSQL
        '                        .TableName = "CONSIGNHDR WITH (ROWLOCK)"
        '                        .AddField("ReferID", ConsignhdrCont.ReferID, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransType", ConsignhdrCont.TransType, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransDate", ConsignhdrCont.TransDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("Status", ConsignhdrCont.Status, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("TransportStatus", ConsignhdrCont.TransportStatus, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("ReceiveStatus", ConsignhdrCont.ReceiveStatus, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("GeneratorID", ConsignhdrCont.GeneratorID, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorLocID", ConsignhdrCont.GeneratorLocID, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorAddress", ConsignhdrCont.GeneratorAddress, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorPIC", ConsignhdrCont.GeneratorPIC, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("GeneratorPos", ConsignhdrCont.GeneratorPos, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("GeneratorTelNo", ConsignhdrCont.GeneratorTelNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorFaxNo", ConsignhdrCont.GeneratorFaxNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorEmailAddress", ConsignhdrCont.GeneratorEmailAddress, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("TransporterID", ConsignhdrCont.TransporterID, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterLocID", ConsignhdrCont.TransporterLocID, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterAddress", ConsignhdrCont.TransporterAddress, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterPIC", ConsignhdrCont.TransporterPIC, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("TransporterPos", ConsignhdrCont.TransporterPos, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("TransporterTelNo", ConsignhdrCont.TransporterTelNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterFaxNo", ConsignhdrCont.TransporterFaxNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterEmailAddress", ConsignhdrCont.TransporterEmailAddress, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("VehicleID", ConsignhdrCont.VehicleID, SQLControl.EnumDataType.dtString)
        '                        .AddField("DriverID", ConsignhdrCont.DriverID, SQLControl.EnumDataType.dtString)
        '                        .AddField("DriverName", ConsignhdrCont.DriverName, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ICNo", ConsignhdrCont.ICNo, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ReceiverID", ConsignhdrCont.ReceiverID, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverLocID", ConsignhdrCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverAddress", ConsignhdrCont.ReceiverAddress, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverPIC", ConsignhdrCont.ReceiverPIC, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ReceiverPos", ConsignhdrCont.ReceiverPos, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ReceiverTelNo", ConsignhdrCont.ReceiverTelNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverFaxNo", ConsignhdrCont.ReceiverFaxNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverEmailAddress", ConsignhdrCont.ReceiverEmailAddress, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("PremiseID", ConsignhdrCont.PremiseID, SQLControl.EnumDataType.dtString)
        '                        .AddField("TempStorage1", ConsignhdrCont.TempStorage1, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("TempStorage2", ConsignhdrCont.TempStorage2, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("DeliveryDate", ConsignhdrCont.DeliveryDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("TargetTransportDate", ConsignhdrCont.TargetTransportDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("TransportDate", ConsignhdrCont.TransportDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("TargetReceiveDate", ConsignhdrCont.TargetReceiveDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("ReceiveDate", ConsignhdrCont.ReceiveDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("DeliveryRemark", ConsignhdrCont.DeliveryRemark, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ReceiveRemark", ConsignhdrCont.ReceiveRemark, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("RejectRemark", ConsignhdrCont.RejectRemark, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("CreateDate", ConsignhdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("CreateBy", ConsignhdrCont.CreateBy, SQLControl.EnumDataType.dtString)
        '                        .AddField("LastUpdate", ConsignhdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("UpdateBy", ConsignhdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
        '                        '.AddField("rowguid", ConsignhdrCont.rowguid, SQLControl.EnumDataType.dtString)
        '                        .AddField("IsHost", ConsignhdrCont.IsHost, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("IsConfirm", ConsignhdrCont.IsConfirm, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("IsNew", ConsignhdrCont.IsNew, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("LastSyncBy", ConsignhdrCont.LastSyncBy, SQLControl.EnumDataType.dtString)
        '                        .AddField("Active", ConsignhdrCont.Active, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("Flag", ConsignhdrCont.Flag, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("LiveCal", ConsignhdrCont.LiveCal, SQLControl.EnumDataType.dtNumeric)

        '                        Select Case pType
        '                            Case SQLControl.EnumSQLType.stInsert
        '                                If blnFound = True And blnFlag = False Then
        '                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                                Else
        '                                    If blnFound = False Then
        '                                        .AddField("TransID", ConsignhdrCont.TransID, SQLControl.EnumDataType.dtString)
        '                                        .AddField("ContractNo", ConsignhdrCont.ContractNo, SQLControl.EnumDataType.dtString)
        '                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                                    End If
        '                                End If
        '                            Case SQLControl.EnumSQLType.stUpdate
        '                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                        End Select

        '                        ListSQL.Add(strSQL)
        '                    End With

        '                    'save all details
        '                    For Each CNDtlCont In ListCNdtlCont
        '                        With objSQL
        '                            .TableName = "CONSIGNDTL WITH (ROWLOCK)"

        '                            .AddField("WasteCode", CNDtlCont.WasteCode, SQLControl.EnumDataType.dtString)
        '                            .AddField("WasteDescription", CNDtlCont.WasteDescription, SQLControl.EnumDataType.dtString)
        '                            .AddField("WasteComponent", CNDtlCont.WasteComponent, SQLControl.EnumDataType.dtString)
        '                            .AddField("WasteType", CNDtlCont.WasteType, SQLControl.EnumDataType.dtString)
        '                            .AddField("WastePackage", CNDtlCont.WastePackage, SQLControl.EnumDataType.dtString)
        '                            .AddField("OriginCode", CNDtlCont.OriginCode, SQLControl.EnumDataType.dtString)
        '                            .AddField("OriginDescription", CNDtlCont.OriginDescription, SQLControl.EnumDataType.dtString)
        '                            .AddField("SerialNo", CNDtlCont.SerialNo, SQLControl.EnumDataType.dtString)
        '                            .AddField("Qty", CNDtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("PackQty", CNDtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("StorageID", CNDtlCont.StorageID, SQLControl.EnumDataType.dtString)
        '                            .AddField("CountryCode", CNDtlCont.CountryCode, SQLControl.EnumDataType.dtString)
        '                            .AddField("PackagingQty", CNDtlCont.PackagingQty, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("PackagingQtyKg", CNDtlCont.PackagingQtyKg, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("RcvQty", CNDtlCont.RcvQty, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("RcvPackQty", CNDtlCont.RcvPackQty, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("TreatmentCost", CNDtlCont.TreatmentCost, SQLControl.EnumDataType.dtNumeric)
        '                            .AddField("Remark", CNDtlCont.Remark, SQLControl.EnumDataType.dtString)
        '                            .AddField("OperationType", CNDtlCont.OperationType, SQLControl.EnumDataType.dtString)

        '                            '.AddField("RowGuid", ItmTransDtlCont.RowGuid, SQLControl.EnumDataType.dtString)
        '                            .AddField("CreateDate", CNDtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
        '                            .AddField("CreateBy", CNDtlCont.CreateBy, SQLControl.EnumDataType.dtString)
        '                            .AddField("AuthorisedBy", CNDtlCont.AuthorisedBy, SQLControl.EnumDataType.dtString)
        '                            .AddField("AuthorisedDate", CNDtlCont.AuthorisedDate, SQLControl.EnumDataType.dtDateTime)
        '                            .AddField("LastSyncBy", CNDtlCont.LastSyncBy, SQLControl.EnumDataType.dtString)
        '                            .AddField("LastUpdate", CNDtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
        '                            .AddField("UpdateBy", CNDtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

        '                            .AddField("ReceivedDate", CNDtlCont.ReceivedDate, SQLControl.EnumDataType.dtDateTime)
        '                            .AddField("ReceivedBy", CNDtlCont.ReceivedBy, SQLControl.EnumDataType.dtString)

        '                            Select Case pType
        '                                Case SQLControl.EnumSQLType.stInsert
        '                                    If blnFound = True And blnFlag = False Then
        '                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CNDtlCont.ContractNo) & "' AND SeqNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, CNDtlCont.SeqNo) & "'")
        '                                    Else
        '                                        If blnFound = False Then
        '                                            .AddField("TransID", CNDtlCont.TransID, SQLControl.EnumDataType.dtString)
        '                                            .AddField("ContractNo", CNDtlCont.ContractNo, SQLControl.EnumDataType.dtString)
        '                                            .AddField("SeqNo", CNDtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
        '                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                                        End If
        '                                    End If
        '                                Case SQLControl.EnumSQLType.stUpdate
        '                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CNDtlCont.ContractNo) & "' AND SeqNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, CNDtlCont.SeqNo) & "'")
        '                            End Select

        '                        End With

        '                        ListSQL.Add(strSQL)
        '                    Next

        '                    If Reject Then 'Save Inventory Adjustment
        '                        If ContItmTransHDR Is Nothing Then
        '                            'Message return
        '                        Else
        '                            blnExec = False
        '                            blnFound = False
        '                            blnFlag = False

        '                            Dim ItmtranshdrInfo As Actions.ItmtranshdrInfo = New Actions.ItmtranshdrInfo
        '                            With ItmtranshdrInfo.MyInfo
        '                                strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ContItmTransHDR.DocCode) & "' and TransType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ContItmTransHDR.TransType) & "' and posted = 0")
        '                            End With
        '                            rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
        '                            blnExec = True

        '                            If rdr Is Nothing = False Then
        '                                With rdr
        '                                    If .Read Then
        '                                        blnFound = True
        '                                        strSQL = "DELETE ITMTRANSHDR WITH (ROWLOCK) WHERE DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ContItmTransHDR.DocCode) & "' and TransType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ContItmTransHDR.TransType) & "' and posted = 0"
        '                                        ListSQL.Add(strSQL)
        '                                    End If
        '                                    .Close()
        '                                End With
        '                            End If

        '                            With objSQL
        '                                .TableName = "ITMTRANSHDR  WITH (ROWLOCK)"
        '                                .AddField("LocID", ContItmTransHDR.LocID, SQLControl.EnumDataType.dtString)
        '                                .AddField("RequestCode", ContItmTransHDR.RequestCode, SQLControl.EnumDataType.dtString)
        '                                .AddField("BatchCode", ContItmTransHDR.BatchCode, SQLControl.EnumDataType.dtString)
        '                                .AddField("TermID", ContItmTransHDR.TermID, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("RegistedSupp", ContItmTransHDR.RegistedSupp, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("CompanyID", ContItmTransHDR.CompanyID, SQLControl.EnumDataType.dtString)
        '                                .AddField("CompanyName", ContItmTransHDR.CompanyName, SQLControl.EnumDataType.dtStringN)
        '                                .AddField("CompanyType", ContItmTransHDR.CompanyType, SQLControl.EnumDataType.dtString)
        '                                .AddField("TransType", ContItmTransHDR.TransType, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("TransDate", ContItmTransHDR.TransDate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("TransInit", ContItmTransHDR.TransInit, SQLControl.EnumDataType.dtString)
        '                                .AddField("TransSrc", ContItmTransHDR.TransSrc, SQLControl.EnumDataType.dtString)
        '                                .AddField("TransDest", ContItmTransHDR.TransDest, SQLControl.EnumDataType.dtString)
        '                                .AddField("InterTrans", ContItmTransHDR.InterTrans, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("Remark", ContItmTransHDR.Remark, SQLControl.EnumDataType.dtStringN)
        '                                .AddField("Reason", ContItmTransHDR.Reason, SQLControl.EnumDataType.dtStringN)
        '                                .AddField("PostDate", ContItmTransHDR.PostDate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("Posted", ContItmTransHDR.Posted, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("Status", ContItmTransHDR.Status, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("ConfirmDate", ContItmTransHDR.ConfirmDate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("AuthID", ContItmTransHDR.AuthID, SQLControl.EnumDataType.dtString)
        '                                .AddField("AuthPOS", ContItmTransHDR.AuthPOS, SQLControl.EnumDataType.dtStringN)
        '                                .AddField("CreateDate", ContItmTransHDR.CreateDate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("CreateBy", ContItmTransHDR.CreateBy, SQLControl.EnumDataType.dtString)
        '                                .AddField("ApproveDate", ContItmTransHDR.ApproveDate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("ApproveBy", ContItmTransHDR.ApproveBy, SQLControl.EnumDataType.dtString)
        '                                .AddField("LastUpdate", ContItmTransHDR.LastUpdate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("UpdateBy", ContItmTransHDR.UpdateBy, SQLControl.EnumDataType.dtString)
        '                                .AddField("Active", ContItmTransHDR.Active, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("Inuse", ContItmTransHDR.Inuse, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("LiveCal", ContItmTransHDR.LiveCal, SQLControl.EnumDataType.dtNumeric)

        '                                .AddField("DocCode", ContItmTransHDR.DocCode, SQLControl.EnumDataType.dtString)

        '                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                            End With
        '                            ListSQL.Add(strSQL)
        '                        End If
        '                        If ContItmTransDtl Is Nothing Then
        '                            'Message return
        '                        Else
        '                            With objSQL

        '                                .TableName = "ITMTRANSDTL WITH (ROWLOCK)"
        '                                .AddField("StorageID", ContItmTransDtl.StorageID, SQLControl.EnumDataType.dtString)
        '                                .AddField("TermID", ContItmTransDtl.TermID, SQLControl.EnumDataType.dtCustom)
        '                                .AddField("SeqNo", ContItmTransDtl.SeqNo, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("Status", ContItmTransDtl.Status, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("TransType", ContItmTransDtl.TransType, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("OperationType", ContItmTransDtl.OperationType, SQLControl.EnumDataType.dtString)
        '                                .AddField("ItmPrice", ContItmTransDtl.ItmPrice, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("ReqQty", ContItmTransDtl.ReqQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("ReqPackQty", ContItmTransDtl.ReqPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("OpeningQty", ContItmTransDtl.OpeningQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("OpeningPackQty", ContItmTransDtl.OpeningPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("Qty", ContItmTransDtl.Qty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("PackQty", ContItmTransDtl.PackQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("HandlingQty", ContItmTransDtl.HandlingQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("HandlingPackQty", ContItmTransDtl.HandlingPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("ClosingQty", ContItmTransDtl.ClosingQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("ClosingPackQty", ContItmTransDtl.ClosingPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("LastInQty", ContItmTransDtl.LastInQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("LastInPackQty", ContItmTransDtl.LastInPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("LastOutQty", ContItmTransDtl.LastOutQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("LastOutPackQty", ContItmTransDtl.LastOutPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("RtnPackQty", ContItmTransDtl.RtnPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("RtnQty", ContItmTransDtl.RtnQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("RecPackQty", ContItmTransDtl.RecPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("RecQty", ContItmTransDtl.RecQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("ExpiredQty", ContItmTransDtl.ExpiredQty, SQLControl.EnumDataType.dtNumeric)
        '                                .AddField("ExpiryDate", ContItmTransDtl.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("Remark", ContItmTransDtl.Remark, SQLControl.EnumDataType.dtStringN)
        '                                .AddField("LotNo", ContItmTransDtl.LotNo, SQLControl.EnumDataType.dtStringN)
        '                                .AddField("CityCode", ContItmTransDtl.CityCode, SQLControl.EnumDataType.dtStringN)
        '                                .AddField("SecCode", ContItmTransDtl.SecCode, SQLControl.EnumDataType.dtStringN)
        '                                .AddField("BinCode", ContItmTransDtl.BinCode, SQLControl.EnumDataType.dtStringN)
        '                                '.AddField("RowGuid", ItmTransDtlCont.RowGuid, SQLControl.EnumDataType.dtString)
        '                                .AddField("CreateDate", ContItmTransDtl.CreateDate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("CreateBy", ContItmTransDtl.CreateBy, SQLControl.EnumDataType.dtString)
        '                                .AddField("LastUpdate", ContItmTransDtl.LastUpdate, SQLControl.EnumDataType.dtDateTime)
        '                                .AddField("UpdateBy", ContItmTransDtl.UpdateBy, SQLControl.EnumDataType.dtString)

        '                                .AddField("DocCode", ContItmTransDtl.DocCode, SQLControl.EnumDataType.dtString)
        '                                .AddField("ItemCode", ContItmTransDtl.ItemCode, SQLControl.EnumDataType.dtString)
        '                                .AddField("ItemName", ContItmTransDtl.ItemName, SQLControl.EnumDataType.dtString)
        '                                .AddField("LocID", ContItmTransDtl.LocID, SQLControl.EnumDataType.dtString)

        '                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                            End With

        '                            ListSQL.Add(strSQL)
        '                        End If

        '                        strSQL = "Exec sp_LiveInv '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ContItmTransHDR.DocCode) & "'"

        '                        ListSQL.Add(strSQL)


        '                    Else
        '                        'for TWG
        '                        If ExecTWG = "1" Then
        '                            If ContItmTransHDR Is Nothing OrElse ListItmtransdtlCont Is Nothing Then

        '                                'Message return
        '                            Else
        '                                If ContItmTransHDR.DocCode IsNot Nothing Then
        '                                    blnExec = False
        '                                    blnFound = False
        '                                    blnFlag = False
        '                                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                                        StartSQLControl()
        '                                        With ItmtranshdrInfo.MyInfo
        '                                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ContItmTransHDR.DocCode) & "'")
        '                                        End With
        '                                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
        '                                        blnExec = True

        '                                        If rdr Is Nothing = False Then
        '                                            With rdr
        '                                                If .Read Then
        '                                                    blnFound = True
        '                                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
        '                                                        'Found but deleted
        '                                                        blnFlag = False
        '                                                    Else
        '                                                        'Found and active
        '                                                        blnFlag = True
        '                                                    End If
        '                                                End If
        '                                                .Close()
        '                                            End With
        '                                        End If
        '                                    End If

        '                                    If blnExec Then
        '                                        If blnFlag = True And blnFound = True Then
        '                                            message = "Record already exist"
        '                                            'Throw New ApplicationException("210011: Record already exist")

        '                                            Return False
        '                                        Else
        '                                            StartSQLControl()

        '                                            'save header
        '                                            With objSQL
        '                                                .TableName = "ITMTRANSHDR WITH (ROWLOCK)"
        '                                                .AddField("LocID", ContItmTransHDR.LocID, SQLControl.EnumDataType.dtString)
        '                                                .AddField("RequestCode", ContItmTransHDR.RequestCode, SQLControl.EnumDataType.dtString)
        '                                                .AddField("BatchCode", ContItmTransHDR.BatchCode, SQLControl.EnumDataType.dtString)
        '                                                .AddField("TermID", ContItmTransHDR.TermID, SQLControl.EnumDataType.dtNumeric)
        '                                                .AddField("RegistedSupp", ContItmTransHDR.RegistedSupp, SQLControl.EnumDataType.dtNumeric)
        '                                                .AddField("CompanyID", ContItmTransHDR.CompanyID, SQLControl.EnumDataType.dtString)
        '                                                .AddField("CompanyName", ContItmTransHDR.CompanyName, SQLControl.EnumDataType.dtString)
        '                                                .AddField("CompanyType", ContItmTransHDR.CompanyType, SQLControl.EnumDataType.dtString)
        '                                                .AddField("TransType", ContItmTransHDR.TransType, SQLControl.EnumDataType.dtNumeric)
        '                                                .AddField("TransDate", ContItmTransHDR.TransDate, SQLControl.EnumDataType.dtDateTime)
        '                                                .AddField("TransInit", ContItmTransHDR.TransInit, SQLControl.EnumDataType.dtString)
        '                                                .AddField("TransSrc", ContItmTransHDR.TransSrc, SQLControl.EnumDataType.dtString)
        '                                                .AddField("TransDest", ContItmTransHDR.TransDest, SQLControl.EnumDataType.dtString)
        '                                                .AddField("InterTrans", ContItmTransHDR.InterTrans, SQLControl.EnumDataType.dtNumeric)
        '                                                .AddField("Remark", ContItmTransHDR.Remark, SQLControl.EnumDataType.dtString)
        '                                                .AddField("Reason", ContItmTransHDR.Reason, SQLControl.EnumDataType.dtString)
        '                                                .AddField("PostDate", ContItmTransHDR.PostDate, SQLControl.EnumDataType.dtDateTime)
        '                                                .AddField("Posted", ContItmTransHDR.Posted, SQLControl.EnumDataType.dtNumeric)
        '                                                .AddField("Status", ContItmTransHDR.Status, SQLControl.EnumDataType.dtNumeric)
        '                                                .AddField("ConfirmDate", ContItmTransHDR.ConfirmDate, SQLControl.EnumDataType.dtDateTime)
        '                                                '.AddField("ExpiredDate", ItmtranshdrCont.ExpiredDate, SQLControl.EnumDataType.dtDateTime)
        '                                                .AddField("AuthID", ContItmTransHDR.AuthID, SQLControl.EnumDataType.dtString)
        '                                                .AddField("AuthPOS", ContItmTransHDR.AuthPOS, SQLControl.EnumDataType.dtString)
        '                                                .AddField("CreateDate", ContItmTransHDR.CreateDate, SQLControl.EnumDataType.dtDateTime)
        '                                                .AddField("CreateBy", ContItmTransHDR.CreateBy, SQLControl.EnumDataType.dtString)
        '                                                .AddField("ApproveDate", ContItmTransHDR.ApproveDate, SQLControl.EnumDataType.dtDateTime)
        '                                                .AddField("ApproveBy", ContItmTransHDR.ApproveBy, SQLControl.EnumDataType.dtString)
        '                                                .AddField("LastUpdate", ContItmTransHDR.LastUpdate, SQLControl.EnumDataType.dtDateTime)
        '                                                .AddField("UpdateBy", ContItmTransHDR.UpdateBy, SQLControl.EnumDataType.dtString)
        '                                                .AddField("Active", ContItmTransHDR.Active, SQLControl.EnumDataType.dtNumeric)
        '                                                .AddField("Inuse", ContItmTransHDR.Inuse, SQLControl.EnumDataType.dtNumeric)
        '                                                .AddField("LiveCal", ContItmTransHDR.LiveCal, SQLControl.EnumDataType.dtNumeric)
        '                                                '.AddField("RowGuid", ItmtranshdrCont.RowGuid, SQLControl.EnumDataType.dtString)
        '                                                .AddField("DocCode", ContItmTransHDR.DocCode, SQLControl.EnumDataType.dtString)
        '                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)

        '                                                ListSQL.Add(strSQL)

        '                                            End With

        '                                            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                                                StartSQLControl()

        '                                                If ListItmtransdtlCont.Count > 0 Then
        '                                                    With objSQL
        '                                                        .TableName = "ITMTRANSDTL WITH (ROWLOCK)"
        '                                                        strSQL = BuildDelete(.TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListItmtransdtlCont(0).DocCode) & "'")
        '                                                        ListSQL.Add(strSQL)
        '                                                    End With
        '                                                End If

        '                                            End If

        '                                            For Each ItmTransDtlCont In ListItmtransdtlCont
        '                                                With objSQL

        '                                                    .TableName = "ITMTRANSDTL WITH (ROWLOCK)"
        '                                                    .AddField("StorageID", ItmTransDtlCont.StorageID, SQLControl.EnumDataType.dtString)
        '                                                    .AddField("TermID", ItmTransDtlCont.TermID, SQLControl.EnumDataType.dtCustom)
        '                                                    .AddField("SeqNo", ItmTransDtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("Status", ItmTransDtlCont.Status, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("TransType", ItmTransDtlCont.TransType, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("OperationType", ItmTransDtlCont.OperationType, SQLControl.EnumDataType.dtString)
        '                                                    .AddField("ItmPrice", ItmTransDtlCont.ItmPrice, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("ReqQty", ItmTransDtlCont.ReqQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("ReqPackQty", ItmTransDtlCont.ReqPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("OpeningQty", ItmTransDtlCont.OpeningQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("OpeningPackQty", ItmTransDtlCont.OpeningPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("Qty", ItmTransDtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("PackQty", ItmTransDtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("HandlingQty", ItmTransDtlCont.HandlingQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("HandlingPackQty", ItmTransDtlCont.HandlingPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("ClosingQty", ItmTransDtlCont.ClosingQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("ClosingPackQty", ItmTransDtlCont.ClosingPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("LastInQty", ItmTransDtlCont.LastInQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("LastInPackQty", ItmTransDtlCont.LastInPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("LastOutQty", ItmTransDtlCont.LastOutQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("LastOutPackQty", ItmTransDtlCont.LastOutPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("RtnPackQty", ItmTransDtlCont.RtnPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("RtnQty", ItmTransDtlCont.RtnQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("RecPackQty", ItmTransDtlCont.RecPackQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("RecQty", ItmTransDtlCont.RecQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("ExpiredQty", ItmTransDtlCont.ExpiredQty, SQLControl.EnumDataType.dtNumeric)
        '                                                    .AddField("ExpiryDate", ItmTransDtlCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
        '                                                    .AddField("Remark", ItmTransDtlCont.Remark, SQLControl.EnumDataType.dtString)
        '                                                    .AddField("LotNo", ItmTransDtlCont.LotNo, SQLControl.EnumDataType.dtString)
        '                                                    .AddField("CityCode", ItmTransDtlCont.CityCode, SQLControl.EnumDataType.dtString)
        '                                                    .AddField("SecCode", ItmTransDtlCont.SecCode, SQLControl.EnumDataType.dtString)
        '                                                    .AddField("BinCode", ItmTransDtlCont.BinCode, SQLControl.EnumDataType.dtString)
        '                                                    '.AddField("RowGuid", ItmTransDtlCont.RowGuid, SQLControl.EnumDataType.dtString)
        '                                                    .AddField("CreateDate", ItmTransDtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
        '                                                    .AddField("CreateBy", ItmTransDtlCont.CreateBy, SQLControl.EnumDataType.dtString)
        '                                                    .AddField("LastUpdate", ItmTransDtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
        '                                                    .AddField("UpdateBy", ItmTransDtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

        '                                                    .AddField("DocCode", ItmTransDtlCont.DocCode, SQLControl.EnumDataType.dtString)
        '                                                    .AddField("ItemCode", ItmTransDtlCont.ItemCode, SQLControl.EnumDataType.dtString)
        '                                                    .AddField("ItemName", ItmTransDtlCont.ItemName, SQLControl.EnumDataType.dtString)
        '                                                    .AddField("LocID", ItmTransDtlCont.LocID, SQLControl.EnumDataType.dtString)
        '                                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)

        '                                                End With

        '                                                ListSQL.Add(strSQL)
        '                                            Next

        '                                            'save itemLoc
        '                                            StartSQLControl()
        '                                            'execute store proc
        '                                            strSQL = "Exec sp_LiveInv '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ContItmTransHDR.DocCode) & "'"
        '                                            ListSQL.Add(strSQL)
        '                                        End If



        '                                    End If

        '                                End If
        '                            End If
        '                            strSQL = "Exec sp_LiveInvDeductByCN '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ContractNo) & "'"

        '                            ListSQL.Add(strSQL)
        '                        End If
        '                        'for TWG
        '                    End If

        '                    Try
        '                        objConn.BatchExecute(ListSQL, CommandType.Text)
        '                        Return True
        '                    Catch axExecute As Exception
        '                        If pType = SQLControl.EnumSQLType.stInsert Then
        '                            message = axExecute.Message.ToString()
        '                            'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
        '                        Else
        '                            message = axExecute.Message.ToString()
        '                            'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
        '                        End If

        '                        Dim sqlStatement As String = " "
        '                        If objConn.FailedSQLStatement.Count > 0 Then
        '                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
        '                        End If

        '                        Log.Notifier.Notify(axExecute)
        '                        Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
        '                        Return False
        '                    Finally
        '                        objSQL.Dispose()
        '                    End Try
        '                End If
        '            End If
        '        End If
        '    Catch axAssign As ApplicationException
        '        'Throw axAssign
        '        message = axAssign.Message.ToString()
        '        Log.Notifier.Notify(axAssign)
        '        Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
        '        Return False
        '    Catch exAssign As SystemException
        '        'Throw exAssign
        '        message = exAssign.Message.ToString()
        '        Log.Notifier.Notify(exAssign)
        '        Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
        '        Return False
        '    Finally
        '        ConsignhdrCont = Nothing
        '        rdr = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        'ADD
        'Public Function InsertConsignment(ByVal ConsignhdrCont As Actions.Container.Consignhdr, ByVal ListCNdtlCont As List(Of Actions.Container.Consigndtl), ByVal EmployeeCont As Profiles.Container.Employee, ByVal VehicleCont As Profiles.Container.Vehicle, ByVal ListCNLblCont As List(Of Actions.Container.Consignlabel), ByVal ListCNLogCont As List(Of Actions.Container.Consignlog), ByRef message As String, Optional ByVal Deduct As Boolean = False, Optional ByVal isBackdated As Boolean = False, Optional ByVal QtyDeletelabel As Integer = 0, Optional ByVal MaxSeqno As Integer = 0) As Boolean
        '    Return SaveConsignment(ConsignhdrCont, ListCNdtlCont, EmployeeCont, VehicleCont, ListCNLblCont, ListCNLogCont, SQLControl.EnumSQLType.stInsert, message, Deduct, isBackdated, QtyDeletelabel, MaxSeqno)
        'End Function

        ''AMEND
        'Public Function UpdateConsignment(ByVal ConsignhdrCont As Actions.Container.Consignhdr, ByVal ListCNdtlCont As List(Of Actions.Container.Consigndtl), ByVal EmployeeCont As Profiles.Container.Employee, ByVal VehicleCont As Profiles.Container.Vehicle, ByVal ListCNLblCont As List(Of Actions.Container.Consignlabel), ByVal ListCNLogCont As List(Of Actions.Container.Consignlog), ByRef message As String, Optional ByVal Deduct As Boolean = False, Optional ByVal isBackDated As Boolean = False, Optional ByVal QtyDeletelabel As Integer = 0, Optional ByVal MaxSeqno As Integer = 0) As Boolean
        '    Return SaveConsignment(ConsignhdrCont, ListCNdtlCont, EmployeeCont, VehicleCont, ListCNLblCont, ListCNLogCont, SQLControl.EnumSQLType.stUpdate, message, Deduct, isBackDated, QtyDeletelabel, MaxSeqno)
        'End Function

        'Public Function UpdateConsignmentReceiver(ByVal ListItmtransdtlCont As List(Of Actions.Container.Itmtransdtl), ByVal ConsignhdrCont As Actions.Container.Consignhdr, ByVal ListCNdtlCont As List(Of Actions.Container.Consigndtl), ByVal ContItmTransHDR As Actions.Container.Itmtranshdr, ByVal ContItmTransDtl As Actions.Container.Itmtransdtl, ByRef message As String, Optional ByVal Deduct As Boolean = False, Optional ByVal ContractNo As String = "", Optional ByVal ExecTWG As String = "") As Boolean
        '    Return SaveConsignmentReceiver(ListItmtransdtlCont, ConsignhdrCont, ListCNdtlCont, ContItmTransHDR, ContItmTransDtl, SQLControl.EnumSQLType.stUpdate, message, Deduct, ContractNo, ExecTWG)
        'End Function
        'end function for Consignment Note

        'Public Function Delete(ByVal ConsignhdrCont As Container.Consignhdr, ByRef message As String) As Boolean
        '    Dim strSQL As String
        '    Dim blnFound As Boolean
        '    Dim blnInUse As Boolean
        '    Dim rdr As System.Data.SqlClient.SqlDataReader
        '    Delete = False
        '    blnFound = False
        '    blnInUse = False
        '    Try
        '        If ConsignhdrCont Is Nothing Then
        '            'Error Message
        '        Else
        '            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                StartSQLControl()
        '                With ConsignhdrInfo.MyInfo
        '                    strSQL = BuildSelect(.CheckFields, .TableName, "TransID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                End With
        '                rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

        '                If rdr Is Nothing = False Then
        '                    With rdr
        '                        If .Read Then
        '                            blnFound = True
        '                            If Convert.ToInt16(.Item("InUse")) = 1 Then
        '                                blnInUse = True
        '                            End If
        '                        End If
        '                        .Close()
        '                    End With
        '                End If

        '                If blnFound = True And blnInUse = True Then
        '                    With objSQL
        '                        strSQL = BuildUpdate(ConsignhdrInfo.MyInfo.TableName, " SET Flag = 0" & _
        '                        " , LastUpdate = '" & ConsignhdrCont.LastUpdate & "' , UpdateBy = '" & _
        '                        objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.UpdateBy) & "' WHERE" & _
        '                        "TransID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                    End With
        '                End If

        '                If blnFound = True And blnInUse = False Then
        '                    strSQL = BuildDelete(ConsignhdrInfo.MyInfo.TableName, "TransID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                End If

        '                Try
        '                    'execute
        '                    objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
        '                    Return True
        '                Catch exExecute As Exception
        '                    message = exExecute.Message.ToString()
        '                    Return False
        '                    'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
        '                End Try
        '            End If
        '        End If

        '    Catch axDelete As ApplicationException
        '        message = axDelete.Message.ToString()
        '        Return False
        '        'Throw axDelete
        '    Catch exDelete As Exception
        '        message = exDelete.Message.ToString()
        '        Return False
        '        'Throw exDelete
        '    Finally
        '        ConsignhdrCont = Nothing
        '        rdr = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        ''Add Cancel
        'Public Function Cancel(ByVal CNhdrCont As Container.Consignhdr, ByRef message As String) As Boolean
        '    Dim strSQL As String
        '    Dim blnFound As Boolean
        '    Dim blnInUse As Boolean
        '    Dim rdr As System.Data.SqlClient.SqlDataReader
        '    Cancel = False
        '    blnFound = False
        '    blnInUse = False
        '    Try
        '        If CNhdrCont Is Nothing Then
        '            'Error Message
        '        Else
        '            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                StartSQLControl()
        '                With ConsignhdrInfo.MyInfo
        '                    strSQL = BuildSelect(.CheckFields, .TableName, "ContractNo = '" & CNhdrCont.ContractNo & "'")
        '                End With
        '                rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

        '                If rdr Is Nothing = False Then
        '                    With rdr
        '                        If .Read Then
        '                            blnFound = True
        '                            If Convert.ToInt16(.Item("InUse")) = 1 Then
        '                                blnInUse = True
        '                            End If
        '                        End If
        '                        .Close()
        '                    End With
        '                End If

        '                'If blnFound = True And blnInUse = True Then
        '                If blnFound = True Then
        '                    With objSQL
        '                        'If Cancel then set Status to 2 (Canceled)
        '                        strSQL = BuildUpdate(ConsignhdrInfo.MyInfo.TableName, " SET Status = 2, Flag = 0" & _
        '                        " , LastUpdate = '" & CNhdrCont.LastUpdate & "' , UpdateBy = '" & _
        '                        objSQL.ParseValue(SQLControl.EnumDataType.dtString, CNhdrCont.UpdateBy) & "' WHERE " & _
        '                        "ContractNo = '" & CNhdrCont.ContractNo & "'")
        '                    End With
        '                End If

        '                Try
        '                    'execute header
        '                    objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

        '                    Return True
        '                Catch exExecute As Exception
        '                    message = exExecute.Message.ToString()
        '                    Return False

        '                End Try
        '            End If
        '        End If

        '    Catch axDelete As ApplicationException
        '        message = axDelete.Message.ToString()
        '        Return False
        '        'Throw axDelete
        '    Catch exDelete As Exception
        '        message = exDelete.Message.ToString()
        '        Return False
        '        'Throw exDelete
        '    Finally
        '        CNhdrCont = Nothing
        '        rdr = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        'added by Antoni Bernad 10272014 - BatchExecute Registration

        Public Function SaveRegistration(ByVal CompanyCont As Profiles.Container.Company, ByVal BizlocateCont As Profiles.Container.Bizlocate, ByVal UsrprofileCont As UserSecurity.Container.UserProfile, ByVal USRAPP As UserSecurity.UsrApp, ByVal EmployeeCont As Profiles.Container.Employee, ByVal EmployeeUsrprofileCont As UserSecurity.Container.UserProfile, ByVal EMPLOYEEUSRAPP As UserSecurity.UsrApp, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, ByVal EmployeeBranchCont As Profiles.Container.Empbranch) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveRegistration = False

            Try
                If CompanyCont Is Nothing OrElse BizlocateCont Is Nothing Then

                Else
                    blnExec = True
                    blnFound = False
                    blnFlag = False
                    'If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    '    StartSQLControl()

                    '    With CompanyInfo.MyInfo
                    '        strSQL = BuildSelect(.CheckFields, .TableName, "RegNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.RegNo) & "'")
                    '    End With
                    '    rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                    '    blnExec = True

                    '    If rdr Is Nothing = False Then
                    '        With rdr
                    '            If .Read Then
                    '                blnFound = True
                    '                blnFlag = False
                    '            End If
                    '            .Close()
                    '        End With
                    '    End If

                    '    'With CompanyInfo.MyInfo
                    '    '    strSQL = BuildSelect(.CheckFields, .TableName, "BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.BizRegID) & "'")
                    '    'End With
                    '    'rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                    '    'blnExec = True

                    '    'If rdr Is Nothing = False Then
                    '    '    With rdr
                    '    '        If .Read Then
                    '    '            blnFound = True
                    '    '            If Convert.ToInt16(.Item("Flag")) = 0 Then
                    '    '                'Found but deleted
                    '    '                blnFlag = False
                    '    '            Else
                    '    '                'Found and active
                    '    '                blnFlag = True
                    '    '            End If
                    '    '        End If
                    '    '        .Close()
                    '    '    End With
                    '    'End If

                    'End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            message = "Record already exist"
                            'Throw New ApplicationException("210011: Record already exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "BIZENTITY WITH (ROWLOCK)"
                                .AddField("BizRegID", CompanyCont.BizRegID, SQLControl.EnumDataType.dtStringN)
                                .AddField("CompanyName", CompanyCont.CompanyName, SQLControl.EnumDataType.dtStringN)
                                .AddField("CompanyType", CompanyCont.CompanyType, SQLControl.EnumDataType.dtString)
                                .AddField("Industrytype", CompanyCont.Industrytype, SQLControl.EnumDataType.dtString)
                                .AddField("BusinessType", CompanyCont.BusinessType, SQLControl.EnumDataType.dtString)
                                .AddField("RegNo", CompanyCont.RegNo, SQLControl.EnumDataType.dtStringN)
                                '.AddField("AcctNo", CompanyCont.AcctNo, SQLControl.EnumDataType.dtString)
                                .AddField("Address1", CompanyCont.Address1, SQLControl.EnumDataType.dtString)
                                .AddField("Address2", CompanyCont.Address2, SQLControl.EnumDataType.dtString)
                                .AddField("Address3", CompanyCont.Address3, SQLControl.EnumDataType.dtString)
                                .AddField("Address4", CompanyCont.Address4, SQLControl.EnumDataType.dtString)
                                .AddField("PostalCode", CompanyCont.PostalCode, SQLControl.EnumDataType.dtString)
                                .AddField("State", CompanyCont.State, SQLControl.EnumDataType.dtString)
                                .AddField("Country", CompanyCont.Country, SQLControl.EnumDataType.dtString)
                                .AddField("City", CompanyCont.City, SQLControl.EnumDataType.dtString)
                                .AddField("Area", CompanyCont.Area, SQLControl.EnumDataType.dtString)
                                .AddField("PBT", CompanyCont.PBT, SQLControl.EnumDataType.dtString)
                                .AddField("TelNo", CompanyCont.TelNo, SQLControl.EnumDataType.dtString)
                                .AddField("FaxNo", CompanyCont.FaxNo, SQLControl.EnumDataType.dtString)
                                .AddField("Email", CompanyCont.Email, SQLControl.EnumDataType.dtStringN)
                                .AddField("CoWebsite", CompanyCont.CoWebsite, SQLControl.EnumDataType.dtStringN)
                                .AddField("ContactPerson", CompanyCont.ContactPerson, SQLControl.EnumDataType.dtStringN)
                                .AddField("ContactDesignation", CompanyCont.ContactDesignation, SQLControl.EnumDataType.dtStringN)
                                .AddField("ContactPersonEmail", CompanyCont.ContactPersonEmail, SQLControl.EnumDataType.dtStringN)
                                .AddField("ContactPersonTelNo", CompanyCont.ContactPersonTelNo, SQLControl.EnumDataType.dtString)
                                .AddField("ContactPersonFaxNo", CompanyCont.ContactPersonFaxNo, SQLControl.EnumDataType.dtString)
                                .AddField("ContactPersonMobile", CompanyCont.ContactPersonMobile, SQLControl.EnumDataType.dtString)
                                .AddField("Remark", CompanyCont.Remark, SQLControl.EnumDataType.dtStringN)
                                .AddField("Active", CompanyCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", CompanyCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", CompanyCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", CompanyCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", CompanyCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", CompanyCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                '.AddField("rowguid", CompanyCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("Flag", CompanyCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                .AddField("BizGrp", CompanyCont.BizGrp, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", CompanyCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RefID", CompanyCont.RefID, SQLControl.EnumDataType.dtString)
                                .AddField("KKM", CompanyCont.KKM, SQLControl.EnumDataType.dtNumeric)
                                'Remark by Mei
                                '.AddField("SubGrp", CompanyCont.SubGrp, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "RegNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, CompanyCont.RegNo) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("RegNo", CompanyCont.RegNo, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "RegNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, CompanyCont.RegNo) & "'")
                                End Select

                                'Roslyn, 04/02/15, update existing company in BIZENTITY instead of adding new one
                                If message <> "branch already exist" Then
                                    ListSQL.Add(strSQL)
                                End If

                            End With

                            'save Location

                            If BizlocateCont Is Nothing Then
                                'Message return
                            Else
                                blnExec = False
                                blnFound = False
                                blnFlag = False

                                With BizlocateInfo.MyInfo
                                    strSQL = BuildSelect(.CheckFields, .TableName, "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "' OR (BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizRegID) & "' AND BranchName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BranchName) & "')")
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

                                With objSQL
                                    .TableName = "BIZLOCATE WITH (ROWLOCK)"
                                    .AddField("BizRegID", BizlocateCont.BizRegID, SQLControl.EnumDataType.dtString)
                                    .AddField("BranchName", BizlocateCont.BranchName, SQLControl.EnumDataType.dtStringN)
                                    .AddField("BranchCode", BizlocateCont.BranchCode, SQLControl.EnumDataType.dtString)
                                    .AddField("AccNo", BizlocateCont.AccNo, SQLControl.EnumDataType.dtString)
                                    .AddField("Address1", BizlocateCont.Address1, SQLControl.EnumDataType.dtString)
                                    .AddField("Address2", BizlocateCont.Address2, SQLControl.EnumDataType.dtString)
                                    .AddField("Address3", BizlocateCont.Address3, SQLControl.EnumDataType.dtString)
                                    .AddField("Address4", BizlocateCont.Address4, SQLControl.EnumDataType.dtString)
                                    .AddField("PostalCode", BizlocateCont.PostalCode, SQLControl.EnumDataType.dtString)
                                    .AddField("ContactPerson", BizlocateCont.ContactPerson, SQLControl.EnumDataType.dtString)
                                    .AddField("ContactDesignation", BizlocateCont.ContactDesignation, SQLControl.EnumDataType.dtString)
                                    .AddField("ContactEmail", BizlocateCont.ContactEmail, SQLControl.EnumDataType.dtString)
                                    .AddField("ContactTelNo", BizlocateCont.ContactTelNo, SQLControl.EnumDataType.dtString)
                                    .AddField("ContactMobile", BizlocateCont.ContactMobile, SQLControl.EnumDataType.dtString)
                                    .AddField("StoreType", BizlocateCont.StoreType, SQLControl.EnumDataType.dtString)
                                    .AddField("Email", BizlocateCont.Email, SQLControl.EnumDataType.dtStringN)
                                    .AddField("Tel", BizlocateCont.Tel, SQLControl.EnumDataType.dtString)
                                    .AddField("Fax", BizlocateCont.Fax, SQLControl.EnumDataType.dtString)
                                    .AddField("Region", BizlocateCont.Region, SQLControl.EnumDataType.dtString)
                                    .AddField("Country", BizlocateCont.Country, SQLControl.EnumDataType.dtString)
                                    .AddField("State", BizlocateCont.State, SQLControl.EnumDataType.dtString)
                                    .AddField("PBT", BizlocateCont.PBT, SQLControl.EnumDataType.dtString)
                                    .AddField("City", BizlocateCont.City, SQLControl.EnumDataType.dtString)
                                    .AddField("Area", BizlocateCont.Area, SQLControl.EnumDataType.dtString)
                                    .AddField("Currency", BizlocateCont.Currency, SQLControl.EnumDataType.dtString)
                                    .AddField("StoreStatus", BizlocateCont.StoreStatus, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpPrevBook", BizlocateCont.OpPrevBook, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpTimeStart", BizlocateCont.OpTimeStart, SQLControl.EnumDataType.dtString)
                                    .AddField("OpTimeEnd", BizlocateCont.OpTimeEnd, SQLControl.EnumDataType.dtString)
                                    .AddField("OpDay1", BizlocateCont.OpDay1, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpDay2", BizlocateCont.OpDay2, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpDay3", BizlocateCont.OpDay3, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpDay4", BizlocateCont.OpDay4, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpDay5", BizlocateCont.OpDay5, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpDay6", BizlocateCont.OpDay6, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpDay7", BizlocateCont.OpDay7, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpBookAlwDY", BizlocateCont.OpBookAlwDY, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpBookAlwHR", BizlocateCont.OpBookAlwHR, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpBookFirst", BizlocateCont.OpBookFirst, SQLControl.EnumDataType.dtString)
                                    .AddField("OpBookLast", BizlocateCont.OpBookLast, SQLControl.EnumDataType.dtString)
                                    .AddField("OpBookIntv", BizlocateCont.OpBookIntv, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("SalesItemType", BizlocateCont.SalesItemType, SQLControl.EnumDataType.dtStringN)
                                    .AddField("InSvcItemType", BizlocateCont.InSvcItemType, SQLControl.EnumDataType.dtStringN)
                                    .AddField("GenInSvcID", BizlocateCont.GenInSvcID, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RcpHeader", BizlocateCont.RcpHeader, SQLControl.EnumDataType.dtStringN)
                                    .AddField("RcpFooter", BizlocateCont.RcpFooter, SQLControl.EnumDataType.dtStringN)
                                    .AddField("PriceLevel", BizlocateCont.PriceLevel, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("IsStockTake", BizlocateCont.IsStockTake, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Active", BizlocateCont.Active, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Inuse", BizlocateCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CreateDate", BizlocateCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("IsHost", BizlocateCont.IsHost, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", BizlocateCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", BizlocateCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                    '.AddField("rowguid", BizlocateCont.rowguid, SQLControl.EnumDataType.dtString)
                                    .AddField("Flag", BizlocateCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("BankAccount", BizlocateCont.BankAccount, SQLControl.EnumDataType.dtString)
                                    .AddField("BranchType", BizlocateCont.BranchType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CreateBy", BizlocateCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("Industrytype", BizlocateCont.IndustryType, SQLControl.EnumDataType.dtString)
                                    .AddField("BusinessType", BizlocateCont.BusinessType, SQLControl.EnumDataType.dtString)
                                    .AddField("RefID", BizlocateCont.RefID, SQLControl.EnumDataType.dtString)

                                    If blnFound = True Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "'")

                                    Else
                                        .AddField("BizLocID", BizlocateCont.BizLocID, SQLControl.EnumDataType.dtString)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    End If
                                End With
                                ListSQL.Add(strSQL)
                            End If

                            'save UserProfile
                            If UsrprofileCont Is Nothing Then
                                'Message return
                            Else
                                blnExec = False
                                blnFound = False
                                blnFlag = False


                                With UserProfileInfo.MyInfo
                                    strSQL = BuildSelect(.CheckFields, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
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

                                With objSQL
                                    .TableName = "USRPROFILE WITH (ROWLOCK)"
                                    .AddField("UserName", UsrprofileCont.UserName, SQLControl.EnumDataType.dtStringN)
                                    .AddField("Password", UsrprofileCont.Password, SQLControl.EnumDataType.dtString)
                                    .AddField("RefID", UsrprofileCont.RefID, SQLControl.EnumDataType.dtString)
                                    .AddField("RefType", UsrprofileCont.RefType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ParentID", UsrprofileCont.ParentID, SQLControl.EnumDataType.dtString)
                                    .AddField("Status", UsrprofileCont.Status, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Logged", UsrprofileCont.Logged, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LogStation", UsrprofileCont.LogStation, SQLControl.EnumDataType.dtString)
                                    .AddField("LastLogin", UsrprofileCont.LastLogin, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("LastLogout", UsrprofileCont.LastLogout, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateDate", UsrprofileCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("LastUpdate", UsrprofileCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", UsrprofileCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("UpdateBy", UsrprofileCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                    '.AddField("SyncCreate", UsrprofileCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                    '.AddField("SyncLastUpd", UsrprofileCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)

                                    If blnFound = True Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                                    Else
                                        .AddField("UserID", UsrprofileCont.UserID, SQLControl.EnumDataType.dtString)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    End If
                                    ListSQL.Add(strSQL)

                                    .TableName = "USRAPP WITH (ROWLOCK)"
                                    .AddField("AccessCode", USRAPP.AccessCode, SQLControl.EnumDataType.dtString)
                                    .AddField("IsInherit", USRAPP.IsInherit, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("AppID", USRAPP.AppID, SQLControl.EnumDataType.dtNumeric)

                                    Select Case pType
                                        Case SQLControl.EnumSQLType.stInsert
                                            'Add
                                            .AddField("UserID", USRAPP.UserID, SQLControl.EnumDataType.dtString)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    End Select

                                    ListSQL.Add(strSQL)

                                End With
                            End If

                            'save Employee
                            If EmployeeCont Is Nothing Then
                                'Message return
                            Else
                                blnExec = False
                                blnFound = False
                                blnFlag = False
                                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                                    StartSQLControl()
                                    With EmployeeInfo.MyInfo
                                        strSQL = BuildSelect(.CheckFields, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
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

                                With objSQL
                                    .TableName = "EMPLOYEE WITH (ROWLOCK)"
                                    .AddField("NickName", EmployeeCont.NickName, SQLControl.EnumDataType.dtString)
                                    .AddField("SurName", EmployeeCont.SurName, SQLControl.EnumDataType.dtStringN)
                                    .AddField("FirstName", EmployeeCont.FirstName, SQLControl.EnumDataType.dtStringN)
                                    .AddField("Salutation", EmployeeCont.Salutation, SQLControl.EnumDataType.dtString)
                                    .AddField("Sex", EmployeeCont.Sex, SQLControl.EnumDataType.dtString)
                                    .AddField("DOB", EmployeeCont.DOB, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("PlaceOfBirth", EmployeeCont.PlaceOfBirth, SQLControl.EnumDataType.dtStringN)
                                    .AddField("NRICNo", EmployeeCont.NRICNo, SQLControl.EnumDataType.dtStringN)
                                    .AddField("Nationality", EmployeeCont.Nationality, SQLControl.EnumDataType.dtString)
                                    .AddField("Race", EmployeeCont.Race, SQLControl.EnumDataType.dtString)
                                    .AddField("Religion", EmployeeCont.Religion, SQLControl.EnumDataType.dtString)
                                    .AddField("Marital", EmployeeCont.Marital, SQLControl.EnumDataType.dtString)
                                    .AddField("CoAddress1", EmployeeCont.CoAddress1, SQLControl.EnumDataType.dtString)
                                    .AddField("CoAddress2", EmployeeCont.CoAddress2, SQLControl.EnumDataType.dtString)
                                    .AddField("CoAddress3", EmployeeCont.CoAddress3, SQLControl.EnumDataType.dtString)
                                    .AddField("CoAddress4", EmployeeCont.CoAddress4, SQLControl.EnumDataType.dtString)
                                    .AddField("CoPostalCode", EmployeeCont.CoPostalCode, SQLControl.EnumDataType.dtString)
                                    .AddField("CoState", EmployeeCont.CoState, SQLControl.EnumDataType.dtString)
                                    .AddField("CoCountry", EmployeeCont.CoCountry, SQLControl.EnumDataType.dtString)
                                    .AddField("PnAddress1", EmployeeCont.PnAddress1, SQLControl.EnumDataType.dtString)
                                    .AddField("PnAddress2", EmployeeCont.PnAddress2, SQLControl.EnumDataType.dtString)
                                    .AddField("PnAddress3", EmployeeCont.PnAddress3, SQLControl.EnumDataType.dtString)
                                    .AddField("PnAddress4", EmployeeCont.PnAddress4, SQLControl.EnumDataType.dtString)
                                    .AddField("PnPostalCode", EmployeeCont.PnPostalCode, SQLControl.EnumDataType.dtString)
                                    .AddField("PnState", EmployeeCont.PnState, SQLControl.EnumDataType.dtString)
                                    .AddField("PnCountry", EmployeeCont.PnCountry, SQLControl.EnumDataType.dtString)
                                    .AddField("EmerContactPerson", EmployeeCont.EmerContactPerson, SQLControl.EnumDataType.dtStringN)
                                    .AddField("EmerContactNo", EmployeeCont.EmerContactNo, SQLControl.EnumDataType.dtString)
                                    .AddField("EmailAddress", EmployeeCont.EmailAddress, SQLControl.EnumDataType.dtStringN)
                                    .AddField("Designation", EmployeeCont.Designation, SQLControl.EnumDataType.dtStringN)
                                    .AddField("ForeignLocal", EmployeeCont.ForeignLocal, SQLControl.EnumDataType.dtString)
                                    .AddField("CommID", EmployeeCont.CommID, SQLControl.EnumDataType.dtString)
                                    .AddField("Salary", EmployeeCont.Salary, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OffDay", EmployeeCont.OffDay, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Overtime", EmployeeCont.Overtime, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Leave", EmployeeCont.Leave, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Levy", EmployeeCont.Levy, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Allergies", EmployeeCont.Allergies, SQLControl.EnumDataType.dtStringN)
                                    .AddField("ClerkNo", EmployeeCont.ClerkNo, SQLControl.EnumDataType.dtStringN)
                                    .AddField("DateHired", EmployeeCont.DateHired, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("DateLeft", EmployeeCont.DateLeft, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("TransportAllowance", EmployeeCont.TransportAllowance, SQLControl.EnumDataType.dtStringN)
                                    .AddField("ServiceAllowance", EmployeeCont.ServiceAllowance, SQLControl.EnumDataType.dtStringN)
                                    .AddField("OtherAllowance", EmployeeCont.OtherAllowance, SQLControl.EnumDataType.dtStringN)
                                    .AddField("Remarks", EmployeeCont.Remarks, SQLControl.EnumDataType.dtStringN)
                                    .AddField("PrivilegeCode", EmployeeCont.PrivilegeCode, SQLControl.EnumDataType.dtString)
                                    .AddField("Status", EmployeeCont.Status, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CreateDate", EmployeeCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", EmployeeCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", EmployeeCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", EmployeeCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("Inuse", EmployeeCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                    '.AddField("rowguid", EmployeeCont.rowguid, SQLControl.EnumDataType.dtString)
                                    .AddField("IsHost", EmployeeCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastSyncBy", EmployeeCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                                    .AddField("AccessLvl", EmployeeCont.AccessLvl, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CompanyID", EmployeeCont.CompanyID, SQLControl.EnumDataType.dtString)
                                    .AddField("locID", EmployeeCont.LocID, SQLControl.EnumDataType.dtString)
                                    .AddField("Flag", EmployeeCont.Flag, SQLControl.EnumDataType.dtNumeric)

                                    If blnFound = True Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
                                    Else
                                        .AddField("EmployeeID", EmployeeCont.EmployeeID, SQLControl.EnumDataType.dtString)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    End If
                                    ListSQL.Add(strSQL)
                                End With
                            End If

                            'save UserProfile
                            If EmployeeUsrprofileCont Is Nothing OrElse EMPLOYEEUSRAPP Is Nothing Then
                                'Message return
                            Else
                                blnExec = False
                                blnFound = False
                                blnFlag = False


                                With UserProfileInfo.MyInfo
                                    strSQL = BuildSelect(.CheckFields, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeUsrprofileCont.UserID) & "'")
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

                                With objSQL
                                    .TableName = "USRPROFILE WITH (ROWLOCK)"
                                    .AddField("UserName", EmployeeUsrprofileCont.UserName, SQLControl.EnumDataType.dtStringN)
                                    .AddField("Password", EmployeeUsrprofileCont.Password, SQLControl.EnumDataType.dtString)
                                    .AddField("RefID", EmployeeUsrprofileCont.RefID, SQLControl.EnumDataType.dtString)
                                    .AddField("RefType", EmployeeUsrprofileCont.RefType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ParentID", EmployeeUsrprofileCont.ParentID, SQLControl.EnumDataType.dtString)
                                    .AddField("Status", EmployeeUsrprofileCont.Status, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Logged", EmployeeUsrprofileCont.Logged, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LogStation", EmployeeUsrprofileCont.LogStation, SQLControl.EnumDataType.dtString)
                                    .AddField("LastLogin", EmployeeUsrprofileCont.LastLogin, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("LastLogout", EmployeeUsrprofileCont.LastLogout, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateDate", EmployeeUsrprofileCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("LastUpdate", EmployeeUsrprofileCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", EmployeeUsrprofileCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("UpdateBy", EmployeeUsrprofileCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                    If blnFound = True Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeUsrprofileCont.UserID) & "'")
                                    Else
                                        .AddField("UserID", EmployeeUsrprofileCont.UserID, SQLControl.EnumDataType.dtString)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    End If
                                    ListSQL.Add(strSQL)

                                    .TableName = "USRAPP WITH (ROWLOCK)"
                                    .AddField("AccessCode", EMPLOYEEUSRAPP.AccessCode, SQLControl.EnumDataType.dtString)
                                    .AddField("IsInherit", EMPLOYEEUSRAPP.IsInherit, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("AppID", EMPLOYEEUSRAPP.AppID, SQLControl.EnumDataType.dtNumeric)

                                    If blnFound = True Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeUsrprofileCont.UserID) & "'")
                                    Else
                                        Select Case pType
                                            Case SQLControl.EnumSQLType.stInsert
                                                'Add
                                                .AddField("UserID", EMPLOYEEUSRAPP.UserID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                        End Select
                                    End If

                                    ListSQL.Add(strSQL)

                                End With
                            End If



                            'With objSQL
                            '    .TableName = "EMPBRANCH WITH (ROWLOCK)"
                            '    .AddField("InAppt", EmployeeCont.InAppt, SQLControl.EnumDataType.dtNumeric)
                            '    .AddField("IsServer", EmployeeCont.IsServer, SQLControl.EnumDataType.dtNumeric)
                            '    .AddField("IsHost", EmployeeCont.IsHost, SQLControl.EnumDataType.dtNumeric)


                            '    .AddField("EmployeeID", EmployeeCont.EmployeeID, SQLControl.EnumDataType.dtString)
                            '    .AddField("LocID", EmployeeCont.LocID, SQLControl.EnumDataType.dtString)
                            '    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            'End With
                            If EmployeeBranchCont Is Nothing Then
                                'Message return
                            Else
                                blnExec = False
                                blnFound = False
                                blnFlag = False
                                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                                    StartSQLControl()
                                    With EmployeeInfo.MyInfo
                                        strSQL = BuildSelect(.CheckFields, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeBranchCont.EmployeeID) & "'")
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

                                With objSQL
                                    .TableName = "EMPBRANCH WITH (ROWLOCK)"
                                    .AddField("InAppt", EmployeeBranchCont.InAppt, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("IsServer", EmployeeBranchCont.IsServer, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("IsHost", EmployeeBranchCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("EmployeeID", EmployeeBranchCont.EmployeeID, SQLControl.EnumDataType.dtString)
                                    .AddField("LocID", EmployeeBranchCont.LocID, SQLControl.EnumDataType.dtString)

                                    If blnFound = True Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeBranchCont.EmployeeID) & "'")
                                    Else
                                        .AddField("EmployeeID", EmployeeBranchCont.EmployeeID, SQLControl.EnumDataType.dtString)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    End If
                                    ListSQL.Add(strSQL)
                                End With
                            End If

                            strSQL = "UPDATE DOEFILENOREG SET Inuse=1, RefID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.BizRegID) & "' WHERE ID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.RefID) & "' AND DOEFileNo='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.AccNo) & "'"
                            ListSQL.Add(strSQL)


                            Try
                                'execute
                                objConn.BatchExecute(ListSQL, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If

                                Dim sqlStatement As String = " "
                                If objConn.FailedSQLStatement.Count > 0 Then
                                    sqlStatement &= objConn.FailedSQLStatement.Item(0)
                                End If

                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
                                Return False

                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                CompanyCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()

            End Try

        End Function


        Public Function InsertRegistration(ByVal CompanyCont As Profiles.Container.Company, ByVal BizlocateCont As Profiles.Container.Bizlocate, ByRef message As String, Optional ByVal UsrprofileCont As UserSecurity.Container.UserProfile = Nothing, Optional ByVal USRAPP As UserSecurity.UsrApp = Nothing, Optional ByVal Employee As Profiles.Container.Employee = Nothing, Optional ByVal EmployeeUsrprofileCont As UserSecurity.Container.UserProfile = Nothing, Optional ByVal EMPLOYEEUSRAPP As UserSecurity.UsrApp = Nothing, Optional ByVal EmployeeBranch As Profiles.Container.Empbranch = Nothing) As Boolean
            Return SaveRegistration(CompanyCont, BizlocateCont, UsrprofileCont, USRAPP, Employee, EmployeeUsrprofileCont, EMPLOYEEUSRAPP, SQLControl.EnumSQLType.stInsert, message, EmployeeBranch)
        End Function
        'end added

        'added by Antoni Bernad 10272014 - BatchExecute Approval

        Public Function SaveRegistrationApproval(ByVal CompanyCont As Profiles.Container.Company, ByVal BizlocateCont As Profiles.Container.Bizlocate, ByVal UsrprofileCont As UserSecurity.Container.UserProfile, ByVal USRAPP As UserSecurity.UsrApp, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveRegistrationApproval = False

            Try
                If CompanyCont Is Nothing Then

                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    StartSQLControl()

                    With CompanyInfo.MyInfo
                        strSQL = BuildSelect(.CheckFields, .TableName, "BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.BizRegID) & "'")
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
                    StartSQLControl()
                    With objSQL

                        .TableName = "BIZENTITY WITH (ROWLOCK)"
                        .AddField("CompanyName", CompanyCont.CompanyName, SQLControl.EnumDataType.dtStringN)
                        .AddField("CompanyType", CompanyCont.CompanyType, SQLControl.EnumDataType.dtString)
                        .AddField("Industrytype", CompanyCont.Industrytype, SQLControl.EnumDataType.dtString)
                        .AddField("BusinessType", CompanyCont.BusinessType, SQLControl.EnumDataType.dtString)
                        .AddField("RegNo", CompanyCont.RegNo, SQLControl.EnumDataType.dtStringN)
                        .AddField("AcctNo", CompanyCont.AcctNo, SQLControl.EnumDataType.dtString)
                        .AddField("Address1", CompanyCont.Address1, SQLControl.EnumDataType.dtString)
                        .AddField("Address2", CompanyCont.Address2, SQLControl.EnumDataType.dtString)
                        .AddField("Address3", CompanyCont.Address3, SQLControl.EnumDataType.dtString)
                        .AddField("Address4", CompanyCont.Address4, SQLControl.EnumDataType.dtString)
                        .AddField("PostalCode", CompanyCont.PostalCode, SQLControl.EnumDataType.dtString)
                        .AddField("State", CompanyCont.State, SQLControl.EnumDataType.dtString)
                        .AddField("Country", CompanyCont.Country, SQLControl.EnumDataType.dtString)
                        .AddField("City", CompanyCont.City, SQLControl.EnumDataType.dtString)
                        .AddField("Area", CompanyCont.Area, SQLControl.EnumDataType.dtString)
                        .AddField("TelNo", CompanyCont.TelNo, SQLControl.EnumDataType.dtString)
                        .AddField("FaxNo", CompanyCont.FaxNo, SQLControl.EnumDataType.dtString)
                        .AddField("Email", CompanyCont.Email, SQLControl.EnumDataType.dtStringN)
                        .AddField("CoWebsite", CompanyCont.CoWebsite, SQLControl.EnumDataType.dtStringN)
                        .AddField("ContactPerson", CompanyCont.ContactPerson, SQLControl.EnumDataType.dtStringN)
                        .AddField("ContactDesignation", CompanyCont.ContactDesignation, SQLControl.EnumDataType.dtStringN)
                        .AddField("ContactPersonEmail", CompanyCont.ContactPersonEmail, SQLControl.EnumDataType.dtStringN)
                        .AddField("ContactPersonTelNo", CompanyCont.ContactPersonTelNo, SQLControl.EnumDataType.dtString)
                        .AddField("ContactPersonFaxNo", CompanyCont.ContactPersonFaxNo, SQLControl.EnumDataType.dtString)
                        .AddField("ContactPersonMobile", CompanyCont.ContactPersonMobile, SQLControl.EnumDataType.dtString)
                        .AddField("Remark", CompanyCont.Remark, SQLControl.EnumDataType.dtStringN)
                        .AddField("Active", CompanyCont.Active, SQLControl.EnumDataType.dtNumeric)
                        .AddField("Inuse", CompanyCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                        .AddField("CreateDate", CompanyCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                        .AddField("CreateBy", CompanyCont.CreateBy, SQLControl.EnumDataType.dtString)
                        .AddField("LastUpdate", CompanyCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                        .AddField("UpdateBy", CompanyCont.UpdateBy, SQLControl.EnumDataType.dtString)
                        '.AddField("rowguid", CompanyCont.rowguid, SQLControl.EnumDataType.dtString)
                        .AddField("Flag", CompanyCont.Flag, SQLControl.EnumDataType.dtNumeric)
                        .AddField("BizGrp", CompanyCont.BizGrp, SQLControl.EnumDataType.dtNumeric)
                        .AddField("Status", CompanyCont.Status, SQLControl.EnumDataType.dtNumeric)


                        If blnFound = True Then
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.BizRegID) & "'")
                        Else
                            .AddField("BizRegID", CompanyCont.BizRegID, SQLControl.EnumDataType.dtString)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                        End If
                        ListSQL.Add(strSQL)

                    End With

                    'save Location

                    If BizlocateCont Is Nothing Then
                        'Message return
                    Else
                        blnExec = False
                        blnFound = False
                        blnFlag = False

                        With BizlocateInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "' OR (BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizRegID) & "' AND BranchName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BranchName) & "')")
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
                        With objSQL
                            .TableName = "BIZLOCATE WITH (ROWLOCK)"
                            .AddField("BizRegID", BizlocateCont.BizRegID, SQLControl.EnumDataType.dtString)
                            .AddField("BranchName", BizlocateCont.BranchName, SQLControl.EnumDataType.dtStringN)
                            .AddField("BranchCode", BizlocateCont.BranchCode, SQLControl.EnumDataType.dtString)
                            .AddField("AccNo", BizlocateCont.AccNo, SQLControl.EnumDataType.dtString)
                            .AddField("Address1", BizlocateCont.Address1, SQLControl.EnumDataType.dtString)
                            .AddField("Address2", BizlocateCont.Address2, SQLControl.EnumDataType.dtString)
                            .AddField("Address3", BizlocateCont.Address3, SQLControl.EnumDataType.dtString)
                            .AddField("Address4", BizlocateCont.Address4, SQLControl.EnumDataType.dtString)
                            .AddField("PostalCode", BizlocateCont.PostalCode, SQLControl.EnumDataType.dtString)
                            .AddField("ContactPerson", BizlocateCont.ContactPerson, SQLControl.EnumDataType.dtString)
                            .AddField("ContactDesignation", BizlocateCont.ContactDesignation, SQLControl.EnumDataType.dtString)
                            .AddField("ContactEmail", BizlocateCont.ContactEmail, SQLControl.EnumDataType.dtString)
                            .AddField("ContactTelNo", BizlocateCont.ContactTelNo, SQLControl.EnumDataType.dtString)
                            .AddField("ContactMobile", BizlocateCont.ContactMobile, SQLControl.EnumDataType.dtString)
                            .AddField("StoreType", BizlocateCont.StoreType, SQLControl.EnumDataType.dtString)
                            .AddField("Email", BizlocateCont.Email, SQLControl.EnumDataType.dtStringN)
                            .AddField("Tel", BizlocateCont.Tel, SQLControl.EnumDataType.dtString)
                            .AddField("Fax", BizlocateCont.Fax, SQLControl.EnumDataType.dtString)
                            .AddField("Region", BizlocateCont.Region, SQLControl.EnumDataType.dtString)
                            .AddField("Country", BizlocateCont.Country, SQLControl.EnumDataType.dtString)
                            .AddField("State", BizlocateCont.State, SQLControl.EnumDataType.dtString)
                            .AddField("City", BizlocateCont.City, SQLControl.EnumDataType.dtString)
                            .AddField("Area", BizlocateCont.Area, SQLControl.EnumDataType.dtString)
                            .AddField("Currency", BizlocateCont.Currency, SQLControl.EnumDataType.dtString)
                            .AddField("StoreStatus", BizlocateCont.StoreStatus, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpPrevBook", BizlocateCont.OpPrevBook, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpTimeStart", BizlocateCont.OpTimeStart, SQLControl.EnumDataType.dtString)
                            .AddField("OpTimeEnd", BizlocateCont.OpTimeEnd, SQLControl.EnumDataType.dtString)
                            .AddField("OpDay1", BizlocateCont.OpDay1, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpDay2", BizlocateCont.OpDay2, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpDay3", BizlocateCont.OpDay3, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpDay4", BizlocateCont.OpDay4, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpDay5", BizlocateCont.OpDay5, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpDay6", BizlocateCont.OpDay6, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpDay7", BizlocateCont.OpDay7, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpBookAlwDY", BizlocateCont.OpBookAlwDY, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpBookAlwHR", BizlocateCont.OpBookAlwHR, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpBookFirst", BizlocateCont.OpBookFirst, SQLControl.EnumDataType.dtString)
                            .AddField("OpBookLast", BizlocateCont.OpBookLast, SQLControl.EnumDataType.dtString)
                            .AddField("OpBookIntv", BizlocateCont.OpBookIntv, SQLControl.EnumDataType.dtNumeric)
                            .AddField("SalesItemType", BizlocateCont.SalesItemType, SQLControl.EnumDataType.dtStringN)
                            .AddField("InSvcItemType", BizlocateCont.InSvcItemType, SQLControl.EnumDataType.dtStringN)
                            .AddField("GenInSvcID", BizlocateCont.GenInSvcID, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RcpHeader", BizlocateCont.RcpHeader, SQLControl.EnumDataType.dtStringN)
                            .AddField("RcpFooter", BizlocateCont.RcpFooter, SQLControl.EnumDataType.dtStringN)
                            .AddField("PriceLevel", BizlocateCont.PriceLevel, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsStockTake", BizlocateCont.IsStockTake, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Active", BizlocateCont.Active, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Inuse", BizlocateCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                            .AddField("CreateDate", BizlocateCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("IsHost", BizlocateCont.IsHost, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", BizlocateCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", BizlocateCont.UpdateBy, SQLControl.EnumDataType.dtString)
                            '.AddField("rowguid", BizlocateCont.rowguid, SQLControl.EnumDataType.dtString)
                            .AddField("Flag", BizlocateCont.Flag, SQLControl.EnumDataType.dtNumeric)
                            .AddField("BankAccount", BizlocateCont.BankAccount, SQLControl.EnumDataType.dtString)
                            .AddField("BranchType", BizlocateCont.BranchType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("CreateBy", BizlocateCont.CreateBy, SQLControl.EnumDataType.dtString)

                            If blnFound = True Then
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "'")

                            Else
                                .AddField("BizLocID", BizlocateCont.BizLocID, SQLControl.EnumDataType.dtString)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            End If
                        End With
                        ListSQL.Add(strSQL)
                    End If

                    'save UserProfile
                    If UsrprofileCont Is Nothing Then
                        'Message return
                    Else
                        blnExec = False
                        blnFound = False
                        blnFlag = False


                        With UserProfileInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
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

                        With objSQL
                            .TableName = "USRPROFILE WITH (ROWLOCK)"
                            .AddField("UserName", UsrprofileCont.UserName, SQLControl.EnumDataType.dtStringN)
                            .AddField("Password", UsrprofileCont.Password, SQLControl.EnumDataType.dtString)
                            .AddField("RefID", UsrprofileCont.RefID, SQLControl.EnumDataType.dtString)
                            .AddField("RefType", UsrprofileCont.RefType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ParentID", UsrprofileCont.ParentID, SQLControl.EnumDataType.dtString)
                            .AddField("Status", UsrprofileCont.Status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Logged", UsrprofileCont.Logged, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LogStation", UsrprofileCont.LogStation, SQLControl.EnumDataType.dtString)
                            .AddField("LastLogin", UsrprofileCont.LastLogin, SQLControl.EnumDataType.dtDateTime)
                            .AddField("LastLogout", UsrprofileCont.LastLogout, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateDate", UsrprofileCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("LastUpdate", UsrprofileCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", UsrprofileCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("UpdateBy", UsrprofileCont.UpdateBy, SQLControl.EnumDataType.dtString)
                            '.AddField("SyncCreate", UsrprofileCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                            '.AddField("SyncLastUpd", UsrprofileCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)

                            If blnFound = True Then
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                            Else
                                .AddField("UserID", UsrprofileCont.UserID, SQLControl.EnumDataType.dtString)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            End If
                            ListSQL.Add(strSQL)

                            .TableName = "USRAPP WITH (ROWLOCK)"
                            .AddField("AccessCode", USRAPP.AccessCode, SQLControl.EnumDataType.dtString)
                            .AddField("IsInherit", USRAPP.IsInherit, SQLControl.EnumDataType.dtNumeric)
                            .AddField("AppID", USRAPP.AppID, SQLControl.EnumDataType.dtNumeric)

                            .AddField("UserID", USRAPP.UserID, SQLControl.EnumDataType.dtString)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            ListSQL.Add(strSQL)

                        End With
                    End If

                    'ongoing added by diana 20141120
                    If Not CompanyCont Is Nothing AndAlso Not BizlocateCont Is Nothing Then
                        strSQL = "UPDATE USRPROFILE WITH (ROWLOCK) SET STATUS=1 WHERE RefID=(SELECT TOP 1 EMPLOYEEID FROM EMPLOYEE WITH (NOLOCK) WHERE CompanyID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.BizRegID) & "' AND LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "')"
                        ListSQL.Add(strSQL)
                    End If

                    Try
                        'execute
                        objConn.BatchExecute(ListSQL, CommandType.Text)
                        Return True

                    Catch axExecute As Exception
                        Dim sqlStatement As String = " "
                        If objConn.FailedSQLStatement.Count > 0 Then
                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
                        End If

                        message = axExecute.Message.ToString()
                        Log.Notifier.Notify(axExecute)
                        Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
                        Return False

                        'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                    Finally
                        objSQL.Dispose()
                    End Try
                End If
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                CompanyCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try

        End Function



        Public Function InsertRegistrationApproval(ByVal CompanyCont As Profiles.Container.Company, ByRef message As String, Optional ByVal BizlocateCont As Profiles.Container.Bizlocate = Nothing, Optional ByVal UsrprofileCont As UserSecurity.Container.UserProfile = Nothing, Optional ByVal USRAPP As UserSecurity.UsrApp = Nothing) As Boolean
            Return SaveRegistrationApproval(CompanyCont, BizlocateCont, UsrprofileCont, USRAPP, message)
        End Function

        Public Function SaveEmployee(ByVal EmployeeCont As Profiles.Container.Employee, ByVal UsrprofileCont As UserSecurity.Container.UserProfile, ByVal USRAPP As UserSecurity.UsrApp, ByVal ListContEmployeeBranch As List(Of Profiles.Container.Empbranch), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveEmployee = False

            Try
                If EmployeeCont Is Nothing Then

                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    StartSQLControl()
                    With EmployeeInfo.MyInfo
                        strSQL = BuildSelect(.CheckFields, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
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
                    StartSQLControl()

                    With objSQL
                        .TableName = "EMPLOYEE WITH (ROWLOCK)"
                        .AddField("NickName", EmployeeCont.NickName, SQLControl.EnumDataType.dtString)
                        .AddField("SurName", EmployeeCont.SurName, SQLControl.EnumDataType.dtStringN)
                        .AddField("FirstName", EmployeeCont.FirstName, SQLControl.EnumDataType.dtStringN)
                        .AddField("Salutation", EmployeeCont.Salutation, SQLControl.EnumDataType.dtString)
                        .AddField("Sex", EmployeeCont.Sex, SQLControl.EnumDataType.dtString)
                        .AddField("DOB", EmployeeCont.DOB, SQLControl.EnumDataType.dtDateTime)
                        .AddField("PlaceOfBirth", EmployeeCont.PlaceOfBirth, SQLControl.EnumDataType.dtStringN)
                        .AddField("NRICNo", EmployeeCont.NRICNo, SQLControl.EnumDataType.dtStringN)
                        .AddField("Nationality", EmployeeCont.Nationality, SQLControl.EnumDataType.dtString)
                        .AddField("Race", EmployeeCont.Race, SQLControl.EnumDataType.dtString)
                        .AddField("Religion", EmployeeCont.Religion, SQLControl.EnumDataType.dtString)
                        .AddField("Marital", EmployeeCont.Marital, SQLControl.EnumDataType.dtString)
                        .AddField("CoAddress1", EmployeeCont.CoAddress1, SQLControl.EnumDataType.dtString)
                        .AddField("CoAddress2", EmployeeCont.CoAddress2, SQLControl.EnumDataType.dtString)
                        .AddField("CoAddress3", EmployeeCont.CoAddress3, SQLControl.EnumDataType.dtString)
                        .AddField("CoAddress4", EmployeeCont.CoAddress4, SQLControl.EnumDataType.dtString)
                        .AddField("CoPostalCode", EmployeeCont.CoPostalCode, SQLControl.EnumDataType.dtString)
                        .AddField("CoState", EmployeeCont.CoState, SQLControl.EnumDataType.dtString)
                        .AddField("CoCountry", EmployeeCont.CoCountry, SQLControl.EnumDataType.dtString)
                        .AddField("PnAddress1", EmployeeCont.PnAddress1, SQLControl.EnumDataType.dtString)
                        .AddField("PnAddress2", EmployeeCont.PnAddress2, SQLControl.EnumDataType.dtString)
                        .AddField("PnAddress3", EmployeeCont.PnAddress3, SQLControl.EnumDataType.dtString)
                        .AddField("PnAddress4", EmployeeCont.PnAddress4, SQLControl.EnumDataType.dtString)
                        .AddField("PnPostalCode", EmployeeCont.PnPostalCode, SQLControl.EnumDataType.dtString)
                        .AddField("PnState", EmployeeCont.PnState, SQLControl.EnumDataType.dtString)
                        .AddField("PnCountry", EmployeeCont.PnCountry, SQLControl.EnumDataType.dtString)
                        .AddField("EmerContactPerson", EmployeeCont.EmerContactPerson, SQLControl.EnumDataType.dtStringN)
                        .AddField("EmerContactNo", EmployeeCont.EmerContactNo, SQLControl.EnumDataType.dtString)
                        .AddField("EmailAddress", EmployeeCont.EmailAddress, SQLControl.EnumDataType.dtStringN)
                        .AddField("Designation", EmployeeCont.Designation, SQLControl.EnumDataType.dtStringN)
                        .AddField("ForeignLocal", EmployeeCont.ForeignLocal, SQLControl.EnumDataType.dtString)
                        .AddField("CommID", EmployeeCont.CommID, SQLControl.EnumDataType.dtString)
                        .AddField("Salary", EmployeeCont.Salary, SQLControl.EnumDataType.dtNumeric)
                        .AddField("OffDay", EmployeeCont.OffDay, SQLControl.EnumDataType.dtNumeric)
                        .AddField("Overtime", EmployeeCont.Overtime, SQLControl.EnumDataType.dtNumeric)
                        .AddField("Leave", EmployeeCont.Leave, SQLControl.EnumDataType.dtNumeric)
                        .AddField("Levy", EmployeeCont.Levy, SQLControl.EnumDataType.dtNumeric)
                        .AddField("Allergies", EmployeeCont.Allergies, SQLControl.EnumDataType.dtStringN)
                        .AddField("ClerkNo", EmployeeCont.ClerkNo, SQLControl.EnumDataType.dtStringN)
                        .AddField("DateHired", EmployeeCont.DateHired, SQLControl.EnumDataType.dtDateTime)
                        .AddField("DateLeft", EmployeeCont.DateLeft, SQLControl.EnumDataType.dtDateTime)
                        .AddField("TransportAllowance", EmployeeCont.TransportAllowance, SQLControl.EnumDataType.dtStringN)
                        .AddField("ServiceAllowance", EmployeeCont.ServiceAllowance, SQLControl.EnumDataType.dtStringN)
                        .AddField("OtherAllowance", EmployeeCont.OtherAllowance, SQLControl.EnumDataType.dtStringN)
                        .AddField("Remarks", EmployeeCont.Remarks, SQLControl.EnumDataType.dtStringN)
                        .AddField("PrivilegeCode", EmployeeCont.PrivilegeCode, SQLControl.EnumDataType.dtString)
                        .AddField("Status", EmployeeCont.Status, SQLControl.EnumDataType.dtNumeric)
                        .AddField("CreateDate", EmployeeCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                        .AddField("CreateBy", EmployeeCont.CreateBy, SQLControl.EnumDataType.dtString)
                        .AddField("LastUpdate", EmployeeCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                        .AddField("UpdateBy", EmployeeCont.UpdateBy, SQLControl.EnumDataType.dtString)
                        .AddField("Inuse", EmployeeCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                        '.AddField("rowguid", EmployeeCont.rowguid, SQLControl.EnumDataType.dtString)
                        .AddField("IsHost", EmployeeCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                        .AddField("LastSyncBy", EmployeeCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                        .AddField("AccessLvl", EmployeeCont.AccessLvl, SQLControl.EnumDataType.dtNumeric)
                        .AddField("CompanyID", EmployeeCont.CompanyID, SQLControl.EnumDataType.dtString)
                        .AddField("locID", EmployeeCont.LocID, SQLControl.EnumDataType.dtString)
                        .AddField("Flag", EmployeeCont.Flag, SQLControl.EnumDataType.dtNumeric)

                        Select Case pType
                            Case SQLControl.EnumSQLType.stInsert
                                If blnFound = True And blnFlag = False Then
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
                                Else
                                    If blnFound = False Then
                                        .AddField("EmployeeID", EmployeeCont.EmployeeID, SQLControl.EnumDataType.dtString)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    End If
                                End If
                            Case SQLControl.EnumSQLType.stUpdate
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
                        End Select
                        ListSQL.Add(strSQL)

                    End With

                    'save UserProfile
                    If UsrprofileCont Is Nothing Then
                        'Message return
                    Else
                        blnExec = False
                        blnFound = False
                        blnFlag = False


                        With UserProfileInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
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

                        With objSQL
                            .TableName = "USRPROFILE WITH (ROWLOCK)"
                            .AddField("UserName", UsrprofileCont.UserName, SQLControl.EnumDataType.dtStringN)
                            .AddField("Password", UsrprofileCont.Password, SQLControl.EnumDataType.dtString)
                            .AddField("RefType", UsrprofileCont.RefType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ParentID", UsrprofileCont.ParentID, SQLControl.EnumDataType.dtString)
                            .AddField("Status", UsrprofileCont.Status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Logged", UsrprofileCont.Logged, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LogStation", UsrprofileCont.LogStation, SQLControl.EnumDataType.dtString)
                            .AddField("LastLogin", UsrprofileCont.LastLogin, SQLControl.EnumDataType.dtDateTime)
                            .AddField("LastLogout", UsrprofileCont.LastLogout, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateDate", UsrprofileCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("LastUpdate", UsrprofileCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", UsrprofileCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("UpdateBy", UsrprofileCont.UpdateBy, SQLControl.EnumDataType.dtString)

                            '.AddField("SyncCreate", UsrprofileCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                            '.AddField("SyncLastUpd", UsrprofileCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)

                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    .AddField("RefID", UsrprofileCont.RefID, SQLControl.EnumDataType.dtString)

                                    If blnFound = True And blnFlag = False Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                                    Else
                                        If blnFound = False Then
                                            .AddField("UserID", UsrprofileCont.UserID, SQLControl.EnumDataType.dtString)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                        End If
                                    End If
                                Case SQLControl.EnumSQLType.stUpdate

                                    .AddField("UserID", UsrprofileCont.UserID, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                            End Select
                            ListSQL.Add(strSQL)


                            .TableName = "USRAPP WITH (ROWLOCK)"
                            .AddField("AccessCode", USRAPP.AccessCode, SQLControl.EnumDataType.dtString)
                            .AddField("IsInherit", USRAPP.IsInherit, SQLControl.EnumDataType.dtNumeric)
                            .AddField("AppID", USRAPP.AppID, SQLControl.EnumDataType.dtNumeric)

                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    'Add
                                    If blnFound = True And blnFlag = False Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                                    Else
                                        If blnFound = False Then

                                            .AddField("UserID", USRAPP.UserID, SQLControl.EnumDataType.dtString)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                        End If
                                    End If
                                Case SQLControl.EnumSQLType.stUpdate
                                    'Edit
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                    "WHERE UserID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                            End Select
                            ListSQL.Add(strSQL)

                        End With
                    End If

                    If ListContEmployeeBranch Is Nothing Then

                    Else
                        If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                            StartSQLControl()

                            If ListContEmployeeBranch.Count > 0 Then
                                With objSQL
                                    .TableName = "EMPBRANCH WITH (ROWLOCK)"
                                    strSQL = BuildDelete(.TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListContEmployeeBranch(0).EmployeeID) & "'")
                                    ListSQL.Add(strSQL)
                                End With
                            End If

                        End If
                        For Each EmployeeBranchCont In ListContEmployeeBranch
                            With objSQL
                                .TableName = "EMPBRANCH WITH (ROWLOCK)"
                                .AddField("InAppt", EmployeeBranchCont.InAppt, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsServer", EmployeeBranchCont.IsServer, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", EmployeeBranchCont.IsHost, SQLControl.EnumDataType.dtNumeric)


                                .AddField("EmployeeID", EmployeeBranchCont.EmployeeID, SQLControl.EnumDataType.dtString)
                                .AddField("LocID", EmployeeBranchCont.LocID, SQLControl.EnumDataType.dtString)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            End With

                            ListSQL.Add(strSQL)
                        Next
                    End If

                    Try
                        'execute
                        objConn.BatchExecute(ListSQL, CommandType.Text)
                        Return True

                    Catch axExecute As Exception
                        Dim sqlStatement As String = " "
                        If objConn.FailedSQLStatement.Count > 0 Then
                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
                        End If

                        message = axExecute.Message.ToString()
                        Log.Notifier.Notify(axExecute)
                        Gibraltar.Agent.Log.Error("eSWISLogic", axExecute.Message & sqlStatement, axExecute.StackTrace)
                        Return False

                        'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                    Finally
                        objSQL.Dispose()
                    End Try
                End If
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                EmployeeCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function


        Public Function InsertEmployee(ByVal EmployeeCont As Profiles.Container.Employee, ByRef message As String, Optional ByVal UsrprofileCont As UserSecurity.Container.UserProfile = Nothing, Optional ByVal USRAPP As UserSecurity.UsrApp = Nothing, Optional ByVal ListContEmployeeBranch As List(Of Profiles.Container.Empbranch) = Nothing) As Boolean
            Return SaveEmployee(EmployeeCont, UsrprofileCont, USRAPP, ListContEmployeeBranch, SQLControl.EnumSQLType.stInsert, message)
        End Function
        Public Function UpdateEmployee(ByVal EmployeeCont As Profiles.Container.Employee, ByRef message As String, Optional ByVal UsrprofileCont As UserSecurity.Container.UserProfile = Nothing, Optional ByVal USRAPP As UserSecurity.UsrApp = Nothing, Optional ByVal ListContEmployeeBranch As List(Of Profiles.Container.Empbranch) = Nothing) As Boolean
            Return SaveEmployee(EmployeeCont, UsrprofileCont, USRAPP, ListContEmployeeBranch, SQLControl.EnumSQLType.stUpdate, message)
        End Function
        'end added

        Public Function IsQOHZero(ByVal locid As String, ByVal itemcode As String, ByVal itemname As String) As Boolean
            Dim strSQL As String

            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim dt As New DataTable
            IsQOHZero = False

            Try
                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
                    strSQL = "select QtyOnHand from itemloc WITH (NOLOCK) where LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, locid) & "' and itemcode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, itemcode) & "' and ItemName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, itemname) & "' "

                    'execute
                    dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, "QtyOnHand"), Data.DataTable)

                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        If dt.Rows(0)("QtyOnHand") <= 0 Then
                            IsQOHZero = True
                        Else
                            IsQOHZero = False
                        End If
                    Else
                        IsQOHZero = False
                    End If
                End If

            Catch exExecute As ApplicationException
                Log.Notifier.Notify(exExecute)
                Gibraltar.Agent.Log.Error("eSWISLogic", exExecute.Message, exExecute.StackTrace)

            Catch exExecute As Exception
                Log.Notifier.Notify(exExecute)
                Gibraltar.Agent.Log.Error("eSWISLogic", exExecute.Message, exExecute.StackTrace)

            Finally

                rdr = Nothing
                dt = Nothing
                EndSQLControl()
                EndConnection()
            End Try

            Return IsQOHZero
        End Function

        Public Overloads Function GetTransID(Optional ByVal TRANSID As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With EmployeeInfo.MyInfo

                    strSQL = "SELECT * FROM CONSIGNLABEL WITH (NOLOCK) "

                    If Not TRANSID Is Nothing And TRANSID <> "" Then strSQL &= " WHERE TRANSID='" & TRANSID & "'"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetMaxSeqNo(Optional ByVal TRANSID As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With EmployeeInfo.MyInfo

                    strSQL = "SELECT ISNULL(max(SeqNo),0) as SeqNo FROM CONSIGNLABEL WITH (NOLOCK) "

                    If Not TRANSID Is Nothing And TRANSID <> "" Then strSQL &= " WHERE TRANSID='" & TRANSID & "'"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Private Function RandomCode(Optional ByVal strInput As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890") As String

            Dim strOutput As String = ""
            Dim rand As New System.Random
            Dim intPlace As Integer

            While strInput.Length > 0

                intPlace = rand.Next(0, strInput.Length)
                strOutput += strInput.Substring(intPlace, 1)
                strInput = strInput.Remove(intPlace, 1)

            End While

            Return strOutput.Substring(0, 6) 'get

        End Function

#End Region

#Region "Export"
        Public Function QueryString(ByVal pageName As String, Optional ByVal ServiceType As String = Nothing, Optional ByVal BatchCode As String = Nothing) As String

            Dim query = ""

            If pageName = "Receiver" Then
                If ServiceType = "COL" Then
                    query = "SELECT '' AS 'WasteID', GeneratorID, SerialNo, WasteCode, WasteComponent, OriginDescription, ISNULL(OriginCode,'') AS OriginCode, ISNULL(cm.CodeDesc, WasteType) AS WasteType, " & _
                        " ISNULL(cp.UOMDesc,WastePackage) AS WastePackage, '' AS OtherPackage, PackagingQty, Qty, PackQty, TreatmentCost, ReceiverID, TransporterID, VehicleID, DriverName, ICNo, TempStorage1, " & _
                        " convert(date, TransportDate,103) AS 'TransportDate', RejectDate, Status, Remark, CountryCode, REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(ErrCode,'1','Generator ID is not found'),'2', " & _
                        "'Transporter ID is not found'),'3','Receiver ID is not found'),'4','Driver ID is not found'),'5','Waste Type is not found'),'6','Waste Packaging is not found'),'0',''),'7','Waste Code is not found'),'X','Serial No is Exist'),'A','Invalid quantity'),'B','Invalid quantity') " & _
                        " as 'Error Description', " & _
                        " CASE WHEN ErrCode = '1' OR ErrCode LIKE '%|1' OR ErrCode LIKE '1|%' OR ErrCode LIKE '%|1|%' THEN '1' ELSE '0' END AS Color1, " & _
                        " CASE WHEN ErrCode = '2' OR ErrCode LIKE '%|2' OR ErrCode LIKE '2|%' OR ErrCode LIKE '%|2|%' THEN '1' ELSE '0' END AS Color15, " & _
                        " CASE WHEN ErrCode = '3' OR ErrCode LIKE '%|3' OR ErrCode LIKE '3|%' OR ErrCode LIKE '%|3|%' THEN '1' ELSE '0' END AS Color14, " & _
                        " CASE WHEN ErrCode = '4' OR ErrCode LIKE '%|4' OR ErrCode LIKE '4|%' OR ErrCode LIKE '%|4|%' THEN '1' ELSE '0' END AS Color17, " & _
                        " CASE WHEN ErrCode = '5' OR ErrCode LIKE '%|5' OR ErrCode LIKE '5|%' OR ErrCode LIKE '%|5|%' THEN '1' ELSE '0' END AS Color7, " & _
                        " CASE WHEN ErrCode = '6' OR ErrCode LIKE '%|6' OR ErrCode LIKE '6|%' OR ErrCode LIKE '%|6|%' THEN '1' ELSE '0' END AS Color8, " & _
                        " CASE WHEN ErrCode = '7' OR ErrCode LIKE '%|7' OR ErrCode LIKE '7|%' OR ErrCode LIKE '%|7|%' THEN '1' ELSE '0' END AS Color3, " & _
                        " CASE WHEN ErrCode = 'X' OR ErrCode LIKE '%|X' OR ErrCode LIKE 'X|%' OR ErrCode LIKE '%|X|%' THEN '1' ELSE '0' END AS Color2, " & _
                        " CASE WHEN ErrCode = 'A' OR ErrCode LIKE '%|A' OR ErrCode LIKE 'A|%' OR ErrCode LIKE '%|A|%' THEN '1' ELSE '0' END AS Color11, " & _
                        " CASE WHEN ErrCode = 'B' OR ErrCode LIKE '%|B' OR ErrCode LIKE 'B|%' OR ErrCode LIKE '%|B|%' THEN '1' ELSE '0' END AS Color10 " & _
                        " FROM ConsignTemp a WITH (NOLOCK) LEFT JOIN CodeMaster cm WITH (NOLOCK) ON a.WasteType=cm.Code AND cm.CodeType='WTY' " & _
                        " LEFT JOIN UOM cp WITH (NOLOCK) ON a.WastePackage=cp.UOMCode AND cp.IsHost=1 WHERE BatchCode='" & BatchCode & "'"
                ElseIf ServiceType = "CLS" Then
                    query = "SELECT UpdateBy AS LoginID, ReceiverID, WasteCode AS 'WasteID', OperationDesc, '' AS OtherOperation, RecPackQty, RcvQty, RcvPackQty, ReceiveDate, RejectDate, ReceiveRemark, " & _
                        " GeneratorID, SerialNo, REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(ErrCode,'1','Generator ID is not found'),'2', " & _
                        "'Transporter ID is not found'),'3','Receiver ID is not found'),'4','Driver''s NRIC is not found'),'5','Waste Type is not found'),'6','Waste Packaging is not found'),'0','') " & _
                        " as 'Error Description', " & _
                        " CASE WHEN ErrCode = '1' OR ErrCode LIKE '%|1' OR ErrCode LIKE '1|%' OR ErrCode LIKE '%|1|%' THEN '1' ELSE '0' END AS Color10, " & _
                        " CASE WHEN ErrCode = '3' OR ErrCode LIKE '%|3' OR ErrCode LIKE '3|%' OR ErrCode LIKE '%|3|%' THEN '1' ELSE '0' END AS Color1, " & _
                        "FROM ConsignTemp WHERE BatchCode='" & BatchCode & "'"
                Else
                    Exit Function
                End If
            End If
            Return query 'get

        End Function
#End Region
    End Class


End Namespace