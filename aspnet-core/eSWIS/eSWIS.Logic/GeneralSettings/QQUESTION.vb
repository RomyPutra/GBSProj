Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace GeneralSettings
#Region "QQUESTION Class"
    Public NotInheritable Class QQUESTION
        Inherits Core.CoreControl
        Private QquestionInfo As QquestionInfo = New QquestionInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal QquestionCont As Container.Qquestion, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If QquestionCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With QquestionInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SPID = '" & QquestionCont.SpID & "'")
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
                                .TableName = "Qquestion"
                                .AddField("Name", QquestionCont.Name, SQLControl.EnumDataType.dtStringN)
                                .AddField("Description", QquestionCont.Description, SQLControl.EnumDataType.dtStringN)
                                .AddField("SubText", QquestionCont.SubText, SQLControl.EnumDataType.dtStringN)
                                .AddField("ControlType", QquestionCont.ControlType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Code", QquestionCont.Code, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RefType", QquestionCont.RefType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RefCode", QquestionCont.RefCode, SQLControl.EnumDataType.dtString)
                                .AddField("SubRefCode", QquestionCont.SubRefCode, SQLControl.EnumDataType.dtString)
                                .AddField("ExtRefCode", QquestionCont.ExtRefCode, SQLControl.EnumDataType.dtString)
                                .AddField("MyProduct", QquestionCont.MyProduct, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CountYN", QquestionCont.CountYN, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", QquestionCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", QquestionCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", QquestionCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", QquestionCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", QquestionCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", QquestionCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncCreate", QquestionCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", QquestionCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", QquestionCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SPID = '" & QquestionCont.SpID & "' AND Code = '" & QquestionCont.Code & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("SPID", QquestionCont.SpID, SQLControl.EnumDataType.dtString)
                                                .AddField("Code", QquestionCont.Code, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SPID = '" & QquestionCont.SpID & "'")
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
                QquestionCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal QquestionCont As Container.Qquestion, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(QquestionCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal QquestionCont As Container.Qquestion, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(QquestionCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal QquestionCont As Container.Qquestion, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If QquestionCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With QquestionInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SPID= '" & QquestionCont.SpID & "' AND Code = '" & QquestionCont.Code & "'")
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
                                strSQL = BuildUpdate(QquestionInfo.MyInfo.TableName, " SET Flag = 0" &
                                " , LastUpdate = '" & QquestionCont.LastUpdate & "' , UpdateBy = '" &
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, QquestionCont.UpdateBy) & "' WHERE " &
                                "SPID = '" & QquestionCont.SpID & "' AND Code = '" & QquestionCont.Code & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(QquestionInfo.MyInfo.TableName, "SPID = '" & QquestionCont.SpID & "' AND Code = '" & QquestionCont.Code & "'")
                            strSQL = BuildDelete(QquestionInfo.MyInfo.TableName, "SPID = '" & QquestionCont.SpID & "' AND Code = '" & QquestionCont.Code & "'")
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
                QquestionCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        Public Overloads Function GetQQUESTION(ByVal SpID As System.String, ByVal Code As System.String) As Container.Qquestion
            Dim rQquestion As Container.Qquestion = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With QquestionInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "SPID = '" & SpID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rQquestion = New Container.Qquestion
                                rQquestion.SpID = drRow.Item("SPID")
                                rQquestion.Code = drRow.Item("Code")
                                rQquestion.Name = drRow.Item("Name")
                                rQquestion.Description = drRow.Item("Description")
                                rQquestion.SubText = drRow.Item("SubText")
                                rQquestion.ControlType = drRow.Item("ControlType")
                                rQquestion.RefType = drRow.Item("RefType")
                                rQquestion.RefCode = drRow.Item("RefCode")
                                rQquestion.SubRefCode = drRow.Item("SubRefCode")
                                rQquestion.ExtRefCode = drRow.Item("ExtRefCode")
                                rQquestion.MyProduct = drRow.Item("MyProduct")
                                rQquestion.CountYN = drRow.Item("CountYN")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rQquestion.CreateDate = drRow.Item("CreateDate")
                                End If
                                rQquestion.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rQquestion.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rQquestion.UpdateBy = drRow.Item("UpdateBy")
                                rQquestion.Active = drRow.Item("Active")
                                rQquestion.rowguid = drRow.Item("rowguid")
                                rQquestion.Status = drRow.Item("Status")
                                rQquestion.SyncCreate = drRow.Item("SyncCreate")
                                rQquestion.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rQquestion.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rQquestion = Nothing
                            End If
                        Else
                            rQquestion = Nothing
                        End If
                    End With
                End If
                Return rQquestion
            Catch ex As Exception
                Throw ex
            Finally
                rQquestion = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetQQUESTION(ByVal SpID As System.String, ByVal Code As System.String, DecendingOrder As Boolean) As List(Of Container.Qquestion)
            Dim rQquestion As Container.Qquestion = Nothing
            Dim lstQquestion As List(Of Container.Qquestion) = New List(Of Container.Qquestion)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With QquestionInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by SpID, Code DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "SpID = '" & SpID & "' AND Code = '" & Code & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rQquestion = New Container.Qquestion
                                rQquestion.SpID = drRow.Item("SpID")
                                rQquestion.Code = drRow.Item("Code")
                                rQquestion.Name = drRow.Item("Name")
                                rQquestion.Description = drRow.Item("Description")
                                rQquestion.SubText = drRow.Item("SubText")
                                rQquestion.ControlType = drRow.Item("ControlType")
                                rQquestion.RefType = drRow.Item("RefType")
                                rQquestion.RefCode = drRow.Item("RefCode")
                                rQquestion.SubRefCode = drRow.Item("SubRefCode")
                                rQquestion.ExtRefCode = drRow.Item("ExtRefCode")
                                rQquestion.MyProduct = drRow.Item("MyProduct")
                                rQquestion.CountYN = drRow.Item("CountYN")
                                rQquestion.CreateBy = drRow.Item("CreateBy")
                                rQquestion.UpdateBy = drRow.Item("UpdateBy")
                                rQquestion.Active = drRow.Item("Active")
                                rQquestion.rowguid = drRow.Item("rowguid")
                                rQquestion.Status = drRow.Item("Status")
                                rQquestion.SyncCreate = drRow.Item("SyncCreate")
                                rQquestion.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rQquestion.LastSyncBy = drRow.Item("LastSyncBy")
                                lstQquestion.Add(rQquestion)
                            Next

                        Else
                            rQquestion = Nothing
                        End If
                        Return lstQquestion
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rQquestion = Nothing
                lstQquestion = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetQQUESTIONList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With QquestionInfo.MyInfo
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

        Public Overloads Function getCode(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With QquestionInfo.MyInfo
                    If SQL = Nothing Or SQL = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                    Else
                        strSQL = "select CodeDesc from  CODEMASTER c inner join  QQUESTION q on c.Code = q.Code where c.Codetype='FAQ'"
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetFAQList(Optional ByVal inactive As String = "1", Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With QquestionInfo.MyInfo

                    strSQL = " select q.SpID,q.Code,c.CodeDesc,q.Name,q.Description,a.Name as NameAnswer,a.Description as DescriptionAnswer" &
                        ",q.CreateBy,q.CreateDate,q.UpdateBy,q.LastUpdate," &
                        "CASE WHEN q.Active = 0 THEN 'In-Active' ELSE 'Active' END as Active " &
                        " from QQUESTION q " &
                        "with (nolock) inner join QANSWER a with (nolock) on q.SpID = a.SpID and q.Code = a.Code " &
                        " inner join CODEMASTER c on q.code=c.code " &
                        " where q.Flag = 1 and c.Codetype='FAQ' and q.Active = '" & inactive & "' "

                    If FieldCond <> "" AndAlso FieldCond IsNot Nothing Then
                        strSQL &= " AND "
                        strSQL &= "q.Code ='" & FieldCond & "'"
                    End If
                    strSQL &= "order by CreateDate DESC"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetFAQView(Optional ByVal filter As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With QquestionInfo.MyInfo

                    strSQL = "SELECT ROW_NUMBER() OVER(ORDER BY CreateDate DESC,Code,SPID, Type DESC) SeqNo, * FROM (" &
                             "SELECT Q.CreateDate,Q.SPID, Q.Code, Q.Description Q, A.Description A FROM QQUESTION Q INNER JOIN QANSWER A ON Q.SPID=A.SPID AND Q.CODE=A.CODE WHERE" &
                             " Q.Flag=1 AND Q.Active=1 AND Q.code <> '0' AND Q.code <> ''"

                    If filter <> "" AndAlso filter IsNot Nothing Then
                        strSQL &= " AND "
                        strSQL &= "Q.Code ='" & filter & "'"
                    End If
                    strSQL &= ") A"
                    strSQL &= " UNPIVOT (Description FOR Type IN (Q,A)) U"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetFAQView2(ByVal WasType As String, ByVal WasCode As String, ByVal Role As String, ByVal BaseID As String, Optional ByVal filter As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With QquestionInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT *, ROW_NUMBER() OVER(ORDER BY CreateDate DESC,WasCode) SeqNo FROM WAC_BASEQUEST WC INNER JOIN CODEMASTER CM WITH(NOLOCK) ON WC.ControlType=CM.Code WHERE " &
                             "CODETYPE='SYS' AND WC.WASCODE = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasCode) & "' " &
                             "AND WC.WasType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasType) & "' " &
                             "And Role = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Role) & "' " &
                             "And BaseID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, BaseID) & "' AND WC.ACTIVE = 1 ORDER BY WC.QuestID"


                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function
        
        Public Overloads Function GetAnswer(ByVal wasCode As String, ByVal wasType As String, ByVal LocID As String, ByVal Role As String, Optional ByVal filter As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With QquestionInfo.MyInfo

                    strSQL = "SELECT Distinct ControlType, LocID, BQ.QuestID, BQ.WasCode, QuestDesc,BQ.WasType," &
                            " STUFF((SELECT '|' + QuestAnswer FROM WAC_BASEQUEST BQ1  WITH(NOLOCK) INNER JOIN WAC_CHARQUEST CQ2 WITH(NOLOCK) ON CQ2.BaseID=BQ1.BaseID AND CQ2.WasCode=BQ1.WasCode AND CQ2.QuestID=BQ1.QuestID  WHERE BQ1.WASCODE= BQ.WASCODE  AND CQ2.LocID = CQ.LocID AND CQ2.QuestID = BQ1.QuestID AND CQ2.QuestID = CQ.QuestID FOR XML PATH ('')), 1, 1, '')" &
                            " As QuestAnswer " &
                            " FROM WAC_BASEQUEST BQ  WITH(NOLOCK) " &
                            " LEFT OUTER JOIN WAC_CHARQUEST CQ WITH(NOLOCK) ON CQ.BaseID=BQ.BaseID AND CQ.WasCode=BQ.WasCode AND CQ.QuestID=BQ.QuestID AND CQ.LocID = '" & LocID & "'" &
                            " WHERE BQ.WASCODE = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, wasCode) & "' AND BQ.WASTYPE = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, wasType) & "' AND BQ.Role = " & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Role) & "" &
                            "Group By CQ.LocID, BQ.QuestID, BQ.WasCode,ControlType, QuestDesc , BQ.BaseID ,CQ.BaseID ,cq.QuestID,cq.QuestAnswer, BQ.WasType " &
                            "Order By BQ.QuestID"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetQuestAnswer(ByVal wasCode As String, ByVal wasType As String, ByVal QuestID As String, ByVal LocID As String, ByVal Role As String, Optional ByVal filter As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With QquestionInfo.MyInfo

                    strSQL = "SELECT Distinct ControlType, LocID, BQ.QuestID, BQ.WasCode, QuestDesc,BQ.WasType," &
                            " STUFF((SELECT '|' + QuestAnswer FROM WAC_BASEQUEST BQ1  WITH(NOLOCK) INNER JOIN WAC_CHARQUEST CQ2 WITH(NOLOCK) ON " &
                            " CQ2.BaseID=BQ1.BaseID AND CQ2.WasCode=BQ1.WasCode AND BQ1.WASTYPE=BQ.WASTYPE AND CQ2.QuestID=BQ1.QuestID" &
                            " WHERE BQ1.WASCODE= BQ.WASCODE  AND CQ2.LocID = CQ.LocID AND CQ2.QuestID = BQ1.QuestID AND CQ2.QuestID = CQ.QuestID FOR XML PATH ('')), 1, 1, '')" &
                            " As QuestAnswer " &
                            " FROM WAC_BASEQUEST BQ  WITH(NOLOCK) " &
                            " LEFT OUTER JOIN WAC_CHARQUEST CQ WITH(NOLOCK) ON CQ.BaseID=BQ.BaseID AND CQ.WasCode=BQ.WasCode AND CQ.QuestID=BQ.QuestID AND CQ.LocID = '" & LocID & "'" &
                            " WHERE BQ.WASCODE = '" & wasCode & "' AND BQ.WASTYPE = '" & wasType & "' AND BQ.QuestID='" & QuestID & "' AND BQ.Role = " & Role & "" &
                            " Group By CQ.LocID, BQ.QuestID, BQ.WasCode,ControlType, QuestDesc , BQ.BaseID ,CQ.BaseID ,cq.QuestID,cq.QuestAnswer, BQ.WasType " &
                            " Order By BQ.QuestID"

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
#Region "Qquestion Container"
        Public Class Qquestion_FieldName
            Public SpID As System.String = "SPID"
            Public Code As System.String = "Code"
            Public Name As System.String = "Name"
            Public Description As System.String = "Description"
            Public SubText As System.String = "SubText"
            Public ControlType As System.String = "ControlType"
            Public RefType As System.String = "RefType"
            Public RefCode As System.String = "RefCode"
            Public SubRefCode As System.String = "SubRefCode"
            Public ExtRefCode As System.String = "ExtRefCode"
            Public MyProduct As System.String = "MyProduct"
            Public CountYN As System.String = "CountYN"
            Public CreateDate As System.String = "CreateDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public Active As System.String = "Active"
            Public Flag As System.String = "Flag"
            Public rowguid As System.String = "rowguid"
            Public Status As System.String = "Status"
            Public SyncCreate As System.String = "SyncCreate"
            Public SyncLastUpd As System.String = "SyncLastUpd"
            Public LastSyncBy As System.String = "LastSyncBy"
        End Class

        Public Class Qquestion
            Protected _SpID As System.String
            Protected _Code As System.String
            Private _Name As System.String
            Private _Description As System.String
            Private _SubText As System.String
            Private _ControlType As System.Byte
            Private _RefType As System.Byte
            Private _RefCode As System.String
            Private _SubRefCode As System.String
            Private _ExtRefCode As System.String
            Private _MyProduct As System.Byte
            Private _CountYN As System.Byte
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
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
            Public Property ControlType As System.Byte
                Get
                    Return _ControlType
                End Get
                Set(ByVal Value As System.Byte)
                    _ControlType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RefType As System.Byte
                Get
                    Return _RefType
                End Get
                Set(ByVal Value As System.Byte)
                    _RefType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RefCode As System.String
                Get
                    Return _RefCode
                End Get
                Set(ByVal Value As System.String)
                    _RefCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SubRefCode As System.String
                Get
                    Return _SubRefCode
                End Get
                Set(ByVal Value As System.String)
                    _SubRefCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ExtRefCode As System.String
                Get
                    Return _ExtRefCode
                End Get
                Set(ByVal Value As System.String)
                    _ExtRefCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property MyProduct As System.Byte
                Get
                    Return _MyProduct
                End Get
                Set(ByVal Value As System.Byte)
                    _MyProduct = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CountYN As System.Byte
                Get
                    Return _CountYN
                End Get
                Set(ByVal Value As System.Byte)
                    _CountYN = Value
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
#Region "Qquestion Info"
    Public Class QquestionInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "SPID,Code,Name,Description,SubText,ControlType,RefType,RefCode,SubRefCode,ExtRefCode,MyProduct,CountYN,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Flag,rowguid,Status,SyncCreate,SyncLastUpd,LastSyncBy"
                .CheckFields = "ControlType,RefType,MyProduct,CountYN,Active,Flag,Status"
                .TableName = "Qquestion"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "SPID,Code,Name,Description,SubText,ControlType,RefType,RefCode,SubRefCode,ExtRefCode,MyProduct,CountYN,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Flag,rowguid,Status,SyncCreate,SyncLastUpd,LastSyncBy"
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
#Region "QQUESTION Scheme"
    Public Class QQUESTIONScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "SPID"
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
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Name"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Description"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SubText"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ControlType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RefType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RefCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "SubRefCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ExtRefCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "MyProduct"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "CountYN"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)

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

        Public ReadOnly Property Name As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property Description As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property SubText As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property ControlType As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property RefType As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property RefCode As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property SubRefCode As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property ExtRefCode As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property MyProduct As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property CountYN As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace