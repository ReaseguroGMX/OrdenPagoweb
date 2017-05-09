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
        Subdirector
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




                If Session("Origen") = 1 Then
                    btn_BuscaOP_Click(sender, e)
                End If
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

        Dim sSel As String = ""
        If Session("Origen") = 1 Then
            sSel = "spS_OrdenPago '" & Session("NumOrds").ToString() & "','','','','','','" & FiltroUsuario & "','" & FiltroEstatus & "','" & FiltroFechaGen & "','" & Master.cod_usuario & "'"
            Session.Remove("Origen")
            Session.Remove("NumOrds")
        Else
            sSel = "spS_OrdenPago 0,'','','','','','" & FiltroUsuario & "','" & FiltroEstatus & "','" & FiltroFechaGen & "','" & Master.cod_usuario & "'"
        End If

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

                btn_Firmar.Enabled = True


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

            DesHabilitaChecksFirma()

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
            btn_Firmar.Enabled = False
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
                    Dim strNumOrds = CategorizaOPs()
                    Dim arrNumOrds() As String = strNumOrds.Split("|")
                    'UPDATE A FIRMAS
                    If Not arrNumOrds(0) = vbNullString Then ActualizaFirmas(arrNumOrds(0), 0, Master.cod_usuario) 'Solicitante
                    If Not arrNumOrds(1) = vbNullString Then ActualizaFirmas(arrNumOrds(1), 1, Master.cod_usuario) 'Jefe Inmediato
                    If Not arrNumOrds(2) = vbNullString Then ActualizaFirmas(arrNumOrds(2), 4, Master.cod_usuario) 'Subdirector
                    If Not arrNumOrds(3) = vbNullString Then ActualizaFirmas(arrNumOrds(3), 2, Master.cod_usuario) 'Dir Area
                    If Not arrNumOrds(4) = vbNullString Then ActualizaFirmas(arrNumOrds(4), 3, Master.cod_usuario) 'Conta

                    'ENVIO DE MAILS
                    If Not arrNumOrds(0) = vbNullString Then EnviaMail(1, Master.cod_usuario, arrNumOrds(0)) 'Solicitante
                    If Not arrNumOrds(1) = vbNullString Then EnviaMail(2, Master.cod_usuario, arrNumOrds(1)) 'Jefe Inmediato
                    If Not arrNumOrds(2) = vbNullString Then EnviaMail(3, Master.cod_usuario, arrNumOrds(2)) 'Subdirector
                    If Not arrNumOrds(3) = vbNullString Then EnviaMail(4, Master.cod_usuario, arrNumOrds(3)) 'Dir Area

                    Mensaje("FIRMA ELECTRONICA", "Se autorizo correctamente")
                    DesHabilitaChecksFirma()

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
    ''' <summary>
    ''' Actualiza en BD las firmas
    ''' </summary>
    Private Sub ActualizaFirmas(NumOp As String, TipoPer As Integer, CodUsu As String)
        Dim wsGmx As New GMXServices.GeneralesClient
        Dim Resultado As Integer = 0
        Try
            Resultado = wsGmx.ActualizaFirma(NumOp, TipoPer, CodUsu)
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: FIRMA ORDENES DE PAGO: ActualizaFirmas", ex.Message)
            LogError("(btnAceptar_Click)" & ex.Message)
        End Try
    End Sub
    Private Sub EnviaMail(TipoPer As Integer, CodUSu As String, strNumOrds As String)
        Dim wsGMXService As New GMXServices.GeneralesClient
        Try
            Dim strBody As String
            Dim Mail As String
            Mail = ObtieneUsuarioMail(TipoPer)

            strBody = FormatoCorreo(strNumOrds, Master.Usuario, TipoPer)
            wsGMXService.EnviaCorreo("martinem@gmx.com.mx", strBody, "Solicitud de Firma de OPs", "", "")
        Catch ex As Exception
            Mensaje("FIRMA ELECTRONICA-: Envio de Correo", ex.Message)
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

    Private Function ObtieneUsuarioMail(tPersona As Integer) As String
        Dim wsGmx As New GMXServices.GeneralesClient
        Dim ListaResultado As IList = Nothing
        Dim strDestinatarios As String = ""
        Try
            ListaResultado = wsGmx.ObtieneUsuarioFirmaE(tPersona)
            ddl_Destinatario.DataSource = ListaResultado
            ddl_Destinatario.DataTextField = "descripcion"
            ddl_Destinatario.DataValueField = "clave"
            ddl_Destinatario.DataBind()
            'For Each item In ListaResultado
            '    strDestinatarios += item.mail & ","
            'Next
            'strDestinatarios = Left(strDestinatarios, Len(strDestinatarios) - 1)
        Catch ex As Exception
            Return vbNullString
        End Try
        Return strDestinatarios
    End Function

    Private Function ObtieneUsuarios(tPersona As Integer, Optional _Default As Integer = 0) As String
        Dim wsGmx As New GMXServices.GeneralesClient
        Dim ListaResultado As IList = Nothing
        Dim strDestinatarios As String = ""
        Try
            ListaResultado = wsGmx.ObtieneUsuarioFirmaE(tPersona)
            For Each item In ListaResultado
                strDestinatarios += item.mail & ","
            Next
            strDestinatarios = Left(strDestinatarios, Len(strDestinatarios) - 1)
        Catch ex As Exception
            Return vbNullString
        End Try
        Return strDestinatarios
    End Function

    Protected Sub chk_FirmaSol_CheckedChanged(sender As Object, e As EventArgs)
        SelectRow(sender, "chk_FirmaSol")
    End Sub

    Protected Sub chk_FirmaJefe_CheckedChanged(sender As Object, e As EventArgs)
        SelectRow(sender, "chk_FirmaJefe")
    End Sub
    Protected Sub chk_FirmaDir_CheckedChanged(sender As Object, e As EventArgs)
        SelectRow(sender, "chk_FirmaDir")
    End Sub
    Protected Sub chk_FirmaCon_CheckedChanged(sender As Object, e As EventArgs)
        SelectRow(sender, "chk_FirmaCon")
    End Sub
    Protected Sub chk_SubDir_CheckedChanged(sender As Object, e As EventArgs)
        SelectRow(sender, "chk_SubDir")
    End Sub

    Private Function CategorizaOPs() As String
        Dim strOPSubDir As String = ""
        Dim strOPConta As String = ""
        Dim strOPJefe As String = ""
        Dim strOPDirArea As String = ""
        Dim strOPSol As String = ""
        Dim strFinal As String = ""

        For Each row In gvd_LstOrdenPago.Rows
            Dim chkSubDir = DirectCast(row.FindControl("chk_SubDir"), CheckBox)
            Dim chkConta = DirectCast(row.FindControl("chk_FirmaCon"), CheckBox)
            Dim chkDirArea = DirectCast(row.FindControl("chk_FirmaDir"), CheckBox)
            Dim chkJefe = DirectCast(row.FindControl("chk_FirmaJefe"), CheckBox)
            Dim chkSol = DirectCast(row.FindControl("chk_FirmaSol"), CheckBox)
            Dim lbl_OrdenPago = DirectCast(row.FindControl("lbl_OrdenPago"), LinkButton)

            If chkSubDir.Checked And chkSubDir.Enabled Then strOPSubDir += Trim(lbl_OrdenPago.Text) & ","
            If chkConta.Checked And chkConta.Enabled Then strOPConta += Trim(lbl_OrdenPago.Text) & ","
            If chkDirArea.Checked And chkDirArea.Enabled Then strOPDirArea += Trim(lbl_OrdenPago.Text) & ","
            If chkJefe.Checked And chkJefe.Enabled Then strOPJefe += Trim(lbl_OrdenPago.Text) & ","
            If chkSol.Checked And chkSol.Enabled Then strOPSol += Trim(lbl_OrdenPago.Text) & ","

        Next
        If Len(strOPSol) > 0 Then strOPSol = Left(strOPSol, Len(strOPSol) - 1)
        If Len(strOPJefe) > 0 Then strOPJefe = Left(strOPJefe, Len(strOPJefe) - 1)
        If Len(strOPDirArea) > 0 Then strOPDirArea = Left(strOPDirArea, Len(strOPDirArea) - 1)
        If Len(strOPConta) > 0 Then strOPConta = Left(strOPConta, Len(strOPConta) - 1)
        If Len(strOPSubDir) > 0 Then strOPSubDir = Left(strOPSubDir, Len(strOPSubDir) - 1)

        strFinal = strOPSol & "|" & strOPJefe & "|" & strOPSubDir & "|" & strOPDirArea & "|" & strOPConta

        Return strFinal

    End Function

    Private Sub SelectRow(sender As Object, CrtlNombre As String)
        Dim CrtlPrevio As String = ""
        Dim gr As GridViewRow = DirectCast(DirectCast(DirectCast(sender, CheckBox).Parent.Parent, DataControlFieldCell).Parent, GridViewRow)
        Dim chkCtrl As CheckBox = TryCast(gvd_LstOrdenPago.Rows(gr.RowIndex).FindControl(CrtlNombre), CheckBox)
        CrtlPrevio = ""
        If CrtlNombre = "chk_FirmaJefe" Then CrtlPrevio = "chk_FirmaSol"
        If CrtlNombre = "chk_SubDir" Then CrtlPrevio = "chk_FirmaJefe"
        If CrtlNombre = "chk_FirmaDir" Then CrtlPrevio = "chk_SubDir"
        If CrtlNombre = "chk_FirmaCon" Then CrtlPrevio = "chk_FirmaDir"
        Dim chkPrevio As CheckBox = TryCast(gvd_LstOrdenPago.Rows(gr.RowIndex).FindControl(CrtlPrevio), CheckBox)
        Dim chkSelRenglon As CheckBox = TryCast(gvd_LstOrdenPago.Rows(gr.RowIndex).FindControl("chk_SelOp"), CheckBox)

        If CrtlPrevio <> vbNullString Then
            If chkPrevio.Checked = True Then
                If chkCtrl.Checked = True And chkCtrl.Enabled = True Then
                    chkSelRenglon.Checked = True
                ElseIf chkCtrl.Checked = False And chkCtrl.Enabled = True Then
                    chkSelRenglon.Checked = False
                End If
            Else
                chkCtrl.Checked = False
                Mensaje("FIRMA ELECTRONICA:", "No puede firmar la Orden de Pago,si no cuenta con la firma previa.")
            End If
        Else
            If chkCtrl.Checked = True And chkCtrl.Enabled = True Then
                chkSelRenglon.Checked = True
            ElseIf chkCtrl.Checked = False And chkCtrl.Enabled = True Then
                chkSelRenglon.Checked = False
            End If
        End If

    End Sub

    Private Sub DesHabilitaChecksFirma()
        For Each row In gvd_LstOrdenPago.Rows
            Dim chkSol = DirectCast(row.FindControl("chk_FirmaSol"), CheckBox)
            Dim chkJefe = DirectCast(row.FindControl("chk_FirmaJefe"), CheckBox)
            Dim chkSubDir = DirectCast(row.FindControl("chk_SubDir"), CheckBox)
            Dim chkDir = DirectCast(row.FindControl("chk_FirmaDir"), CheckBox)
            Dim chkCon = DirectCast(row.FindControl("chk_FirmaCon"), CheckBox)

            If chkSol.Checked = True Then chkSol.Enabled = False
            If chkJefe.Checked = True Then chkJefe.Enabled = False
            If chkSubDir.Checked = True Then chkSubDir.Enabled = False
            If chkDir.Checked = True Then chkDir.Enabled = False
            If chkCon.Checked = True Then chkCon.Enabled = False
        Next
    End Sub

    Private Function FormatoCorreo(strNumOrds As String, UsuSol As String, TipoPer As Integer) As String
        Dim strTipoPersona As String = ""
        If TipoPer = 1 Then strTipoPersona = "Jefe Inmediato"
        If TipoPer = 2 Then strTipoPersona = "SubDirector"
        If TipoPer = 3 Then strTipoPersona = "Director de Área"
        If TipoPer = 4 Then strTipoPersona = "Contabilidad"

        Dim strBody As String = ""
        strBody = "<table style = 'margin: 0px; border: medium; border-image: none; border-collapse: collapse;' border='1' cellspacing='0' cellpadding='0'>"
        strBody += "<tbody> <tr style = 'mso-yfti-irow:0;mso-yfti-firstrow:yes;mso-yfti-lastrow:yes' >"
        strBody += "<td width='395' valign='top' style='margin: 0px; padding: 0cm 5.4pt; border: 1px solid rgb(0, 0, 0); border-image: none; width: 296pt; background-color: transparent;'>"
        strBody += "<p style='margin: 0px; line-height: normal;'><span style='margin: 0px; color: rgb(0, 32, 96);'><img width='74' height='74' src='\\gmxqroapp02\inetpub\wwwroot\OrdenPago\Images\Firmas\logo_gmx.jpg' v:shapes='Imagen_x0020_2'></span><span style='margin: 0px; color: rgb(0, 32, 96);'><span style='margin: 0px;'><font face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp; </font></span><span style='margin: 0px;'><font face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></span></span><b style='mso-bidi-font-weight:&#10;  normal'><span style='margin: 0px; color: rgb(0, 32, 96); font-family: ' Myriad Pro',' sans-serif'; font-size: 16pt;'>GMX Seguros</span></b></p>"
        strBody += "<p style='margin: 0px; line-height: normal;'><span style='margin: 0px; color: rgb(0, 32, 96); font-family: ' Myriad Pro',' sans-serif';'>Autorización"
        strBody += " de Órdenes de Pago.</span></p>"
        strBody += "<p style='margin: 0px; line-height: normal;'><span style='margin: 0px; color: rgb(0, 32, 96); font-family: ' Myriad Pro',' sans-serif';'>&nbsp;</span></p>"
        strBody += "<p style='margin: 0px; line-height: normal;'><span style='margin: 0px; color: rgb(0, 32, 96); font-family: ' Myriad Pro',' sans-serif';'>Se solicita por parte de <b style='mso-bidi-font-weight:normal'>" & UsuSol & " </b>, sean firmadas"
        strBody += " las siguientes órdenes de pago por <b style='mso-bidi-font-weight:normal'> " & strTipoPersona & ": </b> </span></p>"
        strBody += "<p style='margin: 0px; line-height: normal;'><span style='margin: 0px; color: rgb(0, 32, 96); font-family: ' Myriad Pro',' sans-serif';'><b style='mso-bidi-font-weight:normal'>" & strNumOrds & "</b></span></p>"
        strBody += "<p style='margin: 0px; line-height: normal;'><span style='margin: 0px; color: rgb(0, 32, 96); font-family: ' Myriad Pro',' sans-serif';'>Para"
        strBody += " firmarlas electrónicamente dar clic en el enlace debajo:</span></p>"
        strBody += "<p align='center' style='margin: 0px; text-align: center; line-height: normal;'><a href='"
        strBody += ArmaLinkMail(strNumOrds)
        strBody += "'><font color='#0000ff' face='Calibri'>Click Aqui</font></a></p>"
        strBody += "<p style='margin: 0px; line-height: normal;'><font face='Calibri'>&nbsp;</font></p>"
        strBody += "</td>"
        strBody += "<td width = '204' valign='top' style='background: rgb(15, 36, 62); border-width: 1px 1px 1px 0px; border-style: solid solid solid none; border-color: rgb(0, 0, 0); margin: 0px; padding: 0cm 5.4pt; border-image: none; width: 152.9pt;'>"
        strBody += "<p align='center' style='margin 0px; text-align: center; line-height: normal;'><img width='62' height='62' src='\\gmxqroapp02\inetpub\wwwroot\OrdenPago\Images\Firmas\logomail.png' v:shapes='_x0000_i1025'></p>"
        strBody += " <br> "
        strBody += "<p align ='center' style='margin 0px; text-align: center; line-height: normal;'><img width='62' height='62' src='\\gmxqroapp02\inetpub\wwwroot\OrdenPago\Images\Firmas\icono-pago_blanco.png' v:shapes='_x0000_i1025'></p>"
        strBody += "</td>"
        strBody += "</tr>"
        strBody += "</tbody></table>"

        Return strBody

    End Function

    Private Function ArmaLinkMail(strNumOrds As String) As String
        Dim strLink As String = "http://gmxdfcnd5341lbq/OrdenPagoWeb/Pages/Login.aspx"
        Dim strParametros As String
        strParametros = "?Origen=1&NumOrds=" & strNumOrds
        strLink += strParametros
        Return strLink

    End Function

    Protected Sub lnk_SelJefe_Click(sender As Object, e As EventArgs)
        ObtieneUsuarioMail(1)
    End Sub
End Class
