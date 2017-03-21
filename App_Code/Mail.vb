Imports Microsoft.VisualBasic
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Net

<Serializable>
Public Class Mail
    Public cCOR_Id As Integer
    Public cEMI_Id As Integer
    Public cCOR_Servidor As String
    Public cCOR_Usuario As String
    Public cCOR_Contrasena As String
    Public cCOR_Puerto As Integer
    Public cCOR_Email As String

    Public Shared Function enviaMail(ByVal ipServidorCorreo As String,
                                        ByVal puerto As Integer,
                                        ByVal usr As String,
                                        ByVal psw As String,
                                        ByVal nombreRemitente As String,
                                        ByVal eMailRemitente As String,
                                        ByVal destinatarios As List(Of String),
                                        ByVal cc As List(Of String),
                                        ByVal asunto As String,
                                        ByVal contenido As String,
                                        ByVal aAdjuntos As List(Of String)) As Boolean
        Dim smtpCliente As New System.Net.Mail.SmtpClient(ipServidorCorreo)
        Dim Mensaje As New System.Net.Mail.MailMessage()

        Try
            Net.ServicePointManager.ServerCertificateValidationCallback = Nothing
            smtpCliente.Port = puerto
            contenido = contenido.Replace(vbCrLf, "<br>")
            Mensaje.From = New System.Net.Mail.MailAddress(eMailRemitente, nombreRemitente)

            'For Each destinatario As String In destinatarios
            '    If destinatario IsNot "" Then Mensaje.To.Add(destinatario)
            'Next

            'For Each cCopia As String In cc
            '    If cCopia IsNot "" Then Mensaje.CC.Add(cCopia)
            'Next

            Mensaje.To.Add("oscar.sandoval@gmx.com.mx")
            Mensaje.Subject = asunto
            Mensaje.IsBodyHtml = True
            Mensaje.BodyEncoding = System.Text.Encoding.UTF8

            Mensaje.Body = "<!DOCTYPE HTML PUBLIC '-//W3C//DTD HTML 4.0 Transitional//EN'><html><head><title>HTMLPage1</title><META http-equiv='Content-Type' content='text/html; charset=utf-8'>"
            Mensaje.Body += "<meta name='vs_defaultClientScript' content='JavaScript'><meta name='vs_targetSchema' content='http://schemas.microsoft.com/intellisense/ie5'><meta name='GENERATOR' content='Microsoft Visual Studio.NET 7.0'><meta name='ProgId' content='VisualStudio.HTML'><meta name='Originator' content='Microsoft Visual Studio.NET 7.0'></head>"
            Mensaje.Body += "<body MS_POSITIONING='GridLayout'><DIV style='DISPLAY: inline; Z-INDEX: 101; LEFT: 48px; WIDTH: 601px; POSITION: absolute; TOP: 71px; HEIGHT: 289px; TEXT-ALIGN: justify' ms_positioning='FlowLayout' id='DIV1' language='javascript' onclick='return DIV1_onclick()'><P><SPAN lang='ES-MX' style='FONT-SIZE: 12pt; mso-ansi-language: ES-MX'><B style='mso-bidi-font-weight: normal'><SPAN lang='ES-MX' style='FONT-SIZE: 12pt; FONT-FAMILY: Arial; mso-ansi-language: ES-MX'>$ASUNTO</SPAN></B></SPAN></P>"
            Mensaje.Body += "<P><SPAN lang='ES-MX' style='FONT-SIZE: 12pt; mso-ansi-language: ES-MX'><B style='mso-bidi-font-weight: normal'><SPAN lang='ES-MX' style='FONT-SIZE: 12pt; FONT-FAMILY: Arial; mso-ansi-language: ES-MX'></SPAN></B></SPAN><SPAN lang='ES-MX' style='FONT-SIZE: 12pt; FONT-FAMILY: Arial; mso-ansi-language: ES-MX'>$CONTENIDO</SPAN></P><P><SPAN lang='ES-MX' style='FONT-SIZE: 12pt; FONT-FAMILY: Arial; mso-ansi-language: ES-MX'></SPAN></P>"
            Mensaje.Body += "<P class='MsoNormal'><SPAN lang='ES-MX' style='FONT-SIZE: 7pt; COLOR: navy; FONT-FAMILY: Arial; mso-ansi-language: ES-MX'><br><br>Envío automático, favor de no responder el mensaje.</SPAN><SPAN lang='ES-MX' style='FONT-SIZE: 12pt; mso-ansi-language: ES-MX'><o:p></o:p></SPAN></P></DIV></body></html>"
            Mensaje.Body = Mensaje.Body.Replace("$ASUNTO", asunto).Replace("$CONTENIDO", contenido)

            If Not aAdjuntos Is Nothing Then
                For Each aAdjunto As String In aAdjuntos
                    If aAdjunto IsNot "" Then If IO.File.Exists(aAdjunto) Then Mensaje.Attachments.Add(New System.Net.Mail.Attachment(aAdjunto))
                Next
            End If

            smtpCliente.UseDefaultCredentials = True
            smtpCliente.Credentials = New System.Net.NetworkCredential(usr, psw)
            smtpCliente.EnableSsl = True
            ServicePointManager.ServerCertificateValidationCallback = Function(s As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) True

            smtpCliente.Send(Mensaje)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
