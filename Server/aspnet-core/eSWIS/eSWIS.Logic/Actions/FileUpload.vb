Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
    Public NotInheritable Class FileUpload
        Inherits Core.CoreControl
        Private FileUploadInfo As FileUploadInfo = New FileUploadInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

#End Region

#Region "Data Selection"
        Public Overloads Function GetTotalUploadedRecord(ByVal TableName As String, ByVal BatchCode As String) As Integer
            If StartConnection() = True Then
                With FileUploadInfo.MyInfo

                    strSQL = "select sum (CntRecord) as cntrecord from (SELECT COUNT(BatchCode) AS CntRecord FROM " & TableName & " WHERE ErrCode='0' AND BatchCode='" & BatchCode & "' GROUP BY BatchCode  union select 0 as cntrecord) as jo"

                    Dim dt As DataTable = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 And Not IsDBNull(dt.Rows(0)("CntRecord")) Then
                        Return CInt(dt.Rows(0)("CntRecord"))
                    End If
                End With

            Else
                Return 0
            End If
            EndConnection()
            Return 0
        End Function

        Public Overloads Function GetTotalFailUploadedRecord(ByVal TableName As String, ByVal BatchCode As String) As Integer
            If StartConnection() = True Then
                With FileUploadInfo.MyInfo

                    strSQL = "select sum (CntRecord) as cntrecord from (SELECT COUNT(BatchCode) AS CntRecord FROM " & TableName & " WHERE ErrCode<>'0' AND BatchCode='" & BatchCode & "' GROUP BY BatchCode  union select 0 as cntrecord) as jo"

                    Dim dt As DataTable = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 And Not IsDBNull(dt.Rows(0)("CntRecord")) Then
                        Return CInt(dt.Rows(0)("CntRecord"))
                    End If
                End With

            Else
                Return 0
            End If
            EndConnection()
            Return 0
        End Function

#End Region
    End Class


    Namespace Container
#Region "Class Container"
        Public Class FileUpload


        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class FileUploadInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                
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
    Public Class FileUploadScheme
        Inherits Core.SchemeBase

    End Class
#End Region

End Namespace
