Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq

Partial Class Pages_FirmasElectronicas
    Inherits System.Web.UI.Page

    Private dtBusqueda As DataTable

    Private Enum TipoPersona
        Solicitante
        JefeInmediato
        DirectorArea
        Contabilidad
    End Enum

    Private Sub Pages_FirmasElectronicas_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Session.Timeout = 60000
                Session.Add("blnExpiro", "1")

                BindDummyRow()

                Header.Title = "GMX - Firmas Electrónicas"

                Master.Usuario = IIf(Session("Usuario") = "", "Inicia Sesión", Session("Usuario"))
                Master.cod_usuario = Session("cod_usuario")
                Master.cod_suc = Session("cod_suc")
                Master.cod_sector = Session("cod_sector")
                Master.HostName = Session("HostName")
                hid_Url.Value = "VistaOrdenPago.aspx?cod_usuario=" & Master.cod_usuario & "&Usuario=" & Master.Usuario & "&cod_suc=" & Master.cod_suc & "&cod_sector=" & Master.cod_sector

                'Verifica que se haya ingresado a partir de Login
                If Master.cod_usuario = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Redirect", "Redireccionar('Login.aspx');", True)
                End If
                hid_CierraSesion.Value = 0

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Menu", "openNav();", True)
            Else
                If Session("blnExpiro") Is Nothing And hid_CierraSesion.Value = 0 Then
                    Mensaje("ORDEN DE PAGO", "La sesión ha expirado por inactividad, debera ingresar sus credenciales nuevamente")
                    Dim ConsultaBD As ConsultaBD
                    ConsultaBD = New ConsultaBD
                    ConsultaBD.InsertaBitacora(ConsultaBD.ConsultaUsuarioNT(Master.cod_usuario), Master.HostName, "Expiró Sesión (Ordenes de Pago)", "La sesión ha expirado por inactividad, debere ingresar sus credenciales nuevamente")
                    hid_CierraSesion.Value = 1
                    System.Threading.Thread.Sleep(3000) ' 3 segundos
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Redirect", "Redireccionar('Login.aspx');", True)
                End If
            End If
            gvd_Catalogo.UseAccessibleHeader = False
            gvd_Catalogo.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:PAGE LOAD", ex.Message)
            LogError("(Pages_OrdenPago_Load)" & ex.Message)
        End Try
    End Sub

    <System.Web.Services.WebMethod>
    Public Shared Function ObtieneDatos(ByVal Consulta As String) As List(Of Catalogo)
        Dim sCnn As String

        Consulta = Replace(Consulta, "==", "'")


        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(Consulta, sCnn)
        da.Fill(dt)

        Dim Lista = New List(Of Catalogo)

        Dim varCatalogo As Catalogo


        For Each dr In dt.Rows
            varCatalogo = New Catalogo
            varCatalogo.Catalogo(dr("Clave"), dr("Descripcion"))
            Lista.Add(varCatalogo)
        Next

        Return Lista
    End Function

    Private Sub BindDummyRow()
        Dim dummy As New DataTable()
        dummy.Columns.Add("Clave")
        dummy.Columns.Add("Descripcion")

        dummy.Rows.Add()
        gvd_Catalogo.DataSource = dummy
        gvd_Catalogo.DataBind()
    End Sub

    Private Function ObtieneElementos(ByRef Gvd As GridView, ByVal Catalogo As String, ByVal blnTexto As Boolean) As String
        Dim strDatos As String = ""
        For Each row As GridViewRow In Gvd.Rows
            Dim Elemento = DirectCast(row.FindControl("chk_Sel" & Catalogo), HiddenField)
            If Elemento.Value <> "true" Then
                strDatos = strDatos & IIf(blnTexto, ",''", ",") & DirectCast(row.FindControl("lbl_Clave" & Catalogo), Label).Text & IIf(blnTexto, "''", "")
            End If
        Next

        If Len(strDatos) > 0 Then
            strDatos = Mid(strDatos, 2, Len(strDatos) - 1)
        End If

        Return strDatos
    End Function

    Private Function ConsultaOrdenesPago() As DataTable
        Dim FiltroFechaGen As String = ""
        Dim FiltroUsuario As String = ""
        Dim FiltroEstatus As String = ""
        Dim Elementos As String
        Dim sCnn As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Elementos = ObtieneElementos(gvd_Usuario, "Usu", True)
        If Elementos <> "" Then
            FiltroUsuario = Elementos
        End If

        Elementos = ObtieneElementos(gvd_Estatus, "Est", True)
        If Elementos <> "" Then
            FiltroEstatus = Elementos
        End If

        If IsDate(txtFecGeneraDe.Text) And IsDate(txtFecGeneraA.Text) Then
            If CDate(txtFecGeneraDe.Text) <= CDate(txtFecGeneraA.Text) Then
                FiltroFechaGen = " AND fec_generacion >= ''" & FechaAIngles(txtFecGeneraDe.Text) & "'' AND fec_generacion < ''" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txtFecGeneraA.Text))) & "''"
            End If
        End If

        Dim sSel As String = "spS_OrdenPago 0,'','','','','','" & FiltroUsuario & "','" & FiltroEstatus & "','" & FiltroFechaGen & "'"

        Dim da As SqlDataAdapter

        dtBusqueda = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.SelectCommand.CommandTimeout = 10000

        da.Fill(dtBusqueda)

        Session("dtOrdenPago") = dtBusqueda

        Return dtBusqueda

    End Function

    Private Sub btn_OkCatalogo_Click(sender As Object, e As EventArgs) Handles btn_OkCatalogo.Click
        Try

            Dim dtGridView As DataTable
            Dim Datos() As String
            dtGridView = New DataTable
            Dim Seleccionados As String = hid_Seleccion.Value
            If Len(Seleccionados) > 0 Then
                Dim Catalogo As String = hid_Catalogo.Value

                Dim GvdFiltro As GridView
                GvdFiltro = Nothing
                Select Case Catalogo
                    Case "Usu"
                        GvdFiltro = gvd_Usuario

                    Case "Est"
                        GvdFiltro = gvd_Estatus
                End Select

                dtGridView.Columns.Add("Clave")
                dtGridView.Columns.Add("Descripcion")

                For Each row As GridViewRow In GvdFiltro.Rows
                    Dim Elemento = DirectCast(row.FindControl("chk_Sel" & Catalogo), HiddenField)
                    If Elemento.Value <> "true" Then
                        Dim Fila As DataRow = dtGridView.NewRow()
                        Fila("Clave") = DirectCast(row.FindControl("lbl_Clave" & Catalogo), Label).Text
                        Fila("Descripcion") = DirectCast(row.FindControl("lbl_Desc"), Label).Text
                        dtGridView.Rows.Add(Fila)
                    End If
                Next


                Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")
                For Each dato In Datos
                    Dim Fila As DataRow = dtGridView.NewRow()
                    Fila("Clave") = Split(dato, "~")(0)
                    Fila("Descripcion") = Split(dato, "~")(1)
                    dtGridView.Rows.Add(Fila)
                Next

                GvdFiltro.DataSource = dtGridView
                GvdFiltro.DataBind()
            End If

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: OK Catalogo", ex.Message)
            LogError("(btn_OkCatalogo_Click)" & ex.Message)
        End Try
    End Sub

    Private Sub btn_BuscaOP_Click(sender As Object, e As EventArgs) Handles btn_BuscaOP.Click
        Try
            Dim Ramos() As String

            gvd_LstOrdenPago.DataSource = ConsultaOrdenesPago()
            gvd_LstOrdenPago.DataBind()

            If gvd_LstOrdenPago.Rows.Count > 0 Then
                txtFecGeneraDe.Enabled = False
                txtFecGeneraA.Enabled = False
                gvd_Usuario.Enabled = False
                gvd_Estatus.Enabled = False
                btn_BuscaOP.Enabled = False
                btn_Imprimir.Enabled = True
                btn_Solicitante.Enabled = True
                btn_Jefe.Enabled = True
                btn_Direccion.Enabled = True
                btn_Contabilidad.Enabled = True


                For Each Row In gvd_LstOrdenPago.Rows
                    Dim lbl_RamosContables As Label = Row.FindControl("lbl_RamosContables")
                    Dim ddl_RamosContables As DropDownList = Row.FindControl("ddl_RamosContables")

                    Ramos = Split(lbl_RamosContables.Text, "|")
                    For intPos = 1 To UBound(Ramos)
                        ddl_RamosContables.Items.Add(Ramos(intPos))
                    Next
                    ddl_RamosContables.SelectedIndex = 0
                Next
            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: BUSQUEDA ORDENES DE PAGO", ex.Message)
            LogError("(btn_BuscaOP_Click)" & ex.Message)
        End Try
    End Sub

    Private Sub btn_Limpiar_Click(sender As Object, e As EventArgs) Handles btn_Limpiar.Click
        Try
            gvd_LstOrdenPago.DataSource = Nothing
            gvd_LstOrdenPago.DataBind()
            Session("dtOrdenPago") = Nothing
            btn_Imprimir.Enabled = False
            btn_Solicitante.Enabled = False
            btn_Jefe.Enabled = False
            btn_Direccion.Enabled = False
            btn_Contabilidad.Enabled = False
            txtFecGeneraDe.Enabled = True
            txtFecGeneraA.Enabled = True
            gvd_Usuario.Enabled = True
            gvd_Estatus.Enabled = True
            btn_BuscaOP.Enabled = True
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: LIMPIA ORDENES DE PAGO", ex.Message)
            LogError("(btn_Limpiar_Click)" & ex.Message)
        End Try
    End Sub

    Private Function ActualizaDataOP() As DataTable
        Dim dtOrdenPago As DataTable
        dtOrdenPago = New DataTable
        dtOrdenPago = Session("dtOrdenPago")

        For Each row In gvd_LstOrdenPago.Rows
            Dim chk_SelOp = DirectCast(row.FindControl("chk_SelOp"), CheckBox)
            Dim chk_Impresion = DirectCast(row.FindControl("chk_Impresion"), CheckBox)
            Dim lbl_OrdenPago = DirectCast(row.FindControl("lbl_OrdenPago"), LinkButton)
            Dim myRow() As Data.DataRow
            myRow = dtOrdenPago.Select("nro_op ='" & lbl_OrdenPago.Text & "'")

            myRow(0)("tSEl_Val") = chk_SelOp.Checked
            myRow(0)("sn_impresion") = chk_Impresion.Checked
        Next

        Session("dtOrdenPago") = dtOrdenPago

        Return dtOrdenPago
    End Function


    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Try
            Dim dtOrdenPago As DataTable

            If txt_usuario.Text = "" Then
                txt_usuario.Focus()
                Mensaje("ORDEN DE PAGO-: AUTORIZA ORDENES DE PAGO", "Se debe capturar el usuario")
                Exit Sub
            End If
            If txt_contraseña.Text = "" Then
                txt_contraseña.Focus()
                Mensaje("ORDEN DE PAGO-: AUTORIZA ORDENES DE PAGO", "Se debe capturar la contraseña")
                Exit Sub
            End If

            ActualizaDataOP()
            dtOrdenPago = New DataTable
            dtOrdenPago = Session("dtOrdenPago")
            Dim myRow() As Data.DataRow
            myRow = dtOrdenPago.Select("tSEl_Val ='" & True & "'")
            If myRow.Count = 0 Then
                Mensaje("ORDEN DE PAGO-: AUTORIZA ORDENES DE PAGO", "No se ha seleccionado ninguna orden de Pago para autorizar")
                Exit Sub
            End If

            If IsAuthenticated("GMX.COM.MX", txt_usuario.Text, txt_contraseña.Text) Then
                If ConsultaUsuario(txt_usuario.Text) <> "" Then
                    txt_contraseña.Text = ""
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Modal Autoriza", "ClosePopup('#AutorizaModal');", True)
                Else
                    Mensaje("ORDEN DE PAGO-: AUTORIZA ORDENES DE PAGO", "No cuenta con acceso a SII")
                End If
            Else
                Mensaje("ORDEN DE PAGO-: AUTORIZA ORDENES DE PAGO", "Usuario y/o Contraseña de Red incorrectos o su cuenta esta bloqueada")
            End If

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: AUTORIZA ORDENES DE PAGO", ex.Message)
            LogError("(btnAceptar_Click)" & ex.Message)
        End Try
    End Sub

    <System.Web.Services.WebMethod>
    Public Shared Function IsAuthenticated(ByVal Domain As String, ByVal username As String, ByVal pwd As String) As Boolean
        Dim Success As Boolean = False
        Dim Entry As New System.DirectoryServices.DirectoryEntry("LDAP://" & Domain, username, pwd)
        Dim Searcher As New System.DirectoryServices.DirectorySearcher(Entry)
        Searcher.SearchScope = DirectoryServices.SearchScope.OneLevel
        Try
            Dim Results As System.DirectoryServices.SearchResult = Searcher.FindOne
            Success = Not (Results Is Nothing)
        Catch
            Success = False
        End Try
        Return Success
    End Function

    Private Function ConsultaUsuario(ByVal cod_usuario_NT As String) As String
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

    Private Sub gvd_LstOrdenPago_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvd_LstOrdenPago.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim fec_baja As String = sender.DataKeys(e.Row.RowIndex)("fec_baja")
                Dim fec_autoriz_sector As String = sender.DataKeys(e.Row.RowIndex)("fec_autoriz_sector")
                Dim fec_autoriz_contab As String = sender.DataKeys(e.Row.RowIndex)("fec_autoriz_contab")
                Dim fec_pago As String = sender.DataKeys(e.Row.RowIndex)("fec_pago")
                Dim id_pv As Integer = sender.DataKeys(e.Row.RowIndex)("id_pv")
                Dim cod_estatus_op As Integer = sender.DataKeys(e.Row.RowIndex)("cod_estatus_op")
                Dim Duplicado As Integer = sender.DataKeys(e.Row.RowIndex)("Duplicado")

                Dim lbl_OrdenPago As LinkButton = TryCast(e.Row.FindControl("lbl_OrdenPago"), LinkButton)
                Dim lbl_Estatus As Label = TryCast(e.Row.FindControl("lbl_Estatus"), Label)

                Dim btnCorregir As Button = TryCast(e.Row.FindControl("btnCorregir"), Button)
                Dim btnRestaurar As Button = TryCast(e.Row.FindControl("btnRestaurar"), Button)

                If Duplicado = 1 Then
                    lbl_OrdenPago.BackColor = Drawing.Color.Red
                    lbl_Estatus.BackColor = Drawing.Color.Red
                    lbl_OrdenPago.ForeColor = Drawing.Color.White
                    lbl_Estatus.ForeColor = Drawing.Color.White
                ElseIf id_pv = 0 Then
                    lbl_OrdenPago.BackColor = Drawing.Color.Orange
                    lbl_Estatus.BackColor = Drawing.Color.Orange
                    lbl_OrdenPago.ForeColor = Drawing.Color.Black
                    lbl_Estatus.ForeColor = Drawing.Color.Black
                ElseIf IsDate(fec_baja) Then
                    lbl_OrdenPago.BackColor = Drawing.Color.Gray
                    lbl_Estatus.BackColor = Drawing.Color.Gray
                    lbl_OrdenPago.ForeColor = Drawing.Color.White
                    lbl_Estatus.ForeColor = Drawing.Color.White
                    'btnCorregir.Visible = False
                ElseIf cod_estatus_op = 9 Then
                    lbl_OrdenPago.BackColor = Drawing.Color.DarkBlue
                    lbl_Estatus.BackColor = Drawing.Color.DarkBlue
                    lbl_OrdenPago.ForeColor = Drawing.Color.White
                    lbl_Estatus.ForeColor = Drawing.Color.White
                    btnCorregir.Visible = False
                    btnRestaurar.Visible = False
                ElseIf IsDate(fec_pago) Then
                    lbl_OrdenPago.BackColor = Drawing.Color.Green
                    lbl_Estatus.BackColor = Drawing.Color.Green
                    lbl_OrdenPago.ForeColor = Drawing.Color.White
                    lbl_Estatus.ForeColor = Drawing.Color.White
                    'btnCorregir.Visible = False
                    'btnRestaurar.Visible = False
                ElseIf IsDate(fec_autoriz_sector) And Not IsDate(fec_autoriz_contab) Then
                    lbl_OrdenPago.BackColor = Drawing.Color.LightCyan
                    lbl_Estatus.BackColor = Drawing.Color.LightCyan
                ElseIf IsDate(fec_autoriz_contab) Then
                    lbl_OrdenPago.BackColor = Drawing.Color.LightBlue
                    lbl_Estatus.BackColor = Drawing.Color.LightBlue
                    lbl_OrdenPago.ForeColor = Drawing.Color.DarkBlue
                    lbl_Estatus.ForeColor = Drawing.Color.DarkBlue
                    btnCorregir.Visible = False
                    btnRestaurar.Visible = False
                Else
                    lbl_OrdenPago.BackColor = Drawing.Color.Yellow
                    lbl_Estatus.BackColor = Drawing.Color.Yellow
                End If
            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: LISTADO ORDENES DE PAGO", ex.Message)
            LogError("(gvd_LstOrdenPago_RowDataBound)" & ex.Message)
        End Try
    End Sub

    Private Sub gvd_LstOrdenPago_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvd_LstOrdenPago.PageIndexChanging
        Try
            Dim Ramos() As String

            gvd_LstOrdenPago.PageIndex = e.NewPageIndex
            gvd_LstOrdenPago.DataSource = ActualizaDataOP()
            gvd_LstOrdenPago.DataBind()

            For Each Row In gvd_LstOrdenPago.Rows
                Dim lbl_RamosContables As Label = Row.FindControl("lbl_RamosContables")
                Dim ddl_RamosContables As DropDownList = Row.FindControl("ddl_RamosContables")

                Ramos = Split(lbl_RamosContables.Text, "|")
                For intPos = 1 To UBound(Ramos)
                    ddl_RamosContables.Items.Add(Ramos(intPos))
                Next
                ddl_RamosContables.SelectedIndex = 0
            Next

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: PAGINACIÓN ORDENES DE PAGO", ex.Message)
            LogError("(gvd_LstOrdenPago_PageIndexChanging)" & ex.Message)
        End Try
    End Sub

    Private Sub btn_CerrarSesion_Click(sender As Object, e As EventArgs) Handles btn_CerrarSesion.Click
        Try
            If hid_CierraSesion.Value = 0 Then
                Dim ConsultaBD As ConsultaBD
                ConsultaBD = New ConsultaBD
                ConsultaBD.InsertaBitacora(ConsultaBD.ConsultaUsuarioNT(Master.cod_usuario), Master.HostName, "Cierre de Sesión", "Cierre Exitoso (Ordenes de Pago)")
                hid_CierraSesion.Value = 1
            End If

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(btn_CerrarSesion_Click)" & ex.Message)
        End Try
    End Sub

    Private Sub btn_Imprimir_Click(sender As Object, e As EventArgs) Handles btn_Imprimir.Click
        Try


            Dim dtOrdenPago As DataTable
            dtOrdenPago = New DataTable
            dtOrdenPago = ActualizaDataOP()

            Dim strOrdenPago As String = vbNullString

            For Each Row In dtOrdenPago.Rows
                If Row("sn_impresion") Then
                    strOrdenPago = strOrdenPago & Row("nro_op") & ","
                End If
            Next
            If Len(strOrdenPago) > 0 Then
                strOrdenPago = Mid(strOrdenPago, 1, Len(strOrdenPago) - 1)

                Dim server As String = "http://siigmxapp02/ReportServer_SIIGMX02?%2fReportesGMX%2fOrdenPago&rs%3AFormat=PDF&rc:Parameters=false&nro_op=@nro_op"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "ImprimirOrden", "ImprimirOrden('" & server & "','" & strOrdenPago & "');", True)
            End If


        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(btn_Imprimir_Click)" & ex.Message)
        End Try
    End Sub

    Private Function LogError(ByVal strError As String) As Boolean
        Dim ConsultaBD As ConsultaBD
        ConsultaBD = New ConsultaBD
        ConsultaBD.InsertaLogError(Master.cod_usuario, Master.HostName, Replace(Replace(strError, "'", ""), vbCrLf, " "))
        Return True
    End Function

    Private Function Mensaje(ByVal strSegmento As String, ByVal strMsg As String) As Boolean
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Mensaje", "EvaluaMensaje('" & strSegmento & "','" & Replace(Replace(strMsg, "'", ""), vbCrLf, " ") & "');", True)
        'hid_Mensaje.Value = strSegmento & ":" & vbCrLf & strMsg
        Return True
    End Function

    Private Function FechaAIngles(ByVal Fecha As String) As String
        If Fecha <> vbNullString Then
            'Return String.Format("{0:MM/dd/yyyy}", CDate(Fecha))
            Return String.Format("{0:dd/MM/yyyy}", CDate(Fecha))
        Else
            Return ""
        End If

    End Function


    Protected Sub chk_FirmaSol_CheckedChanged(sender As Object, e As EventArgs)
        'For Each row In gvd_LstOrdenPago.Rows
        '    Dim chkSol = DirectCast(row.FindControl("chk_FirmaSol"), CheckBox)
        '    If chkSol.Checked = True Then
        '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "Open Modal Autoriza", "OpenPopup('#AutorizaModal');", True)

        '    End If
        'Next
    End Sub
    Private Sub LlenaUsuarioFirma(tPersona As Integer)
        Dim wsGmx As New GMXServices.GeneralesClient
        Dim ListaResultado As IList = Nothing
        ListaResultado = wsGmx.ObtieneUsuarioFirmaE(tPersona)
        ddl_Usuarios.Items.Clear()
        ddl_Usuarios.DataValueField = "clave"
        ddl_Usuarios.DataTextField = "descripcion"
        Dim registro As ListItem
        For Each item In ListaResultado
            registro = New ListItem(item.descripcion, item.clave)
            ddl_Usuarios.Items.Add(registro)
        Next
        ddl_Usuarios.DataBind()
        If ddl_Usuarios.Items.Count > 0 Then
            ddl_Usuarios.SelectedIndex = 0
        End If
    End Sub
    Private Sub btn_Solicitante_Click(sender As Object, e As EventArgs) Handles btn_Solicitante.Click
        lbl_Asocia.Text = "Jefe Inmediato"
        LlenaUsuarioFirma(TipoPersona.JefeInmediato)
    End Sub

    Private Sub btn_Jefe_Click(sender As Object, e As EventArgs) Handles btn_Jefe.Click
        lbl_Asocia.Text = "Director de Area"
        LlenaUsuarioFirma(TipoPersona.DirectorArea)
    End Sub
    Private Sub btn_Direccion_Click(sender As Object, e As EventArgs) Handles btn_Direccion.Click
        lbl_Asocia.Text = "P. Contabilidad"
        LlenaUsuarioFirma(TipoPersona.Contabilidad)
    End Sub
End Class
