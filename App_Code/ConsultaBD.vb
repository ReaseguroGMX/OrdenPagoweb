Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Data




' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ScriptService>
Public Class ConsultaBD
    Inherits System.Web.Services.WebService

    Private sCnn As String
    Private sSel As String
    Dim da As SqlDataAdapter
    Dim dtConsulta As DataTable


    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetSucursal(ByVal prefix As String) As String()
        Dim sucursales As New List(Of String)()
        Using conn As New SqlConnection()
            'Obtiene cadena de conexión de Web Config
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
            Using cmd As New SqlCommand()

                If Not prefix = vbNullString Then
                    cmd.CommandText = "SELECT cod_suc,txt_nom_suc FROM tsuc where " & "txt_nom_suc like '%' + @SearchText + '%'"
                    cmd.Parameters.AddWithValue("@SearchText", prefix)
                Else
                    cmd.CommandText = "SELECT cod_suc,txt_nom_suc FROM tsuc"
                End If

                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        sucursales.Add(String.Format("{0}|{1}", sdr("txt_nom_suc"), sdr("cod_suc")))
                    End While
                End Using
                conn.Close()
            End Using
            Return sucursales.ToArray()
        End Using
    End Function


    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetConcepto(ByVal prefix As String) As String()
        Dim sucursales As New List(Of String)()
        Using conn As New SqlConnection()
            'Obtiene cadena de conexión de Web Config
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
            Using cmd As New SqlCommand()

                cmd.CommandText = "select distinct concepto,desc_concepto From tasiento_reas Where (sn_cta_corriente = -1 Or sn_ogis = -1) AND " & "desc_concepto like '%' + @SearchText + '%' order by concepto,desc_concepto"
                cmd.Parameters.AddWithValue("@SearchText", prefix)
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        sucursales.Add(String.Format("{0}|{1}", sdr("desc_concepto"), sdr("concepto")))
                    End While
                End Using
                conn.Close()
            End Using
            Return sucursales.ToArray()
        End Using
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetCompañias(ByVal prefix As String, ByVal filtro As String) As String()
        Dim compañias As New List(Of String)()
        Using conn As New SqlConnection()
            'Obtiene cadena de conexión de Web Config
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
            Using cmd As New SqlCommand()

                cmd.CommandText = "select cod_cia_reas,txt_razon_social,id_persona,nro_nit from mcias_reas where " & "txt_razon_social like '%' + @SearchText + '%'" & filtro & " ORDER BY txt_razon_social"
                cmd.Parameters.AddWithValue("@SearchText", prefix)
                cmd.Parameters.AddWithValue("@Filter", filtro)

                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        compañias.Add(String.Format("{0}|{1}", sdr("txt_razon_social"), sdr("cod_cia_reas")))
                    End While
                End Using
                conn.Close()
            End Using
            Return compañias.ToArray()
        End Using
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetCompañia(ByVal Id As Integer) As String
        Dim compañia As String = ""
        Using conn As New SqlConnection()
            'Obtiene cadena de conexión de Web Config
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
            Using cmd As New SqlCommand()
                cmd.CommandText = "select cod_cia_reas,txt_razon_social from mcias_reas where cod_cia_reas = @SearchId"
                cmd.Parameters.AddWithValue("@SearchId", Id)
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        compañia = sdr("txt_razon_social")
                    End While
                End Using
                conn.Close()
            End Using
            Return compañia
        End Using
    End Function


    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetRamos(ByVal prefix As String, ByVal filtro As String) As String()
        Dim ramos As New List(Of String)()
        Using conn As New SqlConnection()
            'Obtiene cadena de conexión de Web Config
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
            Using cmd As New SqlCommand()

                cmd.CommandText = "select cod_ramo, txt_desc from  tramo where sn_ramo_comercial = -1 AND  " & "txt_desc like '%' + @SearchText + '%'" & filtro & " order by txt_desc"
                cmd.Parameters.AddWithValue("@SearchText", prefix)
                cmd.Parameters.AddWithValue("@Filter", filtro)

                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        ramos.Add(String.Format("{0}|{1}", sdr("txt_desc"), sdr("cod_ramo")))
                    End While
                End Using
                conn.Close()
            End Using
            Return ramos.ToArray()
        End Using
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetRamo(ByVal Id As Integer) As String
        Dim ramo As String = ""
        Using conn As New SqlConnection()
            'Obtiene cadena de conexión de Web Config
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
            Using cmd As New SqlCommand()
                cmd.CommandText = "select cod_ramo, txt_desc from  tramo where sn_ramo_comercial = -1 AND cod_ramo = @SearchId"
                cmd.Parameters.AddWithValue("@SearchId", Id)
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        ramo = sdr("txt_desc")
                    End While
                End Using
                conn.Close()
            End Using
            Return ramo
        End Using
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetContrato(ByVal Id As String) As String
        Dim contrato As String = ""
        Using conn As New SqlConnection()
            'Obtiene cadena de conexión de Web Config
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
            Using cmd As New SqlCommand()
                cmd.CommandText = "SELECT distinct mr.id_contrato, mcontrato.txt_desc " +
                                  "FROM mr, mcontrato " +
                                  "WHERE   mr.id_contrato = mcontrato.id_contrato and mr.id_contrato = @SearchId " +
                                  "AND NOT exists  (SELECT tmp_mr_aplica.id_mr FROM tmp_mr_aplica Where  tmp_mr_aplica.id_mr = mr.id_mr ) "

                cmd.Parameters.AddWithValue("@SearchId", Id)
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        contrato = sdr("txt_desc")
                    End While
                End Using
                conn.Close()
            End Using
            Return contrato
        End Using
    End Function


    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetRamosContables(ByVal prefix As String, ByVal filtro As String) As String()
        Dim ramos As New List(Of String)()
        Using conn As New SqlConnection()
            'Obtiene cadena de conexión de Web Config
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
            Using cmd As New SqlCommand()

                cmd.CommandText = "select cod_ramo_contable, txt_desc from  tramo_contable where txt_desc like '%' + @SearchText + '%'" & filtro & " order by txt_desc"
                cmd.Parameters.AddWithValue("@SearchText", prefix)
                cmd.Parameters.AddWithValue("@Filter", filtro)

                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        ramos.Add(String.Format("{0}|{1}", sdr("txt_desc"), sdr("cod_ramo_contable")))
                    End While
                End Using
                conn.Close()
            End Using
            Return ramos.ToArray()
        End Using
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetRamoContable(ByVal Id As Integer) As String
        Dim ramo As String = ""
        Using conn As New SqlConnection()
            'Obtiene cadena de conexión de Web Config
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
            Using cmd As New SqlCommand()
                cmd.CommandText = "select cod_ramo_contable, txt_desc from  tramo_contable where cod_ramo_contable = @SearchId"
                cmd.Parameters.AddWithValue("@SearchId", Id)
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        ramo = sdr("txt_desc")
                    End While
                End Using
                conn.Close()
            End Using
            Return ramo
        End Using
    End Function


    Public Function ObtieneReaseguros(ByVal FiltroContrato As String, ByVal FiltroBroCia As String, ByVal FiltroPol As String,
                                      ByVal FiltroFecha As String, ByVal FiltroRamoCont As String, ByVal FiltroRamoTec As String,
                                      ByVal Estatus As Integer, ByVal Cobranzas As Integer, ByVal FiltroOP As String,
                                      ByVal FiltroMoneda As String) As DataTable

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spS_LstOP_Pendiente '" & FiltroContrato & "','" & FiltroBroCia & "','" & FiltroPol & "','" &
                                         FiltroFecha & "','" & FiltroRamoCont & "','" & FiltroRamoTec & "'," &
                                         Estatus & "," & Cobranzas & ",'" & FiltroOP & "','" & FiltroMoneda & "'"



        da = New SqlDataAdapter(sSel, sCnn)

        da.SelectCommand.CommandTimeout = 20000

        da.Fill(dtConsulta)

        Return dtConsulta

    End Function

    Public Function ObtieneTipoCambio(ByVal Fecha As String, ByVal cod_moneda As Integer) As Double

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spS_TipoCambio '" & Fecha & "'," & cod_moneda

        Dim da As SqlDataAdapter

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtConsulta)

        Return dtConsulta.Rows(0)(0)
    End Function

    Public Function InsertaBitacora(ByVal cod_usuario_NT As String, ByVal HostName As String, ByVal Accion As String, ByVal Descripcion As String) As Boolean
        Dim sCnn As String = ""
        Dim sSel As String
        Dim Resultado As String = ""

        InsertaBitacora = False

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spI_BitacoraOP '" & cod_usuario_NT & "','" & Accion & "','" & Descripcion & "','" & HostName & "'"

        Dim da As SqlDataAdapter
        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        If dtRes(0)(0) <> "1" Then
            Exit Function
        Else
            InsertaBitacora = True
        End If
    End Function

    Public Function ConsultaUsuarioNT(ByVal cod_usuario As String) As String
        Dim sCnn As String = ""
        Dim sSel As String
        Dim Resultado As String = ""

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spS_UsuarioNT '" & cod_usuario & "'"


        Dim da As SqlDataAdapter
        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        If dtRes.Rows.Count > 0 Then
            Resultado = dtRes.Rows(0)("cod_usuario_NT")
        End If

        Return Resultado
    End Function

    Public Function ConsultaUsuario(ByVal cod_usuario_NT As String) As String
        Dim sCnn As String = ""
        Dim sSel As String
        Dim Resultado As String = ""

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spS_UsuarioSII '" & cod_usuario_NT & "'"


        Dim da As SqlDataAdapter
        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)


        da.Fill(dtRes)

        Session.Add("cod_usuario", "")
        Session.Add("cod_suc", "0")
        Session.Add("cod_sector", "0")

        If dtRes.Rows.Count > 0 Then
            Session("cod_usuario") = dtRes.Rows(0)("cod_usuario")
            Session("Usuario") = dtRes.Rows(0)("Usuario")
            Session("cod_suc") = dtRes.Rows(0)("cod_suc")
            Session("cod_sector") = dtRes.Rows(0)("cod_sector")
            Resultado = dtRes.Rows(0)("cod_usuario")
        End If

        Return Resultado

    End Function

    Public Function InsertaLogError(ByVal cod_usuario_NT As String, ByVal HostName As String, ByVal Descripcion As String) As Boolean
        Dim sCnn As String = ""
        Dim sSel As String
        Dim Resultado As String = ""

        InsertaLogError = False

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spI_LogErrores '" & cod_usuario_NT & "','" & Descripcion & "','" & HostName & "'"

        Dim da As SqlDataAdapter
        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        If dtRes(0)(0) <> "1" Then
            Exit Function
        Else
            InsertaLogError = True
        End If
    End Function
    Public Function ConsultaPolNoPago() As DataTable
        Dim sCnn As String = ""
        Dim sSel As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spS_EndososNoPagoOP -1"

        Dim da As SqlDataAdapter
        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        Return dtRes

    End Function

    Public Function InsertaPolNoPago(ByVal id_pv As Integer, ByVal cod_usuario_NT As String) As Boolean
        Dim sCnn As String = ""
        Dim sSel As String
        Dim Resultado As String = ""

        InsertaPolNoPago = False

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "INSERT INTO rEndososNoPago VALUES(" & id_pv & ",'" & cod_usuario_NT & "','" & Month(Today) & "/" & Day(Today) & "/" & Year(Today) & "')"

        Dim da As SqlDataAdapter
        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        InsertaPolNoPago = True
    End Function

    Public Function BorraPolNoPago(ByVal id_pv As String) As Boolean
        Dim sCnn As String = ""
        Dim sSel As String
        Dim Resultado As String = ""

        BorraPolNoPago = False

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "DELETE FROM rEndososNoPago WHERE id_pv IN (" & id_pv & ")"

        Dim da As SqlDataAdapter
        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        BorraPolNoPago = True
    End Function


End Class