Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Rebex.Net
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.Xsl
Imports SEAL.Data
Imports ABS.Navitaire
Imports ABS.Navitaire.BookingManager
Imports ABS.Logic.GroupBooking.Booking

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ProcessService
    Inherits System.Web.Services.WebService
    Private Const AppKey As String = "395a58f9b17b49a5a2ece2f76a393860"
    Private _AppPassKey As String

    Private _proFile, _proMsg As String
    Private objSQL As SEAL.Data.SQLControl
    Private objDCom As SEAL.Data.DataAccess
    Private _aryFile As ArrayList

    <WebMethod()> _
    Public Function AppPassKey(ByVal value As String) As Boolean
        Try
            StartSQLControl()
            value = objSQL.ParseValue(SQLControl.EnumDataType.dtString, value)
            If value = AppKey Then
                _AppPassKey = value
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            _proMsg = ex.Message
            Return "AppPassKey Process error: " & _proMsg
        Finally
            EndSQLControl()
        End Try

    End Function
    ''' <summary>
    ''' Validating Call with AppKey Before Execution
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Jasus</remarks>
    Private Function ValidateCall() As Boolean
        If _AppPassKey = AppKey Then
            Return True
        Else
            Return False
        End If
    End Function

    'added by ektee, test connection
    <WebMethod()> _
    Public Function ConnectionTest() As String
        Dim dtAgent As DataTable = Nothing
        Try
            dtAgent = GetAllAgent()
            If Not dtAgent Is Nothing AndAlso dtAgent.Rows.Count > 0 Then
                Return "Agent Found: " & dtAgent.Rows.Count
            End If
            Return "Agent not found"
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function

    <WebMethod()> _
    Public Function CreditShellProcess() As String
        Dim messageBody As String = vbNullString
        Dim BatchNo As String = vbNullString
        Dim dtContact As DataTable = Nothing
        Try
            'Your processing function here?
            'ProcessingFile()
            BatchNo = GenerateMsgID()
            dtContact = GetCreditShellContact()
            If dtContact Is Nothing = False Then
                If dtContact.Rows.Count > 0 Then
                    For i = 0 To dtContact.Rows.Count - 1
                        Dim objEmailInfo As New EmailInboxInfo
                        objEmailInfo.BatchNo = BatchNo
                        objEmailInfo.RecordLocator = dtContact.Rows(i)("RecordLocator").ToString
                        objEmailInfo.CreatedDate = dtContact.Rows(i)("CreatedDate")
                        objEmailInfo.EmailAddress = dtContact.Rows(i)("EmailAddress").ToString
                        objEmailInfo.AccountNumber = dtContact.Rows(i)("AccountNumber").ToString
                        objEmailInfo.AgentName = dtContact.Rows(i)("AccountNumber").ToString
                        objEmailInfo.DepartmentCode = dtContact.Rows(i)("DepartmentCode").ToString
                        objEmailInfo.LocationCode = dtContact.Rows(i)("LocationCode").ToString
                        objEmailInfo.ExpiryDate = dtContact.Rows(i)("ExpiryDate")
                        objEmailInfo.EmailType = 0

                        messageBody = GenerateEmailBody(objEmailInfo)
                        If messageBody Is Nothing = True Then
                            Return "Error with EmailBody"
                        Else
                            objEmailInfo.EmailBody = messageBody
                            If SaveIntoEmail(objEmailInfo) = True Then
                                UpdateCreditShellContact(objEmailInfo.RecordLocator, objEmailInfo.BatchNo)
                            End If

                        End If
                    Next
                End If
            End If
            Return "Process completed with no error"
            Dispose()
        Catch ex As Exception
            _proMsg = ex.Message
            Return "Process error: " & _proMsg
        End Try
    End Function

    <WebMethod()> _
    Public Function CreditShellProcessByPNR(ByVal RecordLocator As String, ByVal BatchNo As String) As String
        Dim messageBody As String = vbNullString
        'Dim BatchNo As String = vbNullString
        Dim dtContact As DataTable = Nothing
        Try
            'Your processing function here?
            'ProcessingFile()
            'BatchNo = GenerateMsgID()
            StartConnection()
            StartSQLControl()
            RecordLocator = objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecordLocator)
            BatchNo = objSQL.ParseValue(SQLControl.EnumDataType.dtString, BatchNo)
            dtContact = GetCreditShellContact(RecordLocator)
            If dtContact Is Nothing = False Then
                If dtContact.Rows.Count > 0 Then
                    For i = 0 To dtContact.Rows.Count - 1
                        Dim objEmailInfo As New EmailInboxInfo
                        objEmailInfo.BatchNo = BatchNo
                        objEmailInfo.RecordLocator = dtContact.Rows(i)("RecordLocator").ToString
                        objEmailInfo.CreatedDate = dtContact.Rows(i)("CreatedDate")
                        objEmailInfo.EmailAddress = dtContact.Rows(i)("EmailAddress").ToString
                        objEmailInfo.AccountNumber = dtContact.Rows(i)("AccountNumber").ToString
                        objEmailInfo.AgentName = dtContact.Rows(i)("AccountNumber").ToString
                        objEmailInfo.DepartmentCode = dtContact.Rows(i)("DepartmentCode").ToString
                        objEmailInfo.LocationCode = dtContact.Rows(i)("LocationCode").ToString
                        objEmailInfo.ExpiryDate = dtContact.Rows(i)("ExpiryDate")
                        objEmailInfo.EmailType = 0

                        messageBody = GenerateEmailBody(objEmailInfo)
                        If messageBody Is Nothing = True Then
                            Return "Error with EmailBody"
                        Else
                            objEmailInfo.EmailBody = messageBody
                            If SaveIntoEmail(objEmailInfo) = True Then
                                UpdateCreditShellContact(objEmailInfo.RecordLocator, objEmailInfo.BatchNo)
                            End If

                        End If
                    Next
                End If
            End If

            Return "Process completed with no error"
            Dispose()
        Catch ex As Exception
            _proMsg = ex.Message
            Return "CreditShell Process error: " & _proMsg
        Finally
            EndConnection()
            EndSQLControl()
        End Try
    End Function

    Public Function GenerateEmailBody(ByRef EmailInfo As EmailInboxInfo) As String
        Dim vMainPath As String
        Dim vEmailTemplate As String
        'Dim UrlAccessPath As String = ConfigurationManager.AppSettings("EM:UrlWithAccessToken").ToString()
        Dim AccessTokelink As String = Nothing
        'objProcess.ConnectionString = ConfigurationManager.AppSettings("DBConnection").ToString()
        'vMainPath = ConfigurationManager.AppSettings("EM:TemplatePath")

        '  Dim Directory As New System.IO.DirectoryInfo("D:\Projects\AA\AAEmailService(Production)\AAEmailService(Production)\AirAsiaEmailService\Template\AA_CREDITSHELL.XSLT")


        'vMainPath = "D:\Projects\AA\AAEmailService(Production)\AAEmailService(Production)\AirAsiaEmailService\Template"
        vMainPath = My.Settings.EmailTemplatePath
        vEmailTemplate = Nothing
        'Check whether the file exists

        Try
            Select Case EmailInfo.EmailType
                'If Directory.Exists = True Then
                Case 0
                    vEmailTemplate = "AA_CREDITSHELL.XSLT"
                Case 1
                    vEmailTemplate = "AA_AGENTCONFIRMATION.XSLT" ' will return MyFileName.txt

                Case 2
                    vEmailTemplate = "AA_AGENTPASSWORDRETRIEVAL.XSLT" ' will return MyFileName.txt
                Case 3
                    vEmailTemplate = "AA_PAYMENTREMINDER.XSLT"
                Case 4
                    vEmailTemplate = "AA_PAYMENTCONFIRMATION.XSLT"
                Case 5
                    vEmailTemplate = "AA_FULLPAYMENTCONFIRMATION.XSLT"
                Case 6
                    vEmailTemplate = "AA_PASSENGERDETAILREMINDER.XSLT"
                Case 7
                    vEmailTemplate = "AA_PAYMENTREMINDERFORPNR.XSLT"
            End Select

            If vEmailTemplate Is Nothing = False Then
                EmailInfo.EmailTemplate = vEmailTemplate
            End If
            'Else
            'vEmailTemplate = String.Empty
            'vMainPath = String.Empty
            'End If
            'vEmailTemplate = "D7_CreditShell.xslt"

            Dim objxslt As New XslCompiledTransform()
            If IO.File.Exists(Path.Combine(vMainPath, vEmailTemplate)) = False Then
                Return String.Empty
                Exit Function
            End If
            objxslt.Load(Path.Combine(vMainPath, vEmailTemplate))
            'objxslt.Load(Path.GetFullPath(Directory.FullName))
            Dim xmldoc As New XmlDocument
            xmldoc.AppendChild(xmldoc.CreateElement("DocumentRoot"))
            Dim xpathnav As XPathNavigator = xmldoc.CreateNavigator()
            Dim xslarg As New XsltArgumentList
            Dim emailbuilder As New StringBuilder
            Dim xmlwriter As New XmlTextWriter(New System.IO.StringWriter(emailbuilder))
            objxslt.Transform(xpathnav, xslarg, xmlwriter, Nothing)
            Dim bodytext As String
            Dim xemaildoc As New XmlDocument
            xemaildoc.LoadXml(emailbuilder.ToString())
            Dim bodynode As XmlNode


            bodynode = xemaildoc.SelectSingleNode("//body")
            bodytext = bodynode.InnerXml
            If bodytext.Length > 0 Then
                Select Case EmailInfo.EmailType
                    Case 0  'CREDITSHELL
                        bodytext = bodytext.Replace("{PNR}", EmailInfo.RecordLocator)
                        bodytext = bodytext.Replace("{ExpiryDate}", Format(EmailInfo.ExpiryDate, "dd MMMM yyyy HH:mm"))
                    Case 1  'REGISTRATION
                        bodytext = bodytext.Replace("{Title}", EmailInfo.Title)
                        bodytext = bodytext.Replace("{FirstName}", EmailInfo.FirstName)
                        bodytext = bodytext.Replace("{LastName}", EmailInfo.LastName)
                        bodytext = bodytext.Replace("{Username}", EmailInfo.UserName)
                        bodytext = bodytext.Replace("{Password}", EmailInfo.EmailPassword)
                    Case 2  'FORGET PASSWORD
                        bodytext = bodytext.Replace("{Title}", EmailInfo.Title)
                        bodytext = bodytext.Replace("{FirstName}", EmailInfo.FirstName)
                        bodytext = bodytext.Replace("{LastName}", EmailInfo.LastName)
                        bodytext = bodytext.Replace("{Username}", EmailInfo.UserName)
                        bodytext = bodytext.Replace("{Password}", EmailInfo.EmailPassword)
                    Case 3  'PAYMENT REMINDER
                        bodytext = bodytext.Replace("{Title}", EmailInfo.Title)
                        bodytext = bodytext.Replace("{FirstName}", EmailInfo.FirstName)
                        bodytext = bodytext.Replace("{LastName}", EmailInfo.LastName)
                        bodytext = bodytext.Replace("{TransID}", EmailInfo.BookingID)
                        bodytext = bodytext.Replace("{Currency}", EmailInfo.Currency)
                        bodytext = bodytext.Replace("{PNR}", EmailInfo.RecordLocator)
                        bodytext = bodytext.Replace("{BalanceDue}", Format(EmailInfo.BalanceDue, "n2"))
                        bodytext = bodytext.Replace("{ExpiryDate}", Format(EmailInfo.ExpiryDate, "dd MMMM yyyy HH:mm") & " GMT +8")
                        bodytext = bodytext.Replace("{MinPayment}", Format(EmailInfo.NextPaymentAmt, "n2"))
                    Case 4 'PAYMENTCONFIRMATION
                        bodytext = bodytext.Replace("{Title}", EmailInfo.Title)
                        bodytext = bodytext.Replace("{FirstName}", EmailInfo.FirstName)
                        bodytext = bodytext.Replace("{LastName}", EmailInfo.LastName)
                        bodytext = bodytext.Replace("{TransID}", EmailInfo.BookingID)
                        bodytext = bodytext.Replace("{Currency}", EmailInfo.Currency)
                        bodytext = bodytext.Replace("{PaymentAmt}", Format(EmailInfo.PaymentAmt, "n2"))
                        bodytext = bodytext.Replace("{BalanceDue}", Format(EmailInfo.BalanceDue, "n2"))
                        bodytext = bodytext.Replace("{ExpiryDate}", Format(EmailInfo.ExpiryDate, "dd MMMM yyyy HH:mm") & " GMT +8")
                        bodytext = bodytext.Replace("{MinPayment}", Format(EmailInfo.NextPaymentAmt, "n2"))
                        bodytext = bodytext.Replace("{PNR}", EmailInfo.RecordLocator)
                    Case 5 'FULLPAYMENTCONFIRMATION
                        bodytext = bodytext.Replace("{Title}", EmailInfo.Title)
                        bodytext = bodytext.Replace("{FirstName}", EmailInfo.FirstName)
                        bodytext = bodytext.Replace("{LastName}", EmailInfo.LastName)
                        bodytext = bodytext.Replace("{TransID}", EmailInfo.BookingID)
                        bodytext = bodytext.Replace("{PNR}", EmailInfo.RecordLocator)
                    Case 6 'PASSENGER DETAIL REMINDER
                        bodytext = bodytext.Replace("{Title}", EmailInfo.Title)
                        bodytext = bodytext.Replace("{FirstName}", EmailInfo.FirstName)
                        bodytext = bodytext.Replace("{LastName}", EmailInfo.LastName)
                        bodytext = bodytext.Replace("{TransID}", EmailInfo.BookingID)
                        bodytext = bodytext.Replace("{PNR}", EmailInfo.RecordLocator)
                        bodytext = bodytext.Replace("{ExpiryDate}", Format(EmailInfo.ExpiryDate, "dd MMMM yyyy HH:mm") & " GMT +8")

                    Case 7 'PAYMENT REMINDER WITH PNR
                        bodytext = bodytext.Replace("{Title}", EmailInfo.Title)
                        bodytext = bodytext.Replace("{FirstName}", EmailInfo.FirstName)
                        bodytext = bodytext.Replace("{LastName}", EmailInfo.LastName)
                        bodytext = bodytext.Replace("{TransID}", EmailInfo.BookingID)
                        bodytext = bodytext.Replace("{BalanceDue}", Format(EmailInfo.BalanceDue, "n2"))
                        bodytext = bodytext.Replace("{ExpiryDate}", Format(EmailInfo.ExpiryDate, "dd MMMM yyyy HH:mm") & " GMT +8")
                        bodytext = bodytext.Replace("{Currency}", EmailInfo.Currency)
                        bodytext = bodytext.Replace("{PNR}", EmailInfo.RecordLocator)
                        bodytext = bodytext.Replace("{MinPayment}", Format(EmailInfo.NextPaymentAmt, "n2"))
                        'If EmailInfo.NextPaymentAmt > 0 Then
                        '    bodytext = bodytext.Replace("{Remark}", "with minimum payment amount of <b>" & EmailInfo.Currency & " " & Format(EmailInfo.NextPaymentAmt, "n2") & "</b>")
                        'End If
                End Select
            End If

            Dim linkAgent As String = ConfigurationManager.AppSettings("linkAgent").ToString()
            Dim linkFacebook As String = ConfigurationManager.AppSettings("linkFacebook").ToString()
            Dim linkTwitter As String = ConfigurationManager.AppSettings("linkTwitter").ToString()
            Dim linkMembership As String = ConfigurationManager.AppSettings("linkMembership").ToString()
            Dim linkAsk As String = ConfigurationManager.AppSettings("linkAsk").ToString()

            bodytext = bodytext.Replace("linkAgentHref", linkAgent)
            bodytext = bodytext.Replace("linkFacebookHref", linkFacebook)
            bodytext = bodytext.Replace("linkTwitterHref", linkTwitter)
            bodytext = bodytext.Replace("linkMembershipHref", linkMembership)
            bodytext = bodytext.Replace("linkAskHref", linkAsk)

            'bodytext = bodytext.Replace("{Copyright}", "&#169;")
            Return bodytext
        Catch ex As Exception
            _proMsg = ex.Message
            Return Nothing
        End Try
    End Function

    Public Overloads Function SaveIntoEmail(ByRef EmailInfo As EmailInboxInfo) As Boolean
        Dim rValue As Boolean
        Dim arrList As New ArrayList
        Dim strSQL As String = vbNullString
        Dim MsgID As String = Nothing
        Dim dtCheckExist As DataTable = Nothing
        MsgID = GenerateMsgID()

        objSQL.ClearFields()
        objSQL.ClearCondtions()
        arrList.Clear()
        With objSQL
            .AddField("BatchNo", EmailInfo.BatchNo, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone)
            .AddField("RecordLocator", EmailInfo.RecordLocator, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone)
            '.AddField("QueueCode", pEmailTokenInfo.QueueCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone)
            .AddField("CreatedDate", EmailInfo.CreatedDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone)
            .AddField("EmailType", EmailInfo.EmailType, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone)
            .AddField("EmailAddress", EmailInfo.EmailAddress, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone)
            .AddField("ExpiryDate", EmailInfo.ExpiryDate, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNone)
            .AddField("MsgID", MsgID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone)
            .AddField("AgentID", EmailInfo.AgentID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone)
            .AddField("EmailBody", EmailInfo.EmailBody, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone)
            .AddField("EmailTemplate", EmailInfo.EmailTemplate, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone)
            If EmailInfo.EmailType = 3 Or EmailInfo.EmailType = 4 Or EmailInfo.EmailType = 5 Or EmailInfo.EmailType = 6 Then
                .AddField("TransID", EmailInfo.BookingID, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone)
                If EmailInfo.EmailType = 3 Or EmailInfo.EmailType = 4 Or EmailInfo.EmailType = 5 Then
                    .AddField("Currency", EmailInfo.Currency, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cNone)
                    .AddField("BalanceDue", EmailInfo.BalanceDue, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone)
                    .AddField("PaymentAmt", EmailInfo.PaymentAmt, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone)
                    .AddField("TransTotalAmt", EmailInfo.TransTotalAmt, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cNone)
                End If
            End If

            If EmailInfo.EmailType = 0 Then
                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert, "CREDITSHELL_EMAIL", vbNullString)
            Else
                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert, "AG_EMAIL", vbNullString)
            End If
        End With
        strSQL = strSQL.Replace("{Copyright}", "&copy;")
        arrList.Add(strSQL)


        rValue = objDCom.BatchExecute(arrList, CommandType.Text, True, False)
        Return rValue
    End Function

    Public Function GetCreditShellContact(Optional ByVal RecordLocator As String = "") As DataTable

        Dim dtDetails As DataTable = Nothing
        Dim strCond As String = String.Empty
        Dim strSQL As String

        Try
            StartConnection2()
            StartSQLControl()
            RecordLocator = objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecordLocator)
            If RecordLocator <> "" Then
                RecordLocator = " AND RecordLocator='" & RecordLocator & "'"
            End If
            strSQL = "SELECT RecordLocator, CreatedDate, EmailAddress, AccountNumber, AgentName, DepartmentCode, LocationCode, ExpiryDate " & _
                     "FROM CREDITSHELL_CONTACT WHERE PROCESSBATCH= '' AND ISNULL(Processdate,'')='' " & _
                     RecordLocator & _
                     "GROUP BY RecordLocator, CreatedDate, EmailAddress, AccountNumber, AgentName, DepartmentCode, LocationCode, ExpiryDate"
            dtDetails = objDCom.Execute(strSQL, CommandType.Text)

            If dtDetails Is Nothing = False Then
                If dtDetails.Rows.Count > 0 Then
                    Return dtDetails
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Catch ex As Exception
            _proMsg = ex.Message
            Return Nothing
        Finally
            EndSQLControl()
            EndConnection()
        End Try
    End Function

    'added by ketee, test connection
    Public Function GetConnection(ByRef Value As String) As String
        Try
            StartSQLControl()
            Value = objSQL.ParseValue(SQLControl.EnumDataType.dtString, Value)
            If ValidateCall() Then
                Value = ""
            End If
            Return String.Empty
        Catch ex As Exception
            _proMsg = ex.Message
        Finally
            EndSQLControl()
        End Try
        
    End Function

    Public Sub UpdateCreditShellContact(ByVal RecordLocator As String, ByVal Batch As String)
        Dim dt As DataTable = Nothing
        'Dim strCond As String = String.Empty
        Dim strSQL As String
        Try
            StartConnection()
            StartSQLControl()
            RecordLocator = objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecordLocator)
            Batch = objSQL.ParseValue(SQLControl.EnumDataType.dtString, Batch)

            strSQL = "UPDATE CREDITSHELL_CONTACT SET BatchDate=getdate(), ProcessBatch = " & _
                     Batch & " , ProcessDate=GETDATE() WHERE RecordLocator = '" & RecordLocator & _
                     "' AND ProcessBatch='' AND  ISNULL(PROCESSDATE,'')=''  "
            dt = objDCom.Execute(strSQL, CommandType.Text)

        Catch ex As Exception
            _proMsg = ex.Message
        Finally
            EndConnection()
            EndSQLControl()
        End Try
    End Sub

    <WebMethod()> _
    Public Function GroupBookingEmailing(ByVal EmailType As Integer, ByVal AgentID As String, _
                                         ByVal EmailAddress As String, ByVal TransID As String, ByVal PaymentAmt As Double, _
                                         ByVal RecordLocator As String) As String
        Dim messageBody As String = vbNullString
        Dim BatchNo As String = vbNullString
        Dim dtContact As DataTable = Nothing
        Dim dtTransDtl As DataTable = Nothing

        Try
            StartSQLControl()
            AgentID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID)
            TransID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, TransID)
            EmailAddress = objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmailAddress)
            RecordLocator = objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecordLocator)

            BatchNo = GenerateMsgID()
            dtContact = GetAgentContact(AgentID)
            If dtContact Is Nothing Then
                dtContact = GetEmailContact(EmailAddress)
            End If
            If dtContact Is Nothing = False Then
                If dtContact.Rows.Count > 0 Then
                    For i = 0 To dtContact.Rows.Count - 1
                        Dim objEmailInfo As New EmailInboxInfo
                        objEmailInfo.BatchNo = BatchNo
                        objEmailInfo.AgentID = AgentID
                        objEmailInfo.UserName = dtContact.Rows(i)("Username").ToString
                        objEmailInfo.EmailPassword = dtContact.Rows(i)("Password").ToString
                        objEmailInfo.Title = dtContact.Rows(i)("Title").ToString
                        objEmailInfo.FirstName = dtContact.Rows(i)("ContactFirstName").ToString
                        objEmailInfo.LastName = dtContact.Rows(i)("ContactLastName").ToString
                        'objEmailInfo.EmailAddress = dtContact.Rows(i)("Email").ToString
                        objEmailInfo.EmailAddress = EmailAddress
                        '0=CDS, 1=Register, 2=Password, 3=PaymentReminder , 4=PaymentConfirmation, 5=FullPaymentConfirmation 6=PassengerDetailReminder
                        objEmailInfo.EmailType = EmailType

                        If EmailType = 3 Or EmailType = 4 Or EmailType = 5 Or EmailType = 6 Then
                            objEmailInfo.BookingID = TransID
                            If EmailType = 5 Then
                                dtTransDtl = GetBookingDetailsByTransID(TransID)
                                If Not dtTransDtl Is Nothing Then
                                    For Each dtRow In dtTransDtl.Rows
                                        If (objEmailInfo.RecordLocator <> "") Then
                                            objEmailInfo.RecordLocator &= " "
                                        End If
                                        objEmailInfo.RecordLocator &= dtRow("RecordLocator")
                                        'added by ketee, 20170419, add to display expiry date
                                        objEmailInfo.ExpiryDate = dtTransDtl.Rows(i)("ExpiryDate").ToString
                                    Next
                                End If
                            Else
                                objEmailInfo.RecordLocator = RecordLocator
                            End If
                            If EmailType = 3 Or EmailType = 4 Or EmailType = 5 Then
                                dtTransDtl = GetPaymentInfo(TransID, AgentID)
                                objEmailInfo.ExpiryDate = dtTransDtl.Rows(i)("ExpiryDate").ToString
                                objEmailInfo.Currency = dtTransDtl.Rows(i)("Currency").ToString
                                objEmailInfo.PaymentAmt = PaymentAmt
                                objEmailInfo.CollectedAmt = dtTransDtl.Rows(i)("CollectedAmt")
                                objEmailInfo.TransTotalAmt = dtTransDtl.Rows(i)("TransTotalAmt")
                                objEmailInfo.BalanceDue = objEmailInfo.TransTotalAmt - objEmailInfo.CollectedAmt
                            End If
                        ElseIf EmailType = 7 Then
                            objEmailInfo.BookingID = TransID
                            dtTransDtl = GetBookingDetailsByPNR(TransID, RecordLocator)
                            If Not dtTransDtl Is Nothing Then
                                objEmailInfo.ExpiryDate = dtTransDtl.Rows(0)("NextDueDate").ToString
                                objEmailInfo.Currency = dtTransDtl.Rows(0)("Currency").ToString
                                objEmailInfo.PaymentAmt = PaymentAmt
                                objEmailInfo.CollectedAmt = dtTransDtl.Rows(0)("DetailCollectedAmt")
                                objEmailInfo.TransTotalAmt = dtTransDtl.Rows(0)("LineTotal")
                                objEmailInfo.BalanceDue = objEmailInfo.TransTotalAmt - objEmailInfo.CollectedAmt
                                objEmailInfo.NextPaymentAmt = dtTransDtl.Rows(0)("NextDueAmount")
                                objEmailInfo.RecordLocator = RecordLocator
                            Else
                                Return ""
                            End If
                        End If

                        messageBody = GenerateEmailBody(objEmailInfo)
                        If messageBody Is Nothing = True Then
                            Return "Error with EmailBody"
                        Else
                            objEmailInfo.EmailBody = messageBody
                            If SaveIntoEmail(objEmailInfo) = True Then

                            End If

                        End If
                    Next
                End If
            End If
            Return ""
        Catch ex As Exception
            _proMsg = ex.Message
            Return "Process error: " & _proMsg
        Finally
            EndConnection()
            EndSQLControl()
        End Try
    End Function

    <WebMethod()> _
    Public Function GroupBookingEmailingMultiPNR(ByVal EmailType As Integer, ByVal AgentID As String, _
                                                 ByVal EmailAddress As String, ByVal TransID As String, ByVal PaymentAmt As Double, _
                                                 ByVal RecordLocator As String) As String
        Dim messageBody As String = vbNullString
        Dim BatchNo As String = vbNullString
        Dim dtContact As DataTable = Nothing
        Dim dtTransDtl As DataTable = Nothing

        Try
            StartSQLControl()
            AgentID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID)
            TransID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, TransID)
            EmailAddress = objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmailAddress)
            RecordLocator = objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecordLocator)

            BatchNo = GenerateMsgID()
            dtContact = GetAgentContact(AgentID)
            If dtContact Is Nothing Then
                dtContact = GetEmailContact(EmailAddress)
            End If
            If dtContact Is Nothing = False Then
                If dtContact.Rows.Count > 0 Then
                    For i = 0 To dtContact.Rows.Count - 1
                        Dim objEmailInfo As New EmailInboxInfo
                        objEmailInfo.BatchNo = BatchNo
                        objEmailInfo.AgentID = AgentID
                        objEmailInfo.UserName = dtContact.Rows(i)("Username").ToString
                        objEmailInfo.EmailPassword = dtContact.Rows(i)("Password").ToString
                        objEmailInfo.Title = dtContact.Rows(i)("Title").ToString
                        objEmailInfo.FirstName = dtContact.Rows(i)("ContactFirstName").ToString
                        objEmailInfo.LastName = dtContact.Rows(i)("ContactLastName").ToString
                        objEmailInfo.EmailAddress = dtContact.Rows(i)("Email").ToString
                        objEmailInfo.EmailAddress = "diana.huang8808@gmail.com"
                        'objEmailInfo.EmailAddress = "ketee.vit@gmail.com"
                        '0=CDS, 1=Register, 2=Password, 3=PaymentReminder , 4=PaymentConfirmation, 5=FullPaymentConfirmation 6=PassengerDetailReminder
                        objEmailInfo.EmailType = EmailType

                        If EmailType = 3 Or EmailType = 4 Or EmailType = 5 Or EmailType = 6 Then
                            objEmailInfo.BookingID = TransID
                            dtTransDtl = GetBookingDetailsByTransID(TransID)
                            If Not dtTransDtl Is Nothing Then
                                For Each dtRow In dtTransDtl.Rows
                                    If (objEmailInfo.RecordLocator <> "") Then
                                        objEmailInfo.RecordLocator &= " "
                                    End If
                                    objEmailInfo.RecordLocator &= dtRow("RecordLocator")
                                Next
                            End If

                            If EmailType = 3 Or EmailType = 4 Or EmailType = 5 Then
                                dtTransDtl = GetPaymentInfo(TransID, AgentID)
                                objEmailInfo.ExpiryDate = dtTransDtl.Rows(i)("ExpiryDate").ToString
                                objEmailInfo.Currency = dtTransDtl.Rows(i)("Currency").ToString
                                objEmailInfo.PaymentAmt = PaymentAmt
                                objEmailInfo.CollectedAmt = dtTransDtl.Rows(i)("CollectedAmt")
                                objEmailInfo.TransTotalAmt = dtTransDtl.Rows(i)("TransTotalAmt")
                                objEmailInfo.BalanceDue = objEmailInfo.TransTotalAmt - objEmailInfo.CollectedAmt
                            End If
                        ElseIf EmailType = 7 Then
                            objEmailInfo.BookingID = TransID
                            dtTransDtl = GetBookingDetailsByPNR(TransID, RecordLocator)
                            If Not dtTransDtl Is Nothing Then
                                objEmailInfo.ExpiryDate = dtTransDtl.Rows(0)("NextDueDate").ToString
                                objEmailInfo.Currency = dtTransDtl.Rows(0)("Currency").ToString
                                objEmailInfo.PaymentAmt = PaymentAmt
                                objEmailInfo.CollectedAmt = dtTransDtl.Rows(0)("DetailCollectedAmt")
                                objEmailInfo.TransTotalAmt = dtTransDtl.Rows(0)("LineTotal")
                                objEmailInfo.BalanceDue = objEmailInfo.TransTotalAmt - objEmailInfo.CollectedAmt
                                objEmailInfo.NextPaymentAmt = dtTransDtl.Rows(0)("NextDueAmount")
                                objEmailInfo.RecordLocator = RecordLocator
                            Else
                                Return ""
                            End If
                        End If

                        messageBody = GenerateEmailBody(objEmailInfo)
                        If messageBody Is Nothing = True Then
                            Return "Error with EmailBody"
                        Else
                            objEmailInfo.EmailBody = messageBody
                            If SaveIntoEmail(objEmailInfo) = True Then

                            End If

                        End If
                    Next
                End If
            End If
            Return ""
        Catch ex As Exception
            _proMsg = ex.Message
            Return "Process error: " & _proMsg
        End Try
    End Function

    'added by ketee, Email custome content
    <WebMethod()> _
    Public Function CustomEmail(ByVal EmailAddress As String, ByVal TransID As String, ByVal Subject As String, ByVal Content As String) As String
        Dim messageBody As String = vbNullString
        Dim BatchNo As String = vbNullString
        Dim dtContact As DataTable = Nothing
        Dim dtTransDtl As DataTable = Nothing

        Try
            StartSQLControl()
            TransID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, TransID)
            EmailAddress = objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmailAddress)
            Content = objSQL.ParseValue(SQLControl.EnumDataType.dtString, Content)
            Subject = objSQL.ParseValue(SQLControl.EnumDataType.dtString, Subject)

            BatchNo = GenerateMsgID()

            If EmailAddress Is Nothing = False Then

                Dim objEmailInfo As New EmailInboxInfo
                objEmailInfo.BatchNo = BatchNo
                objEmailInfo.AgentID = 0
                objEmailInfo.UserName = "ErrorLog"
                objEmailInfo.EmailPassword = ""
                objEmailInfo.Title = Subject
                objEmailInfo.FirstName = ""
                objEmailInfo.LastName = ""
                'objEmailInfo.EmailAddress = dtContact.Rows(i)("Email").ToString
                objEmailInfo.EmailAddress = EmailAddress
                'objEmailInfo.EmailAddress = "diana.huang8808@gmail.com"
                'objEmailInfo.EmailAddress = "ketee.vit@gmail.com"
                '0=CDS, 1=Register, 2=Password, 3=PaymentReminder , 4=PaymentConfirmation, 5=FullPaymentConfirmation 6=PassengerDetailReminder
                objEmailInfo.EmailType = 7

                objEmailInfo.BookingID = TransID

                dtTransDtl = GetBookingDetailsByTransID(TransID)
                If Not dtTransDtl Is Nothing Then
                    For Each dtRow In dtTransDtl.Rows
                        If (objEmailInfo.RecordLocator <> "") Then
                            objEmailInfo.RecordLocator &= " "
                        End If
                        objEmailInfo.RecordLocator &= dtRow("RecordLocator")
                    Next
                End If


                messageBody = Content
                If messageBody Is Nothing = True Then
                    Return "Error with EmailBody"
                Else
                    objEmailInfo.EmailBody = messageBody
                    If SaveIntoEmail(objEmailInfo) = True Then

                    End If

                End If

            End If
            Return ""
        Catch ex As Exception
            _proMsg = ex.Message
            Return "Process error: " & _proMsg
        Finally
            EndSQLControl()
        End Try
    End Function

    ''' <summary>
    ''' Obtain Agent Contact Details
    ''' </summary>
    ''' <param name="AgentID"></param>
    ''' <returns></returns>
    ''' <remarks>Jasus</remarks>
    Public Function GetAgentContact(ByVal AgentID As String) As DataTable

        Dim dtDetails As DataTable = Nothing
        Dim strCond As String = String.Empty
        Dim strSQL As String

        Try


            StartConnection()
            StartSQLControl()
            AgentID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID)
            strSQL = "SELECT Username, Password, Title, ContactFirstName, ContactLastName, Email " & _
                      "FROM AG_PROFILE WHERE AgentID='" & AgentID & "' "
            dtDetails = objDCom.Execute(strSQL, CommandType.Text)

            If dtDetails Is Nothing = False Then
                If dtDetails.Rows.Count > 0 Then
                    Return dtDetails
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Catch ex As Exception
            _proMsg = ex.Message
            Return Nothing
        Finally
            EndSQLControl()
            EndConnection()
        End Try
    End Function

    Public Function GetAllAgent() As DataTable
        Dim dtDetails As DataTable = Nothing
        Dim strCond As String = String.Empty
        Dim strSQL As String
        Try
            StartConnection()
            StartSQLControl()

            strSQL = "SELECT Username, Password, Title, ContactFirstName, ContactLastName, Email " & _
                     "FROM AG_PROFILE "
            dtDetails = objDCom.Execute(strSQL, CommandType.Text)

            If dtDetails Is Nothing = False Then
                If dtDetails.Rows.Count > 0 Then
                    Return dtDetails
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Catch ex As Exception
            _proMsg = ex.Message
            Throw New HttpException(ex.StackTrace)
        Finally
            EndSQLControl()
            EndConnection()
        End Try
    End Function

    Public Function GetPaymentInfo(ByVal TransID As String, ByVal AgentID As String) As DataTable

        Dim dtDetails As DataTable = Nothing
        Dim strCond As String = String.Empty
        Dim strSQL As String
        Try
            StartConnection()
            StartSQLControl()
            AgentID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID)
            TransID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, TransID)
            strSQL = "SELECT  ExpiryDate, Currency, CollectedAmt, TransTotalAmt " & _
                     " FROM BK_TRANSMAIN " & _
                     " WHERE     (TransID = '" & TransID & "' AND AgentID = '" & AgentID & "')"
            dtDetails = objDCom.Execute(strSQL, CommandType.Text)

            If dtDetails Is Nothing = False Then
                If dtDetails.Rows.Count > 0 Then
                    Return dtDetails
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Catch ex As Exception
            _proMsg = ex.Message
            Return Nothing
        Finally
            EndSQLControl()
            EndConnection()
        End Try
    End Function

    Public Function GetBookingDetailsByPNR(ByVal TransID As String, ByVal PNR As String) As DataTable

        Dim dtDetails As DataTable = Nothing
        Dim strCond As String = String.Empty
        Dim strSQL As String
        Try
            StartConnection()
            StartSQLControl()
            TransID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, TransID)
            PNR = objSQL.ParseValue(SQLControl.EnumDataType.dtString, PNR)
            strSQL = "SELECT  DISTINCT RecordLocator, Currency, TransID, SUM(LineTotal) AS LineTotal, SUM(DetailCollectedAmt) AS DetailCollectedAmt, SUM(LineFee) AS ServiceCharge, PaxAdult, PaxChild, NextDueDate, NextDueAmount" & _
                     " FROM BK_TRANSDTL " & _
                     " WHERE     (TransID = '" & TransID & "' AND RecordLocator = '" & PNR & "')" & _
                     " GROUP BY RecordLocator, Currency, TransID, PaxAdult, PaxChild, NextDueDate, NextDueAmount"
            dtDetails = objDCom.Execute(strSQL, CommandType.Text)

            If dtDetails Is Nothing = False Then
                If dtDetails.Rows.Count > 0 Then
                    Return dtDetails
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If

        Catch ex As Exception
            _proMsg = ex.Message
            Return Nothing
        Finally
            EndConnection()
            EndSQLControl()
        End Try
    End Function

    Public Function GetBookingDetailsByTransID(ByVal TransID As String) As DataTable

        Dim dtDetails As DataTable = Nothing
        Dim strCond As String = String.Empty
        Dim strSQL As String
        Try
            StartConnection()
            StartSQLControl()
            TransID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, TransID)
            strSQL = "SELECT  DISTINCT RecordLocator, Currency, TransID, SUM(LineTotal) AS LineTotal, SUM(DetailCollectedAmt) AS DetailCollectedAmt, SUM(LineFee) AS ServiceCharge, " & _
                     " PaxAdult, PaxChild, MAX(NextDueDate) AS NextDueDate, NextDueAmount, (select top 1 expirydate from bk_transmain where TransID = BK_TRANSDTL.TransID) as expiryDate" & _
                     " FROM BK_TRANSDTL " & _
                     " WHERE     (TransID = '" & TransID & "' AND LEN(RecordLocator)>=6)" & _
                     " GROUP BY RecordLocator, Currency, TransID, PaxAdult, PaxChild, NextDueAmount"
            dtDetails = objDCom.Execute(strSQL, CommandType.Text)

            If dtDetails Is Nothing = False Then
                If dtDetails.Rows.Count > 0 Then
                    Return dtDetails
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If

        Catch ex As Exception
            _proMsg = ex.Message
            Return Nothing
        Finally
            EndConnection()
            EndSQLControl()
        End Try
    End Function

    <WebMethod()> _
    Public Function AgentPasswordRetrievalProcess(ByVal EmailAddress As String) As String
        Dim messageBody As String = vbNullString
        Dim BatchNo As String = vbNullString
        Dim dtContact As DataTable = Nothing
        Try
            StartSQLControl()
            EmailAddress = objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmailAddress)
            Return "Process completed with no error"
            Return messageBody
        Catch ex As Exception
            _proMsg = ex.Message
            Return "Process error: " & _proMsg
        Finally
            EndSQLControl()
        End Try
    End Function

    Public Function GetEmailContact(ByVal EmailAddress As String) As DataTable
        Dim dtDetails As DataTable = Nothing
        Dim strCond As String = String.Empty
        Dim strSQL As String

        Try
            StartConnection()
            StartSQLControl()
            EmailAddress = objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmailAddress)
            strSQL = "SELECT     Username, Password, Title, ContactFirstName, ContactLastName, Email " & _
                     "FROM         AG_PROFILE WHERE Email='" & EmailAddress & "' AND Status=1 "
            dtDetails = objDCom.Execute(strSQL, CommandType.Text)

            If dtDetails Is Nothing = False Then
                If dtDetails.Rows.Count > 0 Then
                    Return dtDetails
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Catch ex As Exception
            _proMsg = ex.Message
            Return Nothing
        Finally
            EndSQLControl()
            EndConnection()
        End Try
    End Function

    'Modify by ketee, 20160202, add as overload function
    <WebMethod()> _
    Public Function ReminderProcess() As String
        Dim messageBody As String = vbNullString
        Dim BatchNo As String = vbNullString
        Dim dtContact As DataTable = Nothing
        Dim dtTransDtl As DataTable = New DataTable
        Try
            'Your processing function here?
            'ProcessingFile()
            BatchNo = GenerateMsgID()
            dtContact = GetReminderInfo()
            If dtContact Is Nothing = False Then
                If dtContact.Rows.Count > 0 Then
                    For i = 0 To dtContact.Rows.Count - 1
                        Dim objEmailInfo As New EmailInboxInfo
                        objEmailInfo.BatchNo = BatchNo
                        objEmailInfo.BookingID = dtContact.Rows(i)("TransID").ToString
                        objEmailInfo.ExpiryDate = dtContact.Rows(i)("ExpiryDate")
                        objEmailInfo.Currency = dtContact.Rows(i)("Currency").ToString
                        dtTransDtl = GetBookingDetailsByTransID(objEmailInfo.BookingID)
                        If Not dtTransDtl Is Nothing Then
                            For Each dtRow In dtTransDtl.Rows
                                If (objEmailInfo.RecordLocator <> "") Then
                                    objEmailInfo.RecordLocator &= " "
                                End If
                                objEmailInfo.RecordLocator &= dtRow("RecordLocator")
                            Next
                        End If
                        objEmailInfo.RecordLocator = dtContact.Rows(i)("RecordLocator").ToString
                        objEmailInfo.CollectedAmt = dtContact.Rows(i)("CollectedAmt")
                        objEmailInfo.TransTotalAmt = dtContact.Rows(i)("TransTotalAmt")
                        objEmailInfo.BalanceDue = objEmailInfo.TransTotalAmt - objEmailInfo.CollectedAmt
                        If Not IsDBNull(dtContact.Rows(i)("NextReminderDate")) Then
                            objEmailInfo.NextReminderDate = dtContact.Rows(i)("NextReminderDate")
                        Else
                            objEmailInfo.NextReminderDate = objEmailInfo.ExpiryDate
                        End If
                        objEmailInfo.AgentID = dtContact.Rows(i)("AgentID")
                        objEmailInfo.Title = dtContact.Rows(i)("Title").ToString
                        objEmailInfo.FirstName = dtContact.Rows(i)("ContactFirstName")
                        objEmailInfo.LastName = dtContact.Rows(i)("ContactLastName")
                        objEmailInfo.EmailAddress = dtContact.Rows(i)("Email")
                        'objEmailInfo.EmailAddress = "ketee.vit@gmail.com"
                        objEmailInfo.Status = dtContact.Rows(i)("TransStatus")
                        Select Case objEmailInfo.Status
                            Case 1
                                objEmailInfo.EmailType = 3
                            Case 2
                                objEmailInfo.EmailType = 6
                        End Select

                        messageBody = GenerateEmailBody(objEmailInfo)
                        If messageBody Is Nothing = True Then
                            Return "Error with EmailBody"
                        Else
                            objEmailInfo.EmailBody = messageBody
                            If SaveIntoEmail(objEmailInfo) = True Then
                                UpdateReminder(objEmailInfo.NextReminderDate, objEmailInfo.BookingID)
                            End If

                        End If
                    Next
                End If
            End If
            Return ""

        Catch ex As Exception
            _proMsg = ex.Message
            Return "Process error: " & _proMsg
        Finally
            Dispose()
        End Try
    End Function

    <WebMethod()> _
    Public Function ResendReminder(ByVal transID As String) As String
        Dim messageBody As String = vbNullString
        Dim BatchNo As String = vbNullString
        Dim dtContact As DataTable = Nothing
        Dim dtTransDtl As DataTable = New DataTable
        Try
            'Your processing function here?
            'ProcessingFile()
            StartSQLControl()
            BatchNo = GenerateMsgID()
            transID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, transID)
            dtTransDtl = GetBookingDetailsByTransID(transID)
            dtContact = GetTransaction(transID)
            If dtContact Is Nothing = False Then
                If dtContact.Rows.Count > 0 Then
                    For i = 0 To dtContact.Rows.Count - 1
                        Dim objEmailInfo As New EmailInboxInfo
                        objEmailInfo.BatchNo = BatchNo
                        objEmailInfo.BookingID = dtContact.Rows(i)("TransID").ToString
                        objEmailInfo.ExpiryDate = dtContact.Rows(i)("ExpiryDate")
                        objEmailInfo.Currency = dtContact.Rows(i)("Currency").ToString
                        If Not dtTransDtl Is Nothing Then
                            For Each dtRow In dtTransDtl.Rows
                                If (objEmailInfo.RecordLocator <> "") Then
                                    objEmailInfo.RecordLocator &= " "
                                End If
                                objEmailInfo.RecordLocator &= dtRow("RecordLocator")
                            Next
                        End If
                        'objEmailInfo.RecordLocator = dtContact.Rows(i)("RecordLocator").ToString
                        objEmailInfo.CollectedAmt = dtContact.Rows(i)("CollectedAmt")
                        objEmailInfo.TransTotalAmt = dtContact.Rows(i)("TransTotalAmt")
                        objEmailInfo.BalanceDue = objEmailInfo.TransTotalAmt - objEmailInfo.CollectedAmt
                        If Not IsDBNull(dtContact.Rows(i)("NextReminderDate")) Then
                            objEmailInfo.NextReminderDate = dtContact.Rows(i)("NextReminderDate")
                        Else
                            objEmailInfo.NextReminderDate = objEmailInfo.ExpiryDate
                        End If
                        objEmailInfo.AgentID = dtContact.Rows(i)("AgentID")
                        objEmailInfo.Title = dtContact.Rows(i)("Title").ToString
                        objEmailInfo.FirstName = dtContact.Rows(i)("ContactFirstName")
                        objEmailInfo.LastName = dtContact.Rows(i)("ContactLastName")
                        objEmailInfo.EmailAddress = dtContact.Rows(i)("Email")
                        'objEmailInfo.EmailAddress = "ketee.vit@gmail.com"
                        objEmailInfo.Status = dtContact.Rows(i)("TransStatus")
                        objEmailInfo.NextPaymentAmt = dtTransDtl.Rows(0)("NextDueAmount")
                        Select Case objEmailInfo.Status
                            Case 1
                                objEmailInfo.EmailType = 3
                            Case 2
                                objEmailInfo.EmailType = 6
                        End Select

                        messageBody = GenerateEmailBody(objEmailInfo)
                        If messageBody Is Nothing = True Then
                            Return "Error with EmailBody"
                        Else
                            objEmailInfo.EmailBody = messageBody
                            If SaveIntoEmail(objEmailInfo) = True Then
                                UpdateReminder(objEmailInfo.NextReminderDate, objEmailInfo.BookingID)
                            End If

                        End If
                    Next
                End If
            End If
            Return "Process completed with no error"
            Dispose()
        Catch ex As Exception
            _proMsg = ex.Message
            Return "Process error: " & _proMsg
        Finally
            dtTransDtl = Nothing
            EndSQLControl()
        End Try
    End Function


    Public Function GetReminderInfo() As DataTable
        Dim dtDetails As DataTable = Nothing
        Dim strCond As String = String.Empty
        Dim strSQL As String
        Try
            StartConnection()
            StartSQLControl()

            strSQL = "SELECT a.TransID, a.ExpiryDate, a.Currency, a.CollectedAmt, a.TransTotalAmt, " & _
                     " a.NextReminderDate, b.Title, a.Transstatus, " & _
                     " b.ContactFirstName, b.ContactLastName, b.Email, b.AgentID" & _
                     " FROM         BK_TRANSMAIN AS a INNER JOIN " & _
                     " AG_PROFILE AS b ON a.AgentID = b.AgentID " & _
                     " WHERE     (a.TransStatus in (1,2)) AND (a.CurReminderDate = CAST(GETDATE() AS date)) "
            dtDetails = objDCom.Execute(strSQL, CommandType.Text)

            If dtDetails Is Nothing = False Then
                If dtDetails.Rows.Count > 0 Then
                    Return dtDetails
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If

        Catch ex As Exception
            _proMsg = ex.Message
            Return Nothing
        Finally
            EndSQLControl()
            EndConnection()
        End Try
    End Function

    Public Function GetTransaction(ByVal transID As String) As DataTable
        Dim dtDetails As DataTable = Nothing
        Dim strCond As String = String.Empty
        Dim strSQL As String
        Try
            StartConnection()
            StartSQLControl()
            transID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, transID)

            strSQL = "SELECT a.TransID, a.ExpiryDate, a.Currency, a.CollectedAmt, a.TransTotalAmt, " & _
                     " a.NextReminderDate, b.Title, a.Transstatus, " & _
                     " b.ContactFirstName, b.ContactLastName, b.Email, b.AgentID" & _
                     " FROM         BK_TRANSMAIN AS a INNER JOIN " & _
                     " AG_PROFILE AS b ON a.AgentID = b.AgentID " & _
                     " WHERE     (a.TransStatus in (1,2)) AND A.TransID='" & transID & "' "
            dtDetails = objDCom.Execute(strSQL, CommandType.Text)

            If dtDetails Is Nothing = False Then
                If dtDetails.Rows.Count > 0 Then
                    Return dtDetails
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If

        Catch ex As Exception
            _proMsg = ex.Message
            Return Nothing
        Finally
            EndSQLControl()
            EndConnection()
        End Try
    End Function

    Public Sub UpdateReminder(ByVal NextReminderDate As DateTime, ByVal TransID As String)
        Dim dt As DataTable = Nothing
        'Dim strCond As String = String.Empty
        Dim strSQL As String
        Try
            StartConnection()
            StartSQLControl()
            TransID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, TransID)
            strSQL = "UPDATE BK_TRANSMAIN SET CurReminderDate= '" & NextReminderDate & "', SyncLastUpd=Getdate() WHERE TransID = '" & TransID & "' "
            dt = objDCom.Execute(strSQL, CommandType.Text)
        Catch ex As Exception
            _proMsg = ex.Message
        Finally
            EndSQLControl()
            EndConnection()
        End Try
    End Sub

    <WebMethod()> _
    Public Function LastProcessMessage() As String
        Return _proMsg
    End Function

    'cancel PNR
    <WebMethod()> _
    Public Function CancelBooking(ByVal AgentID As String, ByVal PNR As String)
        Try
            StartSQLControl()
            AgentID = objSQL.ParseValue(SQLControl.EnumDataType.dtString, AgentID)
            PNR = objSQL.ParseValue(SQLControl.EnumDataType.dtString, PNR)

            Dim objNavitaire As APIBooking = New APIBooking("")
            Dim objBooking As BookingControl = New BookingControl()
            Return objBooking.CancelBookingByPNR(AgentID, PNR)
        Catch ex As Exception
            _proMsg = ex.Message
            Return Nothing
        Finally
            EndSQLControl()
        End Try
    End Function

    Private Function GetBooking(ByVal PNR As String) As GetBookingResponse
        Try
            StartSQLControl()
            PNR = objSQL.ParseValue(SQLControl.EnumDataType.dtString, PNR)

            Dim objNavitaire As APIBooking = New APIBooking("")
            Dim errmsg As String = ""
            Dim responseBooking As GetBookingResponse = New GetBookingResponse()

            responseBooking = objNavitaire.GetBookingResponseByPNR(PNR)
            Return responseBooking
        Catch ex As Exception
            _proMsg = ex.Message
            Throw New HttpException(ex.StackTrace)
        Finally
            EndSQLControl()
        End Try
    End Function

    <WebMethod()> _
    Public Function TestGetBooking(ByVal PNR As String) As String
        Try
            StartSQLControl()
            PNR = objSQL.ParseValue(SQLControl.EnumDataType.dtString, PNR)

            Dim resp As GetBookingResponse = New GetBookingResponse
            resp = GetBooking(PNR)
            If Not resp Is Nothing Then
                Return "Booking Retrieved"
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
            Return String.Empty
        Finally
            EndSQLControl()
        End Try
    End Function

    'auto cancel transaction
    <WebMethod()> _
    Public Function CancelTransaction() As String
        Dim objBooking As BookingControl = New BookingControl()
        If objBooking.CancelUpToDateExpiryTransaction("aax") = False Then
            CustomEmail(ConfigurationManager.AppSettings("ErrorLogEmail").ToString(), "", "Cancellation Fail", " <br/> Fail to Cancel payment expiry Transaction at: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"))
            Return "Cancellation fail to complete successfully"
        End If
        Return "Process Done"
    End Function

    'added by ketee, 20160205, update all payment expiry booking transaction details, and passenger list
    <WebMethod(True)>
    Public Function UpdateAllPaymentExpiryBookingDetails(ByVal Mode As Integer) As String
        Dim objBooking As BookingControl = New BookingControl()
        Dim AllTransaction As List(Of BookingTransactionMain) = New List(Of BookingTransactionMain)
        Dim listTrans As ListTransaction = Nothing
        Dim msg As String = ""
        Try
            AllTransaction = objBooking.GetAllBK_TRANSMAINTransactionExpiry("", "", "", "1", False, 3, 1)
            If Not AllTransaction Is Nothing AndAlso AllTransaction.Count > 0 Then
                Dim VoidPNRs As List(Of BookingTransactionDetail) = New List(Of BookingTransactionDetail)
                Dim ExpiredPNRs As List(Of BookingTransactionDetail) = New List(Of BookingTransactionDetail)
                For Each trans In AllTransaction
                    Dim singleTrans As List(Of ListTransaction) = New List(Of ListTransaction)
                    listTrans = New ListTransaction
                    singleTrans = objBooking.GetTransactionDetails(trans.TransID)
                    listTrans = singleTrans(0)
                    If objBooking.UpdateAllBookingJourneyDetails(listTrans, listTrans.AgentUserName.ToString(), listTrans.AgentID.ToString(), VoidPNRs, ExpiredPNRs, True, True, Mode) = False Then
                        _proMsg = "Fail to Get Latest Update for Transaction : " + listTrans.TransID
                        CustomEmail(ConfigurationManager.AppSettings("ErrorLogEmail").ToString(), listTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update for Transaction : " + listTrans.TransID)
                    Else
                        msg = objBooking.GetLatestReminderbyTransID(listTrans.TransID)
                        If msg = "" Then
                            ResendReminder(listTrans.TransID)
                            msg = ""
                        End If
                    End If

                Next

                If objBooking.UpdateTransDTL(VoidPNRs, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update) = True Then
                    Dim dtMaxAttempt As DataTable = objBooking.GetSysPreftbyGrpID("AA", "MAXATTEMPT")
                    If Not dtMaxAttempt Is Nothing AndAlso dtMaxAttempt.Rows.Count > 0 And Not IsDBNull(dtMaxAttempt.Rows(0)("SYSValue")) Then
                        Dim FailPNRs As List(Of BookingTransactionDetail) = New List(Of BookingTransactionDetail)
                        FailPNRs = objBooking.GetTransDTLByAttempt(dtMaxAttempt.Rows(0)("SYSValue"))
                        Dim PNRs As String = ""
                        For Each bookDTL As BookingTransactionDetail In FailPNRs
                            If PNRs <> "" Then PNRs &= ", "
                            PNRs &= bookDTL.RecordLocator
                        Next

                        If PNRs <> "" Then
                            _proMsg = "Failed to get latest update for PNR : " + PNRs
                            CustomEmail(ConfigurationManager.AppSettings("ErrorLogEmail").ToString(), listTrans.TransID, "", "Errors Log :" & DateTime.Now.ToString() & " <br/> " & _proMsg)
                        End If
                    End If

                End If

                If Mode = 0 Then
                    Dim PNRs As String = ""
                    For Each bookDTL As BookingTransactionDetail In ExpiredPNRs
                        If PNRs <> "" Then PNRs &= ", "
                        PNRs &= bookDTL.RecordLocator
                    Next

                    If PNRs <> "" Then
                        _proMsg = "Expired PNR : " + PNRs
                        CustomEmail(ConfigurationManager.AppSettings("ErrorLogEmail").ToString(), listTrans.TransID, "", "Errors Log :" & DateTime.Now.ToString() & " <br/> " & _proMsg)
                    End If
                End If

                Return ""
            End If
        Catch ex As Exception
            _proMsg = ex.Message
            CustomEmail(ConfigurationManager.AppSettings("ErrorLogEmail").ToString(), listTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update for Transaction : " + listTrans.TransID)
            Return _proMsg
        Finally
            objBooking = Nothing
            AllTransaction = Nothing
            listTrans = Nothing

        End Try
    End Function

    'updating flight details
    <WebMethod(True)> _
    Public Function UpdateFlight()
        Dim objBooking As BookingControl = New BookingControl()
        Return objBooking.UpdateFlightAndPayment()
    End Function

    'clear expired journey
    <WebMethod(True)> _
    Public Function ClearJourney()
        Dim objBooking As BookingControl = New BookingControl()
        Return objBooking.ClearExpiredJourney()
    End Function

    'updating all transaction
    'remark by ketee, 20160621
    ''<WebMethod(True)> _
    ''Public Function UpdateTransaction()
    ''    Dim objBooking As BookingControl = New BookingControl()
    ''    Return objBooking.UpdateAllTransaction()
    ''End Function

    'updating pending confirm transaction
    <WebMethod(True)> _
    Public Function UpdateUnconfirmedTransaction()
        Dim objBooking As BookingControl = New BookingControl()
        Return objBooking.UpdatePendingConfirmTransactionProcess()
    End Function

    'updating pending payment transaction
    <WebMethod(True)> _
    Public Function UpdatePendingPaymentTransaction()
        Dim objBooking As BookingControl = New BookingControl()
        Return objBooking.UpdatePendingPaymentTransactionProcess()
    End Function

    'updating pending passenger transaction
    <WebMethod(True)> _
    Public Function UpdatePendingPassengerTransaction()
        Dim objBooking As BookingControl = New BookingControl()
        Return objBooking.UpdatePendingPassengerTransactionProcess()
    End Function

    'get latest flight details
    <WebMethod(True)> _
    Public Function GetLatestFlight()
        Dim objBooking As BookingControl = New BookingControl()
        Return objBooking.AutoUpdateTransaction()
    End Function

    'auto cancel flight
    <WebMethod()> _
    Public Function AutoCancelFlight()
        Dim objBooking As BookingControl = New BookingControl()
        Return objBooking.CancelExpiredTransaction()
    End Function

#Region "Core Function"
    Public Overloads Function GenerateMsgID() As String
        Dim rValue As String = Nothing
        rValue = DateTime.Now().Year.ToString + DateTime.Now().Month.ToString + DateTime.Now().Day.ToString + DateTime.Now().Hour.ToString + DateTime.Now().Minute.ToString + DateTime.Now().Second.ToString + DateTime.Now().Millisecond.ToString
        Return rValue
    End Function
    Private Sub StartSQLControl()
        objSQL = New SEAL.Data.SQLControl
        objSQL.ParseForType = SEAL.Data.SQLControl.EnumParseFor.MSSQL
    End Sub
    Private Sub EndSQLControl()
        objSQL.Dispose()
    End Sub
    Private Sub StartConnection()
        objDCom = New SEAL.Data.DataAccess
        'Henry 20120907
        'objDCom.ConnectionString = My.Settings.ConnStr2
        objDCom.ConnectionString = My.Settings.ConnStr
    End Sub
    Private Sub StartConnection2()
        objDCom = New SEAL.Data.DataAccess
        'Henry 20120907
        objDCom.ConnectionString = My.Settings.ConnStr2
        'objDCom.ConnectionString = My.Settings.ConnStr
    End Sub
    Private Sub EndConnection()
        Try
            If objDCom Is Nothing = False Then
                objDCom.CloseConnection()
                objDCom.Dispose()
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region
End Class