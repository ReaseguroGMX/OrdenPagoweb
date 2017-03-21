Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Net.Mail
Imports System.IO
Imports System.Text
Imports System.Drawing.Imaging

Partial Class Pages_OrdenPago
    Inherits System.Web.UI.Page

    Private dtPolizas As DataTable
    Private dtBusqueda As DataTable
    Dim dtDetalle As DataTable
    Private clCatalogo As Catalogo

    Private Enum Poliza
        cod_suc = 0
        ramo_pol
        nro_pol
        aaaa_endoso
        nro_endoso
    End Enum

    Private Enum enRecordsetEnum
        enGenerales
        enReaseguro
        enImputacion
    End Enum

    Private dtContratosPol As DataTable
    Private Resultados(3) As DataTable

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

    Private Sub BindDummyRow()
        Dim dummy As New DataTable()
        dummy.Columns.Add("Clave")
        dummy.Columns.Add("Descripcion")

        dummy.Rows.Add()
        gvd_Catalogo.DataSource = dummy
        gvd_Catalogo.DataBind()
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

    <System.Web.Services.WebMethod>
    Public Shared Function ObtieneDatosRTF(id_pv As String) As String
        Dim RespWcf As String
        Dim output As String = ""
        Dim wcfConn As New GMXServices.GeneralesClient
        Try
            RespWcf = wcfConn.ObtieneAclaraciones(id_pv)
            Dim StrDescripcion As String = ""
            If Not RespWcf Is Nothing Then
                Return RespWcf
                'ScriptManager.RegisterStartupScript(Me, Me.GetType, "Open Modal Ordenes", "OpenPopup('#AclaracionesModal');", True)
            End If

        Catch ex As Exception
            Return Nothing     ' Mensaje("ORDEN DE PAGO:ObtieneAclaraciones: ", ex.Message)
        End Try


    End Function


    Private Sub Pages_OrdenPago_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then


                Session.Timeout = 60000

                Session.Add("blnExpiro", "1")

                Session.Add("dtGeneral", Nothing)
                BindDummyRow()
                LlenaSucursal()
                LlenaMoneda()
                Header.Title = "GMX - Ordenes de Pago"
                Session.Add("dtCuotas", Nothing)
                Session.Add("sSel", Nothing)
                Session.Add("dtPolizas", Nothing)

                Master.Usuario = IIf(Session("Usuario") = "", "Inicia Sesión", Session("Usuario"))
                Master.cod_usuario = Session("cod_usuario")
                Master.cod_suc = Session("cod_suc")
                Master.cod_sector = Session("cod_sector")

                Master.HostName = Session("HostName")
                hid_Url.Value = "VistaOrdenPago.aspx?cod_usuario=" & Master.cod_usuario & "&Usuario=" & Master.Usuario & "&cod_suc=" & Master.cod_suc & "&cod_sector=" & Master.cod_sector

                ' Verifica que se haya ingresado a partir de Login
                If Master.cod_usuario = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Redirect", "Redireccionar('Login.aspx');", True)
                End If
                hid_CierraSesion.Value = 0

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Menu", "openNav();", True)

            Else
                If Session("blnExpiro") Is Nothing And hid_CierraSesion.Value = 0 Then
                    EstadoBloqueoSesion()
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

    Private Sub EstadoBloqueoSesion()
        hid_Ventanas.Value = "1|1|1|1|1|1|"
        gvd_Broker.Enabled = False
        gvd_Compañia.Enabled = False
        gvd_Contrato.Enabled = False
        txt_FechaDe.Enabled = False
        txt_FechaA.Enabled = False
        gvd_Poliza.Enabled = False
        txtFecPagoDe.Enabled = False
        txtFecPagoA.Enabled = False
        opt_Cobranzas.Enabled = False
        'txtClaveRamCont.Enabled = False
        'txtSearchRamCont.Enabled = False
        opt_Estatus.Enabled = False
        gvd_LstOrdenPago.Enabled = False
        gvd_Usuario.Enabled = False
        gvd_Estatus.Enabled = False
        btn_BuscaOP.Enabled = False
        btn_Imprimir.Enabled = False
        btn_Limpiar.Enabled = False
        btn_Buscar.Enabled = False
        btn_CnlBuscar.Enabled = False
        gvd_Reaseguro.Enabled = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "Inhabilita", "DisableControls();", True)
    End Sub

    Private Sub LlenaSucursal()
        clCatalogo = New Catalogo
        ddl_SucursalPol.DataValueField = "Clave"
        ddl_SucursalPol.DataTextField = "Descripcion"
        ddl_SucursalPol.DataSource = clCatalogo.ObtieneCatalogo("Suc")
        ddl_SucursalPol.DataBind()
    End Sub

    Private Sub LlenaMoneda()
        clCatalogo = New Catalogo
        ddl_Moneda.DataValueField = "Clave"
        ddl_Moneda.DataTextField = "Descripcion"
        ddl_Moneda.DataSource = clCatalogo.ObtieneCatalogo("Mon")
        ddl_Moneda.DataBind()

        ddl_MonedaEnd.DataValueField = "Clave"
        ddl_MonedaEnd.DataTextField = "Descripcion"
        ddl_MonedaEnd.DataSource = ddl_Moneda.DataSource
        ddl_MonedaEnd.DataBind()

        Dim lst As ListItem = New ListItem("...", "-1")

        ddl_Moneda.Items.Insert(0, lst)
    End Sub

    Public Sub gvd_Reaseguro_PreRender(sender As Object, e As EventArgs) Handles gvd_Reaseguro.PreRender
        Try
            gvd_Reaseguro.UseAccessibleHeader = False
            If gvd_Reaseguro.Rows.Count > 0 Then
                gvd_Reaseguro.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:PreRender Reaseguro", ex.Message)
            LogError("gvd_Reaseguro_PreRender" & ex.Message)
        End Try
    End Sub

    Private Sub btn_Buscar_Click(sender As Object, e As EventArgs) Handles btn_Buscar.Click
        Dim FiltroContrato As String = ""
        Dim FiltroBroCia As String = ""
        Dim FiltroPol As String = ""
        Dim FiltroFecha As String = ""
        Dim FiltroRamoCont As String = ""
        Dim FiltroRamoTec As String = ""
        Dim FiltroOP As String = ""
        Dim FiltroMoneda As String = ""
        Dim Elementos As String
        Try


            Dim sCnn As String

            sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

            Elementos = ObtieneElementos(gvd_Broker, "Bro", False)
            If Elementos <> "" Then
                FiltroBroCia = " AND mr.cod_cia_reas_brok IN (" & Elementos & ")"
            End If

            Elementos = ObtieneElementos(gvd_Compañia, "Cia", False)
            If Elementos <> "" Then
                FiltroBroCia = FiltroBroCia & " AND mr.cod_cia_reas_cia IN(" & Elementos & ")"
            End If


            Elementos = ObtieneElementos(gvd_Contrato, "Fac", True)
            If Elementos <> "" Then
                FiltroContrato = " AND mr.id_Contrato IN(" & Elementos & ")"
            End If


            Elementos = ObtieneElementos(gvd_Poliza, "Pol", True)
            If Elementos <> "" Then
                FiltroPol = Elementos
            End If

            If IsDate(txtFecPagoDe.Text) And IsDate(txtFecPagoA.Text) Then
                If CDate(txtFecPagoDe.Text) <= CDate(txtFecPagoA.Text) Then
                    FiltroFecha = " AND EXH.fecha >= ''" & FechaAIngles(txtFecPagoDe.Text) & "'' AND EXH.fecha < ''" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txtFecPagoA.Text))) & "''"
                End If
            End If

            Elementos = ObtieneElementos(gvd_RamoContable, "RamC", False)
            If Elementos <> "" Then
                FiltroRamoCont = " AND mri.cod_ramo_contable IN (" & Elementos & ")"
            End If

            Elementos = ObtieneElementos(gvd_Producto, "Pro", False)
            If Elementos <> "" Then
                FiltroRamoTec = " AND cod_ramo IN (" & Elementos & ")"
            End If

            If ddl_Moneda.SelectedValue > -1 Then
                FiltroMoneda = " AND cod_moneda  = " & ddl_Moneda.SelectedValue
            End If

            If Not Session("dtOrdenPago") Is Nothing Then
                Dim dtOrdenPago As DataTable
                dtOrdenPago = New DataTable
                dtOrdenPago = ActualizaDataOP()

                Dim query = From q In (From p In dtOrdenPago.AsEnumerable()
                                       Where p("tSEl_Val") = True
                                       Select New With {.nro_op = p("nro_op")})
                            Select q.nro_op


                For Each Item In query
                    FiltroOP = FiltroOP & IIf(Len(FiltroOP) > 0, ",", "") & Item
                Next
            End If

            Dim sSel As String = "spS_LstOP_Pendiente '" & FiltroContrato & "','" & FiltroBroCia & "','" & FiltroPol & "','" & FiltroFecha & "','" & FiltroRamoCont & "','" & FiltroRamoTec & "'," & opt_Estatus.SelectedValue & "," & opt_Cobranzas.SelectedValue & ",'" & FiltroOP & "','" & FiltroMoneda & "'," & optAjuste.SelectedValue

            Session("sSel") = "spS_LstOP_Pendiente '" & FiltroContrato & "','" & FiltroBroCia & "','@strPolizas','" & FiltroFecha & "','" & FiltroRamoCont & "','" & FiltroRamoTec & "'," & opt_Estatus.SelectedValue & "," & opt_Cobranzas.SelectedValue & ",'" & FiltroOP & "','" & FiltroMoneda & "'," & optAjuste.SelectedValue

            Dim da As SqlDataAdapter

            dtBusqueda = New DataTable


            da = New SqlDataAdapter(sSel, sCnn)
            da.SelectCommand.CommandTimeout = 1200



            da.Fill(dtBusqueda)

            If dtBusqueda.Rows.Count > 0 Then
                ' hid_Ventanas.Value = "1|1|1|1|1|1|"

                gvd_Broker.Enabled = False
                gvd_Compañia.Enabled = False
                gvd_Contrato.Enabled = False

                txt_FechaDe.Enabled = False
                txt_FechaA.Enabled = False

                txtFecPagoA.Enabled = False
                txtFecPagoDe.Enabled = False

                txtFecGeneraDe.Enabled = False
                txtFecGeneraA.Enabled = False

                'txtClaveRamCont.Enabled = False
                'txtSearchRamCont.Enabled = False

                'txtClaveRamTec.Enabled = False
                'txtSearchRamTec.Enabled = False

                gvd_RamoContable.Enabled = False
                gvd_Producto.Enabled = False

                ddl_Moneda.Enabled = False

                gvd_Poliza.Enabled = False

                opt_Cobranzas.Enabled = False
                opt_Estatus.Enabled = False

                gvd_Usuario.Enabled = False
                gvd_Estatus.Enabled = False

                gvd_LstOrdenPago.Enabled = False
                btn_Imprimir.Enabled = False
                btn_Limpiar.Enabled = False
                btn_BuscaOP.Enabled = False

                optAjuste.Enabled = False
                'Respalda el Datatable
                Session("dtGeneral") = dtBusqueda

                gvd_Reaseguro.DataSource = dtBusqueda
                gvd_Reaseguro.DataBind()

                btn_Buscar.Enabled = False
                btn_CnlBuscar.Enabled = True

                Mensaje("ORDEN DE PAGO-: BUSQUEDA", "Se encontraron " & dtBusqueda.Rows.Count & " resultados")

            Else
                Mensaje("ORDEN DE PAGO-:Busqueda", "No se encontraron resultados")
            End If


        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:Busqueda", ex.Message)
            LogError("btn_Buscar_Click" & ex.Message)
        End Try
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

    Private Sub btn_CnlBuscar_Click(sender As Object, e As EventArgs) Handles btn_CnlBuscar.Click
        Try
            hid_Ventanas.Value = "0|0|0|0|0|"
            gvd_Reaseguro.PageIndex = 0

            gvd_CiasXBroker.DataSource = Nothing
            gvd_CiasXBroker.DataBind()

            gvd_Reaseguro.DataSource = Nothing
            gvd_Reaseguro.DataBind()

            gvd_Broker.Enabled = True
            gvd_Compañia.Enabled = True
            gvd_Contrato.Enabled = True

            txtFecPagoA.Enabled = True
            txtFecPagoDe.Enabled = True

            txt_FechaDe.Enabled = True
            txt_FechaA.Enabled = True

            txtFecGeneraDe.Enabled = True
            txtFecGeneraA.Enabled = True

            optAjuste.Enabled = True
            gvd_Poliza.Enabled = True

            'txtClaveRamCont.Enabled = True
            'txtSearchRamCont.Enabled = True

            'txtClaveRamTec.Enabled = True
            'txtSearchRamTec.Enabled = True

            gvd_RamoContable.Enabled = True
            gvd_Producto.Enabled = True

            ddl_Moneda.Enabled = True

            opt_Cobranzas.Enabled = True
            opt_Estatus.Enabled = True

            hid_TipoCambio.Value = 0

            gvd_Usuario.Enabled = True
            gvd_Estatus.Enabled = True

            gvd_LstOrdenPago.Enabled = True
            If gvd_LstOrdenPago.Rows.Count > 0 Then
                btn_Imprimir.Enabled = True
            Else
                btn_Imprimir.Enabled = False
            End If

            btn_Limpiar.Enabled = True
            btn_BuscaOP.Enabled = True


            btn_Buscar.Enabled = True
            btn_CnlBuscar.Enabled = False
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:Cancelar Busqueda", ex.Message)
            LogError("btn_CnlBuscar_Click" & ex.Message)
        End Try
    End Sub

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
                    Case "Bro"
                        GvdFiltro = gvd_Broker

                    Case "Cia"
                        GvdFiltro = gvd_Compañia

                    Case "Usu"
                        GvdFiltro = gvd_Usuario

                    Case "Est"
                        GvdFiltro = gvd_Estatus

                    Case "Fac"
                        GvdFiltro = gvd_Contrato

                    Case "RamC"
                        GvdFiltro = gvd_RamoContable

                    Case "Pro"
                        GvdFiltro = gvd_Producto

                    Case "RamU"
                        Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")
                        txtClaveRam.Text = Split(Datos(0), "~")(0)
                        txtSearchRam.Text = Split(Datos(0), "~")(1)
                        Exit Sub

                    'Case "RamC"
                    '    Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")
                    '    txtClaveRamCont.Text = Split(Datos(0), "~")(0)
                    '    txtSearchRamCont.Text = Split(Datos(0), "~")(1)
                    '    Exit Sub

                    'Case "RamT"
                    '    Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")
                    '    txtClaveRamTec.Text = Split(Datos(0), "~")(0)
                    '    txtSearchRamTec.Text = Split(Datos(0), "~")(1)
                    '    Exit Sub

                    Case "Cto"

                        If Len(hid_Control.Value) > 0 Then
                            Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")

                            Dim Indices() As String = Split(Split(hid_Control.Value, "|")(1), ",")
                            Dim Tipo As String = Split(hid_Control.Value, "|")(0)

                            Dim gvd_Cuotas As GridView = CType(gvd_CiasXBroker.Rows(CInt(Indices(0))).FindControl("gvd_Cuotas"), GridView)

                            If Tipo = "P" Then
                                Dim hid_codcptoPri As HiddenField = TryCast(gvd_Cuotas.Rows(CInt(Indices(1))).FindControl("hid_codcptoPri"), HiddenField)
                                Dim hid_ConceptoPrima As HiddenField = TryCast(gvd_Cuotas.Rows(CInt(Indices(1))).FindControl("hid_ConceptoPrima"), HiddenField)
                                hid_codcptoPri.Value = Split(Datos(0), "~")(0)
                                hid_ConceptoPrima.Value = Split(Datos(0), "~")(1)
                            Else
                                Dim hid_codcptoCom As HiddenField = TryCast(gvd_Cuotas.Rows(CInt(Indices(1))).FindControl("hid_codcptoCom"), HiddenField)
                                Dim hid_ConceptoComision As HiddenField = TryCast(gvd_Cuotas.Rows(CInt(Indices(1))).FindControl("hid_ConceptoComision"), HiddenField)
                                hid_codcptoCom.Value = Split(Datos(0), "~")(0)
                                hid_ConceptoComision.Value = Split(Datos(0), "~")(1)
                            End If

                            'Actualiza el Grid
                            AñadirCuota(gvd_Cuotas, 0, 0, False)

                        End If


                        hid_Control.Value = ""
                        Exit Sub
                    Case "Cta"
                        Dim Complementos() As String
                        Dim Indice As Integer = hid_Control.Value
                        Dim lbl_Banco As TextBox = CType(gvd_OrdenPago.Rows(Indice - 1).FindControl("lbl_Banco"), TextBox)
                        Dim lbl_Cuenta As TextBox = CType(gvd_OrdenPago.Rows(Indice - 1).FindControl("lbl_Cuenta"), TextBox)

                        Dim hid_cod_banco As HiddenField = CType(gvd_OrdenPago.Rows(Indice - 1).FindControl("hid_cod_banco"), HiddenField)
                        Dim hid_id_cuenta As HiddenField = CType(gvd_OrdenPago.Rows(Indice - 1).FindControl("hid_id_cuenta"), HiddenField)
                        Dim hid_nro_cuenta As HiddenField = CType(gvd_OrdenPago.Rows(Indice - 1).FindControl("hid_nro_cuenta"), HiddenField)

                        Datos = Split(Seleccionados.Substring(0, Seleccionados.Length - 1), "|")

                        hid_id_cuenta.Value = Split(Datos(0), "~")(0)
                        Complementos = Split(Split(Datos(0), "~")(1), "-")

                        hid_cod_banco.Value = Complementos(2)
                        hid_nro_cuenta.Value = Complementos(0)

                        lbl_Banco.Text = "Banco: " & Complementos(1)
                        lbl_Cuenta.Text = "Cuenta: " & Complementos(0)

                        Dim id_pv As Integer = gvd_OrdenPago.DataKeys(Indice - 1)("id_pv")
                        Dim cod_cia_reas_brok As Integer = gvd_OrdenPago.DataKeys(Indice - 1)("cod_cia_reas_brok")
                        Dim cod_moneda As Integer = gvd_OrdenPago.DataKeys(Indice - 1)("cod_moneda")

                        Dim dtTemporal As DataTable = Session("dtCuotas")
                        Dim myRow() As Data.DataRow
                        myRow = dtTemporal.Select("id_pv ='" & id_pv & "' AND cod_cia_reas_brok = '" & cod_cia_reas_brok & "' AND cod_moneda = '" & cod_moneda & "'")
                        For Each item In myRow
                            item("id_Cuenta") = hid_id_cuenta.Value
                            item("cod_banco") = Complementos(2)
                            item("Cuenta") = Complementos(0)
                            item("Banco") = Complementos(1)
                        Next

                        Session("dtCuotas") = dtTemporal

                        Exit Sub
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

    Protected Sub gvd_Reaseguro_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvd_Reaseguro.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim SaldoPrima As Double = sender.DataKeys(e.Row.RowIndex)("SaldoPrima")
                Dim SaldoComision As Double = sender.DataKeys(e.Row.RowIndex)("SaldoComision")
                Dim PrimaRecuperada As Double = sender.DataKeys(e.Row.RowIndex)("PrimaRecuperada")

                Dim lbl_Poliza As Label = TryCast(e.Row.FindControl("lbl_Poliza"), Label)
                Dim lbl_PrimaTotal As Label = TryCast(e.Row.FindControl("lbl_PrimaTotal"), Label)
                Dim lbl_Cobranza As Label = TryCast(e.Row.FindControl("lbl_Cobranza"), Label)
                Dim div_primaCob As HtmlGenericControl = TryCast(e.Row.FindControl("div_primaCob"), HtmlGenericControl)
                Dim div_capa As HtmlGenericControl = TryCast(e.Row.FindControl("div_capa"), HtmlGenericControl)
                Dim div_prima As HtmlGenericControl = TryCast(e.Row.FindControl("div_prima"), HtmlGenericControl)
                Dim div_comision As HtmlGenericControl = TryCast(e.Row.FindControl("div_comision"), HtmlGenericControl)
                Dim btn_Detalle As ImageButton = TryCast(e.Row.FindControl("btn_Detalle"), ImageButton)
                Dim btn_GeneraOP As Button = TryCast(e.Row.FindControl("btn_GeneraOP"), Button)

                Dim lbl_SaldoCob As Label = TryCast(e.Row.FindControl("lbl_SaldoCob"), Label)

                Dim lbl_Espacio1 As Label = TryCast(e.Row.FindControl("lbl_Espacio1"), Label)
                Dim lbl_DetPrima As Label = TryCast(e.Row.FindControl("lbl_DetPrima"), Label)
                Dim lbl_Espacio2 As Label = TryCast(e.Row.FindControl("lbl_Espacio2"), Label)
                Dim lbl_DetComision As Label = TryCast(e.Row.FindControl("lbl_DetComision"), Label)
                Dim lbl_Espacio3 As Label = TryCast(e.Row.FindControl("lbl_Espacio3"), Label)
                Dim lbl_InfoPol As Label = TryCast(e.Row.FindControl("lbl_InfoPol"), Label)
                Dim lbl_TitCapa As Label = TryCast(e.Row.FindControl("lbl_TitCapa"), Label)
                Dim lbl_Capa As Label = TryCast(e.Row.FindControl("lbl_Capa"), Label)
                Dim lbl_TitRamo As Label = TryCast(e.Row.FindControl("lbl_TitRamo"), Label)
                Dim lbl_TitContrato As Label = TryCast(e.Row.FindControl("lbl_TitContrato"), Label)

                Dim lbl_PCTotal As Label = TryCast(e.Row.FindControl("lbl_PCTotal"), Label)
                Dim lbl_PCGenerada As Label = TryCast(e.Row.FindControl("lbl_PCGenerada"), Label)
                Dim lbl_PCRestante As Label = TryCast(e.Row.FindControl("lbl_PCRestante"), Label)
                Dim lbl_PCRecuperada As Label = TryCast(e.Row.FindControl("lbl_PCRecuperada"), Label)


                Dim lbl_MntPCTotal As Label = TryCast(e.Row.FindControl("lbl_MntPCTotal"), Label)
                Dim lbl_MntPCGenerada As Label = TryCast(e.Row.FindControl("lbl_MntPCGenerada"), Label)
                Dim lbl_MntPCRestante As Label = TryCast(e.Row.FindControl("lbl_MntPCRestante"), Label)
                Dim lbl_MntPCRecuperada As Label = TryCast(e.Row.FindControl("lbl_MntPCRecuperada"), Label)


                Dim lbl_CancelaPoliza As Label = TryCast(e.Row.FindControl("lbl_CancelaPoliza"), Label)
                Dim lbl_Total2 As TextBox = TryCast(e.Row.FindControl("lbl_Total2"), TextBox)
                Dim lbl_TitMonto As Label = TryCast(e.Row.FindControl("lbl_TitMonto"), Label)


                Dim img_Check As Image = TryCast(e.Row.FindControl("img_Check"), Image)

                Dim Id_Pol As String = gvd_Reaseguro.DataKeys(e.Row.RowIndex)("Id_Pol").ToString()
                Dim blnPendiente As String = gvd_Reaseguro.DataKeys(e.Row.RowIndex)("blnPendiente").ToString()
                Dim bln_Cambio As String = gvd_Reaseguro.DataKeys(e.Row.RowIndex)("bln_Cambio").ToString()
                Dim cod_estatus_op As Integer = gvd_Reaseguro.DataKeys(e.Row.RowIndex)("cod_estatus_op").ToString()
                Dim sn_NoPago As String = gvd_Reaseguro.DataKeys(e.Row.RowIndex)("sn_NoPago").ToString()

                'Dim chk_DescartaPago As CheckBox = TryCast(e.Row.FindControl("chk_DescartaPago"), CheckBox)


                If Id_Pol = "0" Then
                    div_primaCob.Style("background-color") = "lightblue"
                    div_capa.Style("background-color") = "lightblue"
                    div_prima.Style("background-color") = "lightblue"
                    div_comision.Style("background-color") = "lightblue"
                    lbl_TitCapa.Visible = False
                    lbl_Capa.Visible = False
                    btn_Detalle.Visible = False

                    If sn_NoPago = "1" Then
                        lbl_Espacio2.Text = "MOVIMIENTO INTERNO"
                        lbl_Espacio2.BackColor = Drawing.Color.Red
                        lbl_Espacio2.ForeColor = Drawing.Color.White
                        lbl_Espacio2.Font.Bold = True
                        lbl_Espacio2.Font.Size = 10
                        lbl_Espacio2.Height = 27
                    End If

                    lbl_PCTotal.BackColor = Drawing.Color.LightBlue
                    lbl_PCGenerada.BackColor = Drawing.Color.LightBlue
                    lbl_PCRestante.BackColor = Drawing.Color.LightBlue
                    lbl_PCRecuperada.BackColor = Drawing.Color.LightBlue

                    lbl_MntPCTotal.BackColor = Drawing.Color.LightBlue
                    lbl_MntPCGenerada.BackColor = Drawing.Color.LightBlue
                    lbl_MntPCRecuperada.BackColor = Drawing.Color.LightBlue

                    btn_GeneraOP.Visible = True

                    If Val(lbl_SaldoCob.Text) < 0 Then
                        lbl_SaldoCob.ForeColor = Drawing.Color.Red
                    ElseIf Val(lbl_SaldoCob.Text) > 0 Then
                        lbl_SaldoCob.ForeColor = Drawing.Color.Black
                    Else
                        lbl_SaldoCob.ForeColor = Drawing.Color.Green
                    End If

                    If Val(lbl_PrimaTotal.Text) = 0 Then
                        div_primaCob.Style("background-color") = "#9966FF"
                        div_capa.Style("background-color") = "#9966FF"
                        div_prima.Style("background-color") = "#9966FF"
                        div_comision.Style("background-color") = "#9966FF"
                        btn_GeneraOP.Enabled = False
                        lbl_CancelaPoliza.Text = "MOVIMIENTO CANCELADO"
                        lbl_Total2.Enabled = False
                    End If
                Else
                    'chk_DescartaPago.Visible = False

                    lbl_Espacio1.Visible = False
                    lbl_DetPrima.Visible = False
                    lbl_Espacio2.Visible = False
                    lbl_DetComision.Visible = False
                    lbl_Espacio3.Visible = False
                    lbl_InfoPol.Visible = False

                    lbl_CancelaPoliza.BackColor = Drawing.Color.White
                    lbl_CancelaPoliza.BorderColor = Drawing.Color.White
                    lbl_CancelaPoliza.Text = ""
                    lbl_CancelaPoliza.Enabled = False

                    lbl_Total2.BackColor = Drawing.Color.White
                    lbl_Total2.BorderColor = Drawing.Color.White
                    lbl_Total2.Text = ""
                    lbl_Total2.Enabled = False

                    lbl_TitMonto.BackColor = Drawing.Color.White
                    lbl_TitMonto.BorderColor = Drawing.Color.White
                    lbl_TitMonto.ForeColor = Drawing.Color.White
                    If Math.Abs(SaldoPrima - SaldoComision + PrimaRecuperada) <= 9.9 Then
                        If InStr(cod_estatus_op, "5") > 0 And Not (InStr(cod_estatus_op, "1") > 0 Or InStr(cod_estatus_op, "2") > 0) Then
                            div_capa.Style("background-color") = "LightGreen"
                            div_prima.Style("background-color") = "LightGreen"
                            div_comision.Style("background-color") = "LightGreen"
                            div_capa.Style("color") = "Green"
                            div_prima.Style("color") = "Green"
                            div_comision.Style("color") = "Green"
                            lbl_TitCapa.ForeColor = Drawing.Color.Green
                            lbl_TitRamo.ForeColor = Drawing.Color.Green
                            lbl_TitContrato.ForeColor = Drawing.Color.Green

                            lbl_PCTotal.BackColor = Drawing.Color.LightGreen
                            lbl_PCGenerada.BackColor = Drawing.Color.LightGreen
                            lbl_PCRestante.BackColor = Drawing.Color.LightGreen
                            lbl_PCRecuperada.BackColor = Drawing.Color.LightGreen

                            lbl_PCTotal.ForeColor = Drawing.Color.Green
                            lbl_PCGenerada.ForeColor = Drawing.Color.Green
                            lbl_PCRestante.ForeColor = Drawing.Color.Green
                            lbl_PCRecuperada.ForeColor = Drawing.Color.Green

                            lbl_MntPCTotal.BackColor = Drawing.Color.LightGreen
                            lbl_MntPCGenerada.BackColor = Drawing.Color.LightGreen
                            lbl_MntPCRecuperada.BackColor = Drawing.Color.LightGreen
                        ElseIf Not InStr(cod_estatus_op, "0") > 0
                            div_capa.Style("background-color") = "Yellow"
                            div_prima.Style("background-color") = "Yellow"
                            div_comision.Style("background-color") = "Yellow"

                            lbl_PCTotal.BackColor = Drawing.Color.Yellow
                            lbl_PCGenerada.BackColor = Drawing.Color.Yellow
                            lbl_PCRestante.BackColor = Drawing.Color.Yellow
                            lbl_PCRecuperada.BackColor = Drawing.Color.Yellow

                            lbl_PCTotal.ForeColor = Drawing.Color.Black
                            lbl_PCGenerada.ForeColor = Drawing.Color.Black
                            lbl_PCRestante.ForeColor = Drawing.Color.Black
                            lbl_PCRecuperada.ForeColor = Drawing.Color.Black

                            lbl_MntPCTotal.BackColor = Drawing.Color.Yellow
                            lbl_MntPCGenerada.BackColor = Drawing.Color.Yellow
                            lbl_MntPCRecuperada.BackColor = Drawing.Color.Yellow
                        End If

                    ElseIf (SaldoPrima - SaldoComision + PrimaRecuperada) < -9.9 Then
                        If InStr(cod_estatus_op, "5") > 0 And Not (InStr(cod_estatus_op, "1") > 0 Or InStr(cod_estatus_op, "2") > 0) Then
                            div_capa.Style("background-color") = "LightGray"
                            div_prima.Style("background-color") = "LightGray"
                            div_comision.Style("background-color") = "LightGray"
                            div_capa.Style("color") = "Green"
                            div_prima.Style("color") = "Green"
                            div_comision.Style("color") = "Green"
                            lbl_TitCapa.ForeColor = Drawing.Color.Green
                            lbl_TitRamo.ForeColor = Drawing.Color.Green
                            lbl_TitContrato.ForeColor = Drawing.Color.Green

                            lbl_PCTotal.BackColor = Drawing.Color.LightGray
                            lbl_PCGenerada.BackColor = Drawing.Color.LightGray
                            lbl_PCRestante.BackColor = Drawing.Color.LightGray
                            lbl_PCRecuperada.BackColor = Drawing.Color.LightGray

                            lbl_PCTotal.ForeColor = Drawing.Color.Green
                            lbl_PCGenerada.ForeColor = Drawing.Color.Green
                            lbl_PCRestante.ForeColor = Drawing.Color.Green
                            lbl_PCRecuperada.ForeColor = Drawing.Color.Green

                            lbl_MntPCTotal.BackColor = Drawing.Color.LightGray
                            lbl_MntPCGenerada.BackColor = Drawing.Color.LightGray
                            lbl_MntPCRecuperada.BackColor = Drawing.Color.LightGray
                        ElseIf Not InStr(cod_estatus_op, "0") > 0
                            div_capa.Style("background-color") = "Yellow"
                            div_prima.Style("background-color") = "Yellow"
                            div_comision.Style("background-color") = "Yellow"

                            lbl_PCTotal.BackColor = Drawing.Color.Yellow
                            lbl_PCGenerada.BackColor = Drawing.Color.Yellow
                            lbl_PCRestante.BackColor = Drawing.Color.Yellow
                            lbl_PCRecuperada.BackColor = Drawing.Color.Yellow

                            lbl_PCTotal.ForeColor = Drawing.Color.Black
                            lbl_PCGenerada.ForeColor = Drawing.Color.Black
                            lbl_PCRestante.ForeColor = Drawing.Color.Black
                            lbl_PCRecuperada.ForeColor = Drawing.Color.Black

                            lbl_MntPCTotal.BackColor = Drawing.Color.Yellow
                            lbl_MntPCGenerada.BackColor = Drawing.Color.Yellow
                            lbl_MntPCRecuperada.BackColor = Drawing.Color.Yellow
                        End If
                    Else
                        If blnPendiente = "1" Then
                            div_capa.Style("background-color") = "OrangeRed"
                            div_prima.Style("background-color") = "OrangeRed"
                            div_comision.Style("background-color") = "OrangeRed"
                            div_capa.Style("color") = "White"
                            div_prima.Style("color") = "White"
                            div_comision.Style("color") = "White"
                            lbl_TitCapa.ForeColor = Drawing.Color.White
                            lbl_TitRamo.ForeColor = Drawing.Color.White
                            lbl_TitContrato.ForeColor = Drawing.Color.White

                            lbl_PCTotal.BackColor = Drawing.Color.OrangeRed
                            lbl_PCGenerada.BackColor = Drawing.Color.OrangeRed
                            lbl_PCRestante.BackColor = Drawing.Color.OrangeRed
                            lbl_PCRecuperada.BackColor = Drawing.Color.OrangeRed

                            lbl_PCTotal.ForeColor = Drawing.Color.White
                            lbl_PCGenerada.ForeColor = Drawing.Color.White
                            lbl_PCRestante.ForeColor = Drawing.Color.White
                            lbl_PCRecuperada.ForeColor = Drawing.Color.White

                            lbl_MntPCTotal.BackColor = Drawing.Color.OrangeRed
                            lbl_MntPCGenerada.BackColor = Drawing.Color.OrangeRed
                            lbl_MntPCRecuperada.BackColor = Drawing.Color.OrangeRed
                            'Else
                            '    If Not InStr(cod_estatus_op, "0") > 0 Then
                            '        div_capa.Style("background-color") = "Yellow"
                            '        div_prima.Style("background-color") = "Yellow"
                            '        div_comision.Style("background-color") = "Yellow"

                            '        lbl_PCTotal.BackColor = Drawing.Color.Yellow
                            '        lbl_PCGenerada.BackColor = Drawing.Color.Yellow
                            '        lbl_PCRestante.BackColor = Drawing.Color.Yellow
                            '        lbl_PCRecuperada.BackColor = Drawing.Color.Yellow

                            '        lbl_PCTotal.ForeColor = Drawing.Color.Black
                            '        lbl_PCGenerada.ForeColor = Drawing.Color.Black
                            '        lbl_PCRestante.ForeColor = Drawing.Color.Black
                            '        lbl_PCRecuperada.ForeColor = Drawing.Color.Black

                            '        lbl_MntPCTotal.BackColor = Drawing.Color.Yellow
                            '        lbl_MntPCGenerada.BackColor = Drawing.Color.Yellow
                            '        lbl_MntPCRecuperada.BackColor = Drawing.Color.Yellow
                            '    End If
                        End If
                    End If

                    If lbl_Poliza.Text = "" Then
                        lbl_Poliza.Visible = False
                        lbl_Cobranza.Visible = False
                        div_primaCob.Visible = False
                    End If

                    img_Check.Visible = IIf(bln_Cambio = "1", True, False)
                End If

                If Val(lbl_Total2.Text) <> 0 Then
                    lbl_Total2.Enabled = False
                End If

                If Val(lbl_MntPCRestante.Text) < 0 Then
                    lbl_MntPCRestante.ForeColor = Drawing.Color.Red
                ElseIf Val(lbl_MntPCRestante.Text) > 0 Then
                    lbl_MntPCRestante.ForeColor = Drawing.Color.Black
                Else
                    lbl_MntPCRestante.ForeColor = Drawing.Color.Green
                End If

            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: DATABOUND BROKERS Y COMPAÑIAS", ex.Message)
            LogError("(gvd_Reaseguro_RowDataBound)" & ex.Message)
        End Try
    End Sub

    Private Function ObtieneTipoCambio(ByVal Fecha As String, ByVal cod_moneda As Integer) As Double
        Dim sCnn As String = ""
        Dim sSel As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spS_TipoCambio '" & FechaAIngles(Fecha) & "'," & cod_moneda

        Dim da As SqlDataAdapter

        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        Return dtRes.Rows(0)(0)
    End Function

    Private Function ObtieneBrokers(ByVal Cuotas As DataTable) As String()
        Dim cod_broker As Integer
        Dim cod_cia As Integer
        Dim i As Integer = 0
        Dim arrayBroker() As String = Nothing

        Dim brokers = From q In (From p In Cuotas.AsEnumerable()
                                 Order By p("cod_cia_reas_brok"), p("cod_cia_reas_cia")
                                 Select New With {.cod_cia_reas_brok = p("cod_cia_reas_brok"),
                                                  .Broker = p("Broker"),
                                                  .cod_cia_reas_cia = p("cod_cia_reas_cia"),
                                                  .Compañia = p("Compañia")})
                      Select q.cod_cia_reas_brok, q.Broker, q.cod_cia_reas_cia, q.Compañia



        For Each item In brokers.Distinct
            'evalua si existen negocios directos
            If item.cod_cia_reas_brok = 0 Then
                If cod_cia <> item.cod_cia_reas_cia Then
                    cod_cia = item.cod_cia_reas_cia
                    ReDim Preserve arrayBroker(i)
                    arrayBroker(i) = item.cod_cia_reas_cia & "|°|" & item.Compañia
                    i = i + 1
                End If
            Else
                If cod_broker <> item.cod_cia_reas_brok Then
                    cod_broker = item.cod_cia_reas_brok
                    ReDim Preserve arrayBroker(i)
                    arrayBroker(i) = item.cod_cia_reas_brok & "|°|" & item.Broker
                    i = i + 1
                End If
            End If
        Next

        Return arrayBroker

    End Function

    Private Sub EvaluaOrdenPago(ByVal Cuotas As DataTable)
        Dim blnCuota As Boolean
        Dim strRef As String
        Dim cod_broker As Integer
        Dim Descripcion As String = ""
        Dim i As Integer = 0
        Dim arrayBroker() As String = ObtieneBrokers(Cuotas)
        Dim dtOrdenes As DataTable
        Dim Texto As String = ""
        Dim Banco, TipoDeCuenta, Cuenta, Moneda, txt_swift As String
        Dim id_Cuenta, cod_banco, cod_moneda, cod_tipo_banco, id_persona As Integer
        Dim id_pv As Integer
        Dim cod_cia_reas_brok As Integer
        Dim Polizas As String
        Dim strId_pv As String
        Dim Montos() As String

        'Solo si existen registros para almacenar
        If Cuotas.Rows.Count > 0 Then
            dtOrdenes = New DataTable
            dtOrdenes.Columns.Add("Nro")
            dtOrdenes.Columns.Add("Poliza")
            dtOrdenes.Columns.Add("Broker")
            dtOrdenes.Columns.Add("Parcial")
            dtOrdenes.Columns.Add("Impuesto")
            dtOrdenes.Columns.Add("FechaPago")
            dtOrdenes.Columns.Add("TipoPago")
            dtOrdenes.Columns.Add("Texto")
            dtOrdenes.Columns.Add("Banco")
            dtOrdenes.Columns.Add("TipoDeCuenta")
            dtOrdenes.Columns.Add("Cuenta")
            dtOrdenes.Columns.Add("Moneda")
            dtOrdenes.Columns.Add("id_Cuenta")
            dtOrdenes.Columns.Add("cod_banco")
            dtOrdenes.Columns.Add("cod_moneda")
            dtOrdenes.Columns.Add("id_persona")
            dtOrdenes.Columns.Add("txt_swift")
            dtOrdenes.Columns.Add("cod_tipo_banco")
            dtOrdenes.Columns.Add("id_pv")
            dtOrdenes.Columns.Add("cod_cia_reas_brok")

            For i = 0 To UBound(arrayBroker)
                blnCuota = False
                strRef = ""
                cod_broker = Split(arrayBroker(i), "|°|")(0)
                Descripcion = Split(arrayBroker(i), "|°|")(1)
                Banco = ""
                TipoDeCuenta = ""
                Cuenta = ""
                Moneda = ""
                txt_swift = ""

                id_Cuenta = 0
                cod_banco = 0
                cod_moneda = 0
                cod_tipo_banco = 0
                id_persona = 0

                id_pv = 0
                cod_cia_reas_brok = 0

                Polizas = ""
                strId_pv = ""

                Texto = "PAGO DE GARANTIAS "

                For Each Cuota As DataRow In Cuotas.Rows
                    'Valida que se trate del broker o compañia en turno
                    If cod_broker = Cuota("cod_cia_reas_brok") Or (cod_broker = Cuota("cod_cia_reas_cia") And Cuota("cod_cia_reas_brok") = 0) Then

                        If id_pv <> Cuota("id_pv") Then
                            id_pv = Cuota("id_pv")

                            strId_pv = strId_pv & "," & id_pv

                            Polizas = Polizas & Cuota("cod_suc") & "-" &
                                                Cuota("cod_ramo") & "-" &
                                                Cuota("nro_pol") & "-" &
                                                Cuota("aaaa_endoso") & "-" &
                                                Cuota("nro_endoso") & vbCrLf

                            Texto = Texto & vbCrLf & "(" & Cuota("cod_suc") & ") " & Cuota("Sucursal") & "  (" & Cuota("cod_ramo") & ") " & Cuota("Ramo") & "  " &
                                                           Cuota("nro_pol") & " " & Cuota("aaaa_endoso") & " " & Cuota("nro_endoso") & "   " & Cuota("Asegurado") & "   " & vbCrLf &
                                                           "REF "
                            strRef = ""
                        End If

                        Texto = Texto & IIf(strRef <> Cuota("REF"), Cuota("REF") & " , ", "")
                        strRef = Cuota("REF")

                        If blnCuota = False Then

                            blnCuota = True
                            'Broker
                            cod_cia_reas_brok = Cuota("cod_cia_reas_brok")

                            'Datos Bancarios
                            Banco = Cuota("Banco")
                            TipoDeCuenta = Cuota("TipoDeCuenta")
                            Cuenta = Cuota("Cuenta")
                            Moneda = Cuota("Moneda")
                            txt_swift = Cuota("txt_swift")
                            id_Cuenta = Cuota("id_Cuenta")
                            cod_banco = Cuota("cod_banco")
                            cod_moneda = Cuota("cod_moneda")
                            id_persona = Cuota("id_persona")
                            cod_tipo_banco = Cuota("cod_tipo_banco")
                        End If
                    End If
                Next

                Montos = Split(ConsultaParcial(Mid(strId_pv, 2, Len(strId_pv) - 1)), "|")

                'If Len(txt_Ajustes.Text) > 0 Then
                '    Texto = Texto & vbCrLf & "Ajuste: " & txt_Ajustes.Text
                'End If


                dtOrdenes.Rows.Add(i + 1, Polizas, Descripcion, CDbl(Montos(0)), CDbl(Montos(1)),
                                   DateAdd(DateInterval.Day, 1, Now), "T", Texto,
                                   Banco, TipoDeCuenta, Cuenta, Moneda, id_Cuenta, cod_banco, cod_moneda,
                                   id_persona, txt_swift, cod_tipo_banco, Mid(strId_pv, 2, Len(strId_pv) - 1),
                                   cod_cia_reas_brok)
            Next

            gvd_OrdenPago.DataSource = dtOrdenes
            gvd_OrdenPago.DataBind()
        End If


    End Sub

    Private Sub GeneraOrdenPago(ByVal id_pv As Double, ByVal Cuotas As DataTable)
        Dim indMonto As Integer = 0
        Dim indMontoReas As Integer = 0
        Dim indMontoISR As Integer = 0
        Dim strMontos(0) As String
        Dim strMontosReas(0) As String
        Dim strMontosISR(0) As String
        Dim pagina As Integer = 0

        Dim no_correlativo As Integer = 0
        Dim Resultado As String = ""
        Dim SumaPrima As Double
        Dim SumaComision As Double
        Dim SumaISR As Double
        Dim cod_moneda As Integer
        Dim impCambio As Double
        Dim cod_broker As Integer
        Dim Descripcion As String = ""
        Dim cod_suc As Integer
        Dim id_persona As Integer
        Dim ret_ISR As Double = 0
        Dim blnMontos As Boolean = False
        Dim strOrdenPago As String = ""
        Dim i As Integer = 0
        Dim arrayBroker() As String = ObtieneBrokers(Cuotas)
        Dim ArrayAdicional() As String = ObtieneInfoAdicional()
        Dim Cuenta, txt_swift, nro_nit As String
        Dim id_Cuenta, cod_banco As Integer
        Dim FechaPago As String = ""
        Dim sn_transferencia As Integer = 0
        Dim mop_texto As String
        Dim id_imputacion As Double = 0
        Dim nro_op As Double = 0
        Dim Datos As String
        Dim strDatos As String = ""
        Dim PrimaCedida, Comision, Impuesto As Double
        Dim cod_deb_cred As String = ""

        'Solo si existen registros para almacenar
        If Cuotas.Rows.Count > 0 Then

            'Total de Brokers a pagar
            For i = 0 To UBound(arrayBroker)

                blnMontos = False
                no_correlativo = 0
                SumaPrima = 0
                SumaComision = 0
                SumaISR = 0
                Cuenta = ""
                txt_swift = ""
                nro_nit = ""
                id_Cuenta = 0
                cod_banco = 0
                mop_texto = ""

                cod_broker = Split(arrayBroker(i), "|°|")(0)
                Descripcion = Split(arrayBroker(i), "|°|")(1)

                indMonto = 0
                ReDim Preserve strMontos(indMonto)
                strMontos(indMonto) = ""
                indMontoReas = 0
                ReDim Preserve strMontosReas(indMontoReas)
                strMontosReas(indMontoReas) = ""

                For Each Cuota As DataRow In Cuotas.Rows
                    'Valida que se trate del broker o compañia en turno
                    If cod_broker = Cuota("cod_cia_reas_brok") Or (cod_broker = Cuota("cod_cia_reas_cia") And Cuota("cod_cia_reas_brok") = 0) Then
                        cod_moneda = Cuota("cod_moneda")
                        impCambio = Cuota("imp_cambio")

                        cod_suc = Cuota("cod_suc")

                        id_persona = Cuota("id_persona")

                        Cuenta = Cuota("Cuenta")
                        txt_swift = Cuota("txt_swift")
                        id_Cuenta = Cuota("id_Cuenta")
                        cod_banco = Cuota("cod_banco")

                        nro_nit = Cuota("nro_nit")

                        ret_ISR = Cuota("ret_ISR")

                        If Cuota("PrimaCedida") <> 0 Then

                            blnMontos = True

                            If Len(strMontos(indMonto)) > 7500 Then
                                indMonto = indMonto + 1
                                ReDim Preserve strMontos(indMonto)
                                strMontos(indMonto) = ""
                            End If


                            If Len(strMontosReas(indMontoReas)) > 7500 Then
                                indMontoReas = indMontoReas + 1
                                ReDim Preserve strMontosReas(indMontoReas)
                                strMontosReas(indMontoReas) = ""
                            End If

                            PrimaCedida = IIf(Cuota("PrimaCedida") >= 0, Cuota("PrimaCedida"), -1 * Cuota("PrimaCedida"))
                            cod_deb_cred = IIf(Cuota("PrimaCedida") >= 0, "''D''", "''C''")

                            strMontos(indMonto) = strMontos(indMonto) & "(@strKey,8," & no_correlativo & ",NULL,''" & Cuota("Cta_CblePri") & "''," & cod_deb_cred & "," &
                                                                  Cuota("cod_moneda") & "," & PrimaCedida & "," & PrimaCedida * Cuota("imp_cambio") & "," &
                                                                  IIf(Cuota("cod_moneda") = 1, Cuota("imp_cambio"), 1) & ",''" & "PRIMA''),"

                            strMontosReas(indMontoReas) = strMontosReas(indMontoReas) & "(@strKey,8," & no_correlativo & "," & Cuota("cod_cptoPri") & "," & Cuota("cod_cia_reas_brok") & "," & Cuota("cod_cia_reas_cia") & "," &
                                                        "11,1,''" & Cuota("id_contrato") & "''," & Cuota("nro_tramo") & "," & -1 * Cuota("PrimaCedida") & "," &
                                                        Cuota("cod_suc") & "," & Cuota("cod_ramo") & "," & Cuota("nro_pol") & "," & Cuota("aaaa_endoso") & "," & Cuota("nro_endoso") & "," &
                                                        Cuota("cod_suc") & ",1,NULL,NULL," & CStr(Now.ToString("yyyyMM")) & "," & Cuota("cod_ramo_contable") & "," & IIf(Cuota("pje_pri") > 100, 100, Cuota("pje_pri")) & "," & Cuota("nro_cuota") & ",''" & FechaAIngles(Cuota("fecha")) & "''," &
                                                        "0,0,0,0," & Cuota("nro_layer") & "),"


                            no_correlativo = no_correlativo + 1

                            SumaPrima = SumaPrima + Cuota("PrimaCedida")


                            'Si existe impuesto por pagar--------------------------------------------------------------------------------------
                            If Cuota("ret_ISR") <> 0 Then
                                If Len(strMontos(indMonto)) > 7500 Then
                                    indMonto = indMonto + 1
                                    ReDim Preserve strMontos(indMonto)
                                    strMontos(indMonto) = ""
                                End If


                                If Len(strMontosISR(indMontoISR)) > 7500 Then
                                    indMontoISR = indMontoISR + 1
                                    ReDim Preserve strMontosISR(indMontoISR)
                                    strMontosISR(indMontoISR) = ""
                                End If

                                Impuesto = IIf(Cuota("MontoISR") >= 0, Cuota("MontoISR"), -1 * Cuota("MontoISR"))
                                cod_deb_cred = IIf(Cuota("MontoISR") >= 0, "''C''", "''D''")

                                strMontos(indMonto) = strMontos(indMonto) & "(@strKey,4," & no_correlativo & ",NULL,''" & Cuota("cod_cta_cble") & "''," & cod_deb_cred & "," &
                                                                  Cuota("cod_moneda") & "," & Impuesto & "," & Impuesto * Cuota("imp_cambio") & "," &
                                                                  IIf(Cuota("cod_moneda") = 1, Cuota("imp_cambio"), 1) & ",''ISR " & Cuota("Compañia") & "''),"

                                strMontosISR(indMontoISR) = strMontosISR(indMontoISR) & "(@strKey,4," & no_correlativo & "," & Cuota("cod_suc") & ",303,null,708," &
                                                                                          "null,null,null,null,708,null,null,null,null,null,null,null),"

                                no_correlativo = no_correlativo + 1

                                SumaISR = SumaISR + Cuota("MontoISR")
                            End If
                            '----------------------------------------------------------------------------------------------------------------------

                        End If

                        If Cuota("Comision") <> 0 Then

                            blnMontos = True

                            If Len(strMontos(indMonto)) > 7500 Then
                                indMonto = indMonto + 1
                                ReDim Preserve strMontos(indMonto)
                                strMontos(indMonto) = ""
                            End If

                            If Len(strMontosReas(indMontoReas)) > 7500 Then
                                indMontoReas = indMontoReas + 1
                                ReDim Preserve strMontosReas(indMontoReas)
                                strMontosReas(indMontoReas) = ""
                            End If

                            Comision = IIf(Cuota("Comision") >= 0, Cuota("Comision"), -1 * Cuota("Comision"))
                            cod_deb_cred = IIf(Cuota("Comision") >= 0, "''C''", "''D''")

                            strMontos(indMonto) = strMontos(indMonto) & "(@strKey,8," & no_correlativo & ",NULL,''" & Cuota("Cta_CbleCom") & "''," & cod_deb_cred & "," &
                                                                  Cuota("cod_moneda") & "," & Comision & "," & Comision * Cuota("imp_cambio") & "," &
                                                                  IIf(Cuota("cod_moneda") = 1, Cuota("imp_cambio"), 1) & ",''" & "COMISION''),"

                            strMontosReas(indMontoReas) = strMontosReas(indMontoReas) & "(@strKey,8," & no_correlativo & "," & Cuota("cod_cptoCom") & "," & Cuota("cod_cia_reas_brok") & "," & Cuota("cod_cia_reas_cia") & "," &
                                                            "11,1,''" & Cuota("id_contrato") & "''," & Cuota("nro_tramo") & "," & Cuota("Comision") & "," &
                                                            Cuota("cod_suc") & "," & Cuota("cod_ramo") & "," & Cuota("nro_pol") & "," & Cuota("aaaa_endoso") & "," & Cuota("nro_endoso") & "," &
                                                            Cuota("cod_suc") & ",1,NULL,NULL," & CStr(Now.ToString("yyyyMM")) & "," & Cuota("cod_ramo_contable") & "," & IIf(Cuota("pje_com") > 100, 100, Cuota("pje_com")) & "," & Cuota("nro_cuota") & ",''" & FechaAIngles(Cuota("fecha")) & "''," &
                                                            "0,0,0,0," & Cuota("nro_layer") & "),"
                            no_correlativo = no_correlativo + 1

                            SumaComision = SumaComision + Cuota("Comision")
                        End If
                    End If
                Next



                'INSERCION DE LA ORDEN DE PAGO------------------------------------------------------------------------------------------------
                If blnMontos = True Then
                    If UBound(ArrayAdicional) >= 0 Then
                        FechaPago = CDate(Split(ArrayAdicional(i), "|")(0)).ToString("yyyyMMdd")
                        sn_transferencia = Split(ArrayAdicional(i), "|")(1)
                        mop_texto = Split(ArrayAdicional(i), "|")(2)
                    Else
                        FechaPago = Now.ToString("yyyyMMdd")
                        sn_transferencia = -1
                        mop_texto = ""
                    End If

                    Datos = InsertaOrdenPago(id_imputacion, SumaPrima - SumaComision - SumaISR, cod_moneda, impCambio,
                                             cod_broker, Descripcion, cod_suc, id_persona, nro_nit,
                                             Cuenta, txt_swift, id_Cuenta, cod_banco, FechaPago, sn_transferencia,
                                             strMontos, strMontosReas, strMontosISR)

                    'Si la inserción fue satisfactoria
                    If Len(Datos) > 0 Then
                        id_imputacion = Split(Datos, "|")(0)
                        nro_op = Split(Datos, "|")(1)

                        'Valida si se repite el Id_imputacion------------------------------------------------------------------------
                        If EvaluaImputacion(id_imputacion) > 1 Then

                            Dim ConsultaBD As ConsultaBD
                            ConsultaBD = New ConsultaBD
                            ConsultaBD.InsertaBitacora(Master.cod_usuario, Master.HostName, "Correción de Imputación (Orden de Pago)", nro_op & " --> " & id_imputacion)

                            id_imputacion = CorrigeImputacion(nro_op, 1)

                            InsertaMovimientos(id_imputacion, strMontos, strMontosReas, strMontosISR, nro_op)

                            InsertaMovimientosResp(nro_op, id_imputacion, strMontos, strMontosReas, strMontosISR)
                        End If

                        'Actuliza movimientos Contables
                        Actualiza_Mov_Contable(id_imputacion, nro_op)

                        'Guarda Texto de Observaciones
                        Resultado = GuardaTexto(nro_op, mop_texto)
                        If Resultado <> "1" Then
                            Mensaje("ORDEN DE PAGO-: GUARDA TEXTO", Resultado)
                        End If

                        strOrdenPago = strOrdenPago & nro_op & ","
                    End If
                End If
            Next

            'BITACORA-CORREO-REPORTE----------------------------------------------------------------------------------------------------------
            If Len(strOrdenPago) > 0 Then
                strOrdenPago = Mid(strOrdenPago, 1, Len(strOrdenPago) - 1)
                Mensaje("ORDEN DE PAGO-: GENERACIÓN", "Se generó la Orden de pago: " & strOrdenPago)


                If ddl_TipoGenera.SelectedValue = "EN" Then
                    EliminaCuotas(id_pv)
                Else
                    For Each row As GridViewRow In gvd_Endosos.Rows
                        Dim chk_SelPol = DirectCast(row.FindControl("chk_SelPol"), CheckBox)
                        If chk_SelPol.Checked = True Then
                            strDatos = strDatos & "," & DirectCast(row.FindControl("hid_idpv"), HiddenField).Value
                        End If
                    Next

                    If Len(strDatos) > 0 Then
                        strDatos = "'" & Mid(strDatos, 2, Len(strDatos) - 1) & "'"
                    End If

                    EliminaCuotas(strDatos)
                End If


                Dim ConsultaBD As ConsultaBD
                ConsultaBD = New ConsultaBD
                ConsultaBD.InsertaBitacora(Master.cod_usuario, Master.HostName, "Generación", "Orden de Pago: " & strOrdenPago)


                Mail.enviaMail("GMXQROSVRVMBX2.GMX.COM.MX",
                               "25",
                               "oscar.sandoval@gmx.com.mx",
                               "2002351340Man",
                               "oscar.sandoval@gmx.com.mx",
                               "oscar.sandoval@gmx.com.mx",
                               Nothing,
                               Nothing,
                               "Registro de Órden de Pago",
                               "Cod Usuario: " & Master.cod_usuario & vbCrLf & "Usuario:" & Master.Usuario & vbCrLf & "Órden de Pago No: " & strOrdenPago,
                               Nothing)

                Dim server As String = "http://siigmxapp02/ReportServer_SIIGMX02?%2fReportesGMX%2fOrdenPago&rs%3AFormat=PDF&rc:Parameters=false&nro_op=@nro_op"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "ImprimirOrden", "ImprimirOrden('" & server & "','" & strOrdenPago & "');", True)
            Else
                Mensaje("ORDEN DE PAGO-: GENERACIÓN", "No se generó la Orden de Pago, no se encontraron exhibiciones")
            End If
        Else
            Mensaje("ORDEN DE PAGO-: GENERACIÓN", "No se generó la Orden de Pago, error al consultar las exhibiciones temporales")
        End If
    End Sub

    Private Function Actualiza_Mov_Contable(ByVal id_imputacion As Double, ByVal nro_op As Integer) As Boolean
        Dim sCnn As String = ""
        Dim sSel As String
        Dim da As SqlDataAdapter
        Dim dtmov As DataTable

        'Movimientos Autorización

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
        sSel = "EXEC spu_mov_autoriz " & id_imputacion & ", null, null, null, null, null," & nro_op
        dtmov = New DataTable
        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dtmov)

        'Movimientos Contables
        sSel = "EXEC spi_grabar_op_cble " & id_imputacion & "," & nro_op
        dtmov = New DataTable
        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dtmov)

        Return True
    End Function

    Private Function InsertaOrdenPago(ByVal id_imputacion As Double, ByVal MontoTotal As Double, ByVal Moneda As Integer,
                                      ByVal impCambio As Double, ByVal cod_broker As Integer, ByVal Broker As String, ByVal cod_suc As Integer,
                                      ByVal id_persona As Integer, ByVal nro_nit As String, ByVal Cuenta As String, ByVal Swift As String, ByVal id_Cuenta As Integer,
                                      ByVal cod_banco As Integer, ByVal FechaPago As String, ByVal sn_transferencia As Integer,
                                      ByVal MontosGen() As String, ByVal MontosReas() As String, ByVal MontosISR() As String) As String
        Dim sCnn As String = ""
        Dim sSel As String
        Dim nro_op As Integer
        Dim Comando As SqlClient.SqlCommand

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
        Dim conn As SqlConnection = New SqlConnection(sCnn)

        '--------------------------------------------------------------------ORDEN PAGO---------------------------------------------------
        conn.Open()
        Dim trOP As SqlTransaction
        trOP = conn.BeginTransaction()

        Try
            'Comando = New SqlClient.SqlCommand("spS_CatalogosOP 'Imp'", conn)
            'Comando.Transaction = trOP
            'id_imputacion = Convert.ToInt32(Comando.ExecuteScalar())

            ''ANTES DE GUARDAR
            ''Valida id_imputacion repetida antes de guardar la OP--------------------------------------------------------
            'If EvaluaImputacion(id_imputacion) > 0 Then
            '    Comando = New SqlClient.SqlCommand("spS_CatalogosOP 'Imp'", conn)
            '    Comando.Transaction = trOP
            '    id_imputacion = Convert.ToInt32(Comando.ExecuteScalar())
            'End If
            ''------------------------------------------------------------------------------------------------------------
            nro_op = 0
            id_imputacion = 0

            ' 8 es modulo reaseguro
            ' 13 es Reaseguradora
            sSel = " Declare @nro_op int  " &
                   " EXEC spI_OrdenPago -1, 1, NULL, " & Master.cod_suc & ", " & Master.cod_sector & ", 13, NULL, NULL, NULL, NULL, " & cod_broker & ",'" & cod_broker & " - " & Broker & " " & id_persona & " " & nro_nit & "','" & Broker & "'," &
                                         Moneda & "," & IIf(Moneda = 1, impCambio, 0) & "," & MontoTotal & "," & "'" & FechaPago & "',NULL,NULL,NULL,NULL,NULL," & id_imputacion & ",NULL,'" & Master.cod_usuario & "',NULL,''," &
                                         "@nro_op_out = @nro_op output,@cod_origen_pago=8, @id_persona = " & id_persona & ", @id_cuenta_bancaria = " & id_Cuenta & "," &
                                         "@sn_transferencia = " & sn_transferencia & ", @cod_suc_pago=" & Master.cod_suc & ", @nro_cuenta_transferencia='" & Cuenta & "',@cod_banco_transferencia=" & cod_banco

            Comando = New SqlClient.SqlCommand(sSel, conn)
            Comando.Transaction = trOP
            nro_op = Convert.ToInt32(Comando.ExecuteScalar())

            trOP.Commit()
            conn.Close()
        Catch ex As Exception
            trOP.Rollback()
            conn.Close()
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(Inserta Orden Pago)" & ex.Message)
            Return ""
        End Try

        '--------------------------------------------------------------------MOVIMIENTOS-----------------------------------------------------
        If nro_op > 0 Then
            id_imputacion = ConsultaImputacion(nro_op)

            'Valida si se repite el Id_imputacion------------------------------------------------------------------------
            If EvaluaImputacion(id_imputacion) > 1 Then
                Dim id_imputacionAnt As Integer = id_imputacion

                id_imputacion = CorrigeImputacion(nro_op, 1)

                Dim ConsultaBD As ConsultaBD
                ConsultaBD = New ConsultaBD
                ConsultaBD.InsertaBitacora(Master.cod_usuario, Master.HostName, "Correción de Imputación (Orden de Pago)", id_imputacionAnt & " --> " & id_imputacion)
            End If

            EliminaImputacion(id_imputacion)

            If InsertaMovimientos(id_imputacion, MontosGen, MontosReas, MontosISR, nro_op) = False Then
                Return ""
            Else
                InsertaMovimientosResp(nro_op, id_imputacion, MontosGen, MontosReas, MontosISR)
            End If

            Return id_imputacion & "|" & nro_op
        Else
            Return ""
        End If

    End Function

    Private Function ConsultaImputacion(ByVal nro_op As Integer) As Double
        Dim sCnn As String = ""
        Dim sSel As String
        Dim da As SqlDataAdapter
        Dim dtRes As DataTable

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "SELECT id_imputacion FROM mop WHERE nro_op = " & nro_op

        dtRes = New DataTable
        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dtRes)

        Return dtRes(0)("id_imputacion")
    End Function

    Private Function DeshacerOrdenPago(ByVal nro_op As Integer) As Boolean
        Dim sCnn As String = ""
        Dim sSel As String
        Dim da As SqlDataAdapter
        Dim dtRes As DataTable

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "DELETE FROM mop WHERE nro_op = " & nro_op

        dtRes = New DataTable
        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dtRes)

        Return True
    End Function

    Private Function EvaluaImputacion(ByVal id_imputacion As Integer) As Integer
        Dim sCnn As String = ""
        Dim sSel As String
        Dim da As SqlDataAdapter
        Dim dtRes As DataTable

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spS_EvaluaImputacion " & id_imputacion

        dtRes = New DataTable
        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dtRes)

        Return CInt(dtRes.Rows(0)("Repetidos"))
    End Function

    Private Function CorrigeImputacion(ByVal nro_op As Integer, ByVal sn_BorraMov As Integer) As Integer
        Dim sCnn As String = ""
        Dim sSel As String
        Dim da As SqlDataAdapter
        Dim dtRes As DataTable

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spU_ConsecImputacion " & nro_op & "," & sn_BorraMov
        dtRes = New DataTable
        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dtRes)

        Return CInt(dtRes.Rows(0)("id_imputacion"))
    End Function

    Private Function RecuperaMovimientos(ByVal nro_op As Integer, ByVal id_imputacion As Integer) As Integer
        Dim sCnn As String = ""
        Dim sSel As String
        Dim da As SqlDataAdapter
        Dim dtRes As DataTable

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spU_RecuperaMovOP " & nro_op & "," & id_imputacion
        dtRes = New DataTable
        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dtRes)

        Return CInt(dtRes.Rows(0)(0))
    End Function

    Private Function InsertaMovimientos(ByVal id_imputacion As Integer, ByVal MontosGen() As String, ByVal MontosReas() As String, ByVal MontosISR() As String, Optional ByVal nro_op As Integer = 0) As Boolean
        Dim sCnn As String = ""
        Dim Resultado As String
        Dim Comando As SqlClient.SqlCommand
        Dim DatosMontos As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
        Dim conn As SqlConnection = New SqlConnection(sCnn)
        conn.Open()

        Try
            'MOVIMIENTOS GENERALES----------------------------------------------------------------------------------------------------------
            For pagina = 0 To UBound(MontosGen)
                If Len(MontosGen(pagina)) > 0 Then
                    DatosMontos = Mid(MontosGen(pagina), 1, Len(MontosGen(pagina)) - 1)
                    Comando = New SqlClient.SqlCommand("spI_OfGread 'tmp_imputacion_general','" & id_imputacion & "','" & DatosMontos & "'", conn)
                    Resultado = Convert.ToInt32(Comando.ExecuteScalar())
                End If
            Next

            'MOVIMIENTOS REASEGURO----------------------------------------------------------------------------------------------------------
            For pagina = 0 To UBound(MontosReas)
                If Len(MontosReas(pagina)) > 0 Then
                    DatosMontos = Mid(MontosReas(pagina), 1, Len(MontosReas(pagina)) - 1)
                    Comando = New SqlClient.SqlCommand("spI_OfGread 'tmp_imputacion_reas','" & id_imputacion & "','" & DatosMontos & "'", conn)
                    Resultado = Convert.ToInt32(Comando.ExecuteScalar())
                End If
            Next

            'MOVIMIENTOS ISR----------------------------------------------------------------------------------------------------------------
            For pagina = 0 To UBound(MontosISR)
                If Len(MontosISR(pagina)) > 0 Then
                    DatosMontos = Mid(MontosISR(pagina), 1, Len(MontosISR(pagina)) - 1)
                    Comando = New SqlClient.SqlCommand("spI_OfGread 'tmp_imputacion_contable','" & id_imputacion & "','" & DatosMontos & "'", conn)
                    Resultado = Convert.ToInt32(Comando.ExecuteScalar())
                End If
            Next
        Catch ex As Exception
            conn.Close()
            DeshacerOrdenPago(nro_op)
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(Inserta Movimientos)" & ex.Message)
            Return False
        End Try

        Return True
    End Function

    Private Function InsertaMovimientosResp(ByVal nro_op As Integer, ByVal id_imputacion As Integer, ByVal MontosGen() As String, ByVal MontosReas() As String, ByVal MontosISR() As String) As Boolean
        Dim sCnn As String = ""
        Dim Resultado As String
        Dim DatosMontos As String
        Dim Comando As SqlClient.SqlCommand

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
        Dim conn As SqlConnection = New SqlConnection(sCnn)
        conn.Open()

        Try
            'MOVIMIENTOS GENERALES----------------------------------------------------------------------------------------------------------
            For pagina = 0 To UBound(MontosGen)
                If Len(MontosGen(pagina)) > 0 Then
                    DatosMontos = Mid(MontosGen(pagina), 1, Len(MontosGen(pagina)) - 1)
                    Comando = New SqlClient.SqlCommand("spI_OfGread 'aMOV_OPGeneralPend','" & nro_op & "," & id_imputacion & "','" & DatosMontos & "'", conn)
                    Resultado = Convert.ToInt32(Comando.ExecuteScalar())
                End If
            Next

            'MOVIMIENTOS REASEGURO----------------------------------------------------------------------------------------------------------
            For pagina = 0 To UBound(MontosReas)
                If Len(MontosReas(pagina)) > 0 Then
                    DatosMontos = Mid(MontosReas(pagina), 1, Len(MontosReas(pagina)) - 1)
                    Comando = New SqlClient.SqlCommand("spI_OfGread 'aMOV_OPReasPend','" & nro_op & "," & id_imputacion & "','" & DatosMontos & "'", conn)
                    Resultado = Convert.ToInt32(Comando.ExecuteScalar())
                End If
            Next

            'MOVIMIENTOS ISR----------------------------------------------------------------------------------------------------------------
            For pagina = 0 To UBound(MontosISR)
                If Len(MontosISR(pagina)) > 0 Then
                    DatosMontos = Mid(MontosISR(pagina), 1, Len(MontosISR(pagina)) - 1)
                    Comando = New SqlClient.SqlCommand("spI_OfGread 'aMOV_OPContablePend','" & nro_op & "," & id_imputacion & "','" & DatosMontos & "'", conn)
                    Resultado = Convert.ToInt32(Comando.ExecuteScalar())
                End If
            Next
        Catch ex As Exception
            conn.Close()
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(Inserta Movimientos Resp)" & ex.Message)
            Return False
        End Try

        Return True
    End Function

    Private Function GuardaDatos(ByVal Tabla As String, ByVal Key As String, ByVal Datos As String) As String
        Dim sCnn As String = ""

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spI_OfGread '" & Tabla & "','" & Key & "','" & Datos & "'"

        Dim da As SqlDataAdapter

        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        Return dtRes.Rows(0)("Resultado")
    End Function

    Private Function ObtieneNoImputacion() As Double
        clCatalogo = New Catalogo
        Return clCatalogo.ObtieneCatalogo("Imp").Rows(0)("id_imputacion")
    End Function


    Private Sub EliminaImputacion(ByVal id_imputacion As Double)
        Dim sCnn As String
        Dim dtImputacion As DataTable

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spD_imputacion " & id_imputacion

        Dim da As SqlDataAdapter

        dtImputacion = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtImputacion)

    End Sub


    Private Sub btn_OkPoliza_Click(sender As Object, e As EventArgs) Handles btn_OkPoliza.Click
        Try

            Dim dtGridView As DataTable
            dtGridView = New DataTable
            dtGridView.Columns.Add("Clave")
            dtGridView.Columns.Add("Descripcion")

            For Each row As GridViewRow In gvd_Poliza.Rows
                Dim Elemento = DirectCast(row.FindControl("chk_SelPol"), HiddenField)
                If Elemento.Value <> "true" Then
                    Dim Fila As DataRow = dtGridView.NewRow()
                    Fila("Clave") = DirectCast(row.FindControl("lbl_ClavePol"), Label).Text
                    Fila("Descripcion") = DirectCast(row.FindControl("lbl_DescripcionPol"), Label).Text
                    dtGridView.Rows.Add(Fila)
                End If
            Next


            For Each row As GridViewRow In gvd_GrupoPolizas.Rows
                Dim NewRow As DataRow = dtGridView.NewRow()
                Dim chk_SelPol As CheckBox = DirectCast(row.FindControl("chk_SelPol"), CheckBox)


                If chk_SelPol.Checked = True Then
                    Dim txt_Sucursal As TextBox = DirectCast(row.FindControl("txt_Sucursal"), TextBox)
                    Dim txt_Ramo As TextBox = DirectCast(row.FindControl("txt_Ramo"), TextBox)
                    Dim txt_Poliza As TextBox = DirectCast(row.FindControl("txt_Poliza"), TextBox)
                    Dim txt_Sufijo As TextBox = DirectCast(row.FindControl("txt_Sufijo"), TextBox)
                    Dim txt_Endoso As TextBox = DirectCast(row.FindControl("txt_Endoso"), TextBox)
                    Dim txt_Ajuste As TextBox = DirectCast(row.FindControl("txt_Ajuste"), TextBox)
                    Dim lbl_GrupoEndoso As TextBox = DirectCast(row.FindControl("lbl_GrupoEndoso"), TextBox)

                    NewRow("Clave") = txt_Sucursal.Text & "-" & txt_Ramo.Text & "-" & txt_Poliza.Text & "-" &
                                      txt_Sufijo.Text & "-" & txt_Endoso.Text & IIf(optAjuste.SelectedValue = 1, " Aj:" & txt_Ajuste.Text, "")
                    NewRow("Descripcion") = lbl_GrupoEndoso.Text
                    dtGridView.Rows.Add(NewRow)

                End If
            Next

            gvd_Poliza.DataSource = dtGridView
            gvd_Poliza.DataBind()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Ok Poliza", "BuscaEndoso();", True)

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: OK Póliza", ex.Message)
            LogError("(btn_OkPoliza_Click)" & ex.Message)
        End Try
    End Sub

    Private Sub gvd_Reaseguro_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvd_Reaseguro.RowCommand
        Try
            Dim Montos() As String

            If e.CommandName.Equals("Detalle") Then

                'hid_TipoCambio.Value = ObtieneTipoCambio(Today.ToString("dd/MM/yyyy"), 1)  'Tipo de Cambio Dolares

                hid_TipoCambio.Value = 20  'Tipo de Cambio Dolares

                Dim Index As Integer = e.CommandSource.NamingContainer.RowIndex

                gvd_CiasXBroker.DataSource = Nothing
                gvd_CiasXBroker.DataBind()

                hid_Paquete.Value = "0"
                hid_Index.Value = Index

                Dim id_pv As Double = gvd_Reaseguro.DataKeys(Index)("id_pv")
                Dim nro_reas As Integer = gvd_Reaseguro.DataKeys(Index)("nro_reas")   'Revisar
                Dim nro_layer As Integer = gvd_Reaseguro.DataKeys(Index)("nro_layer")
                Dim cod_ramo_contable As Integer = gvd_Reaseguro.DataKeys(Index)("cod_ramo_contable")
                Dim id_contrato As String = gvd_Reaseguro.DataKeys(Index)("id_contrato")
                Dim nro_tramo As Integer = gvd_Reaseguro.DataKeys(Index)("nro_tramo") 'Revisar


                txt_Poliza.Text = gvd_Reaseguro.DataKeys(Index)("cod_suc") & "-" &
                                  gvd_Reaseguro.DataKeys(Index)("cod_ramo") & "-" &
                                  gvd_Reaseguro.DataKeys(Index)("nro_pol") & "-" &
                                  gvd_Reaseguro.DataKeys(Index)("aaaa_endoso") & "-" &
                                  gvd_Reaseguro.DataKeys(Index)("nro_endoso")

                Dim Moneda As Integer = gvd_Reaseguro.DataKeys(Index)("cod_moneda") 'Revisar
                hid_Moneda.Value = Moneda

                Dim blnCancelada As Integer = gvd_Reaseguro.DataKeys(Index)("blnCancelada")
                hid_Cancelada.Value = blnCancelada

                txt_Capa.Text = nro_layer
                txt_Ramo.Text = cod_ramo_contable & " .- " & gvd_Reaseguro.DataKeys(Index)("ramo_contable")
                txt_Contrato.Text = id_contrato
                hid_idPv.Value = id_pv
                hid_nroreas.Value = nro_reas
                hid_nrotramo.Value = nro_tramo

                hid_PrimaReaseguro.Value = gvd_Reaseguro.DataKeys(Index)("PrimaCedida") - gvd_Reaseguro.DataKeys(Index)("Comision")
                txt_MontoFraccionado.Text = 0

                gvd_CiasXBroker.DataSource = ConsultaDetalle(id_pv, nro_layer, cod_ramo_contable, id_contrato, nro_tramo, 0, 0, hid_nroreas.Value)
                gvd_CiasXBroker.DataBind()

                'btn_Guardar.Enabled = IIf(blnCancelada = 1, False, True)
                btn_GenerarOP.Enabled = IIf(blnCancelada = 1, False, True)
                btn_VistaPrevia.Enabled = IIf(blnCancelada = 1, False, True)
                txt_MontoFraccionado.Enabled = IIf(blnCancelada = 1, False, True)
                btn_Fraccionar.Enabled = IIf(blnCancelada = 1, False, True)
                'chk_AplicaTodos.Enabled = IIf(blnCancelada = 1, False, True)

                If blnCancelada = 1 Then _
                Mensaje("ORDEN DE PAGO-:", "El Movimiento ha sido Cancelado en Emisión, solo se pueden modificar las Garantias de Pago")

            ElseIf e.CommandName.Equals("GeneraOP") Then
                ddl_TipoGenera.SelectedValue = "EN"

                lbl_MntParcial.Text = 0
                lbl_MntImpuesto.Text = 0
                lbl_MntTotal.Text = 0

                Dim Index As Integer = e.CommandSource.NamingContainer.RowIndex

                hid_Paquete.Value = "1"
                hid_Index.Value = Index

                gvd_OrdenPago.DataSource = Nothing
                gvd_OrdenPago.DataBind()

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Open Modal Ordenes", "OpenPopup('#OrdenesModal');", True)

                Dim dtTemporal As DataTable

                Dim id_pv As Double = gvd_Reaseguro.DataKeys(Index)("id_pv")

                hid_idPv.Value = id_pv
                txt_Poliza.Text = gvd_Reaseguro.DataKeys(Index)("cod_suc") & "-" &
                                  gvd_Reaseguro.DataKeys(Index)("cod_ramo") & "-" &
                                  gvd_Reaseguro.DataKeys(Index)("nro_pol") & "-" &
                                  gvd_Reaseguro.DataKeys(Index)("aaaa_endoso") & "-" &
                                  gvd_Reaseguro.DataKeys(Index)("nro_endoso")

                Session("dtCuotas") = ConsultaCuotas(id_pv)
                dtTemporal = Session("dtCuotas")

                If dtTemporal.Rows.Count > 0 Then
                    EvaluaOrdenPago(Session("dtCuotas"))

                    Montos = Split(ConsultaParcial(hid_idPv.Value), "|")
                    lbl_MntParcial.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)))
                    lbl_MntImpuesto.Text = String.Format("{0:#,#0.00}", CDbl(Montos(1)))
                    lbl_MntTotal.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)) - CDbl(Montos(1)))

                    'lbl_MntParcial.Text = String.Format("{0:#,#0.00}", ConsultaParcial(hid_idPv.Value))
                    btn_ConfirmarOP.Visible = True
                    lbl_Cuotas.Visible = False
                    ddl_Cuotas.Visible = False
                    btn_EvaluaOP.Visible = False
                    lbl_Genera.Visible = True
                    ddl_TipoGenera.Visible = True
                Else
                    lbl_Cuotas.Visible = True
                    ddl_Cuotas.Visible = True
                    btn_EvaluaOP.Visible = True
                    lbl_MntParcial.Text = ""
                    btn_ConfirmarOP.Visible = False
                    lbl_Genera.Visible = False
                    ddl_TipoGenera.Visible = False

                    clCatalogo = New Catalogo
                    ddl_Cuotas.DataValueField = "Clave"
                    ddl_Cuotas.DataTextField = "Clave"
                    ddl_Cuotas.DataSource = clCatalogo.ObtieneNumCuotas(id_pv)
                    ddl_Cuotas.DataBind()
                End If

                hid_Moneda.Value = gvd_Reaseguro.DataKeys(Index)("cod_moneda")
                ddl_MonedaEnd.SelectedValue = gvd_Reaseguro.DataKeys(Index)("cod_moneda")
            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: ROWCOMMAND Póliza", ex.Message)
            LogError("(gvd_Reaseguro_RowCommand)" & ex.Message)
        End Try
    End Sub

    Private Sub gvd_Reaseguro_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvd_Reaseguro.PageIndexChanging
        Try
            gvd_CiasXBroker.DataSource = Nothing
            gvd_CiasXBroker.DataBind()

            gvd_Reaseguro.PageIndex = e.NewPageIndex
            gvd_Reaseguro.DataSource = Session("dtGeneral")
            gvd_Reaseguro.DataBind()

            Dim lbl_TitCapa As Label = TryCast(gvd_Reaseguro.Rows(0).FindControl("lbl_TitCapa"), Label)
            lbl_TitCapa.Focus()

            'For Each row In gvd_Reaseguro.Rows
            '    Dim btn_Detalle As ImageButton = TryCast(row.FindControl("btn_Detalle"), ImageButton)
            '    btn_Detalle.Focus()
            '    Exit For
            'Next

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: PAGEINDEXCHANGING", ex.Message)
            LogError("(gvd_Reaseguro_PageIndexChanging)" & ex.Message)
        End Try

    End Sub

    Protected Sub gvd_CiasXBroker_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvd_CiasXBroker.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim cod_cia_reas_brok As String = gvd_CiasXBroker.DataKeys(e.Row.RowIndex)("cod_cia_reas_brok").ToString()
                Dim cod_cia_reas_cia As String = gvd_CiasXBroker.DataKeys(e.Row.RowIndex)("cod_cia_reas_cia").ToString()


                Dim gvd_Cuotas As GridView = TryCast(e.Row.FindControl("gvd_Cuotas"), GridView)


                Dim query = From q In (From p In dtDetalle.AsEnumerable()
                                       Where p("cod_cia_reas_brok") = cod_cia_reas_brok And
                                             p("cod_cia_reas_cia") = cod_cia_reas_cia
                                       Select New With {.sn_Seleccion = p("sn_Seleccion"),
                                                        .PrimaCedida = p("PrimaCedida"),
                                                        .Comision = p("Comision"),
                                                        .PrimaCedidaCiaBrok = p("PrimaCedidaCiaBrok"),
                                                        .ComisionCiaBrok = p("ComisionCiaBrok"),
                                                        .cod_moneda = p("cod_moneda"),
                                                        .Moneda = p("Moneda"),
                                                        .nro_cuota_reas = p("nro_cuota_reas"),
                                                        .nro_cuota = p("nro_cuota"),
                                                        .nro_subcuota = p("nro_subcuota"),
                                                        .fecha = p("fecha"),
                                                        .pje_pri = p("pje_pri"),
                                                        .pje_com = p("pje_com"),
                                                        .nro_op = p("nro_op"),
                                                        .cod_estatus_op = p("cod_estatus_op"),
                                                        .fec_pago = p("fec_pago"),
                                                        .fec_baja = p("fec_baja"),
                                                        .blnPendiente = p("blnPendiente"),
                                                        .sn_Origen = p("sn_Origen"),
                                                        .cod_cptoPri = p("cod_cptoPri"),
                                                        .ConceptoPrima = p("ConceptoPrima"),
                                                        .cod_cptoCom = p("cod_cptoCom"),
                                                        .ConceptoComision = p("ConceptoComision"),
                                                        .imp_cambio = p("imp_cambio"),
                                                        .Version = p("Version")
                                           })
                            Select q.sn_Seleccion, q.PrimaCedida, q.Comision, q.PrimaCedidaCiaBrok, q.ComisionCiaBrok,
                                   q.cod_moneda, q.Moneda, q.nro_cuota_reas, q.nro_cuota, q.nro_subcuota, q.fecha, q.pje_pri, q.pje_com, q.nro_op, q.cod_estatus_op,
                                   q.fec_pago, q.fec_baja, q.blnPendiente, q.sn_Origen, q.cod_cptoPri, q.ConceptoPrima, q.cod_cptoCom, q.ConceptoComision, q.imp_cambio, q.Version



                Dim dtCuotas As DataTable
                dtCuotas = New DataTable

                dtCuotas = GeneraDatatableCuotas()

                For Each Item In query.Distinct
                    dtCuotas.Rows.Add(Item.sn_Seleccion, Item.PrimaCedidaCiaBrok, Item.ComisionCiaBrok, Item.PrimaCedida, Item.Comision,
                                      Item.cod_moneda, Item.Moneda, Item.nro_cuota_reas, Item.nro_cuota, Item.nro_subcuota, Item.fecha, Item.pje_pri, Item.pje_com,
                                      Item.blnPendiente, Item.nro_op, Item.cod_estatus_op, Item.fec_pago, Item.fec_baja, Item.sn_Origen,
                                      Item.cod_cptoPri, Item.ConceptoPrima, Item.cod_cptoCom, Item.ConceptoComision, Item.imp_cambio, 0, Item.Version)
                Next

                gvd_Cuotas.DataSource = dtCuotas
                gvd_Cuotas.DataBind()
            End If

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: DATABOUND BROKERS Y COMPAÑIAS", ex.Message)
            LogError("(gvd_CiasXBroker_RowDataBound)" & ex.Message)
        End Try

    End Sub

    Protected Sub gvd_Cuotas_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim blnPendiente As String = sender.DataKeys(e.Row.RowIndex)("blnPendiente").ToString()
                Dim sn_Origen As String = sender.DataKeys(e.Row.RowIndex)("sn_Origen")
                Dim cod_estatus_op As Integer = sender.DataKeys(e.Row.RowIndex)("cod_estatus_op")
                Dim imp_cambio As Double = sender.DataKeys(e.Row.RowIndex)("imp_cambio")

                Dim lbl_Cuota As LinkButton = TryCast(e.Row.FindControl("lbl_Cuota"), LinkButton)
                Dim txt_Cuota As TextBox = TryCast(e.Row.FindControl("txt_Cuota"), TextBox)

                Dim lbl_Prima As TextBox = TryCast(e.Row.FindControl("lbl_Prima"), TextBox)
                Dim lbl_Comision As TextBox = TryCast(e.Row.FindControl("lbl_Comision"), TextBox)
                Dim lbl_OrdenPago As LinkButton = TryCast(e.Row.FindControl("lbl_OrdenPago"), LinkButton)

                Dim chk_SelCuota As CheckBox = TryCast(e.Row.FindControl("chk_SelCuota"), CheckBox)
                Dim btn_BorrarCuota As ImageButton = TryCast(e.Row.FindControl("btn_BorrarCuota"), ImageButton)

                Dim lbl_prcPri As TextBox = TryCast(e.Row.FindControl("lbl_prcPri"), TextBox)
                Dim lbl_prcCom As TextBox = TryCast(e.Row.FindControl("lbl_prcCom"), TextBox)
                Dim lbl_Fecha As TextBox = TryCast(e.Row.FindControl("lbl_Fecha"), TextBox)

                If blnPendiente = "1" Then
                    lbl_Cuota.BackColor = Drawing.Color.OrangeRed
                    lbl_OrdenPago.BackColor = Drawing.Color.OrangeRed
                    lbl_Prima.BackColor = Drawing.Color.OrangeRed
                    lbl_Comision.BackColor = Drawing.Color.OrangeRed
                    lbl_Cuota.ForeColor = Drawing.Color.White
                    lbl_OrdenPago.ForeColor = Drawing.Color.White
                    lbl_Prima.ForeColor = Drawing.Color.White
                    lbl_Comision.ForeColor = Drawing.Color.White
                    txt_Cuota.Visible = False
                Else
                    If lbl_OrdenPago.Text <> "0" Then
                        If cod_estatus_op = 1 Or cod_estatus_op = 2 Then
                            lbl_Cuota.BackColor = Drawing.Color.Yellow
                            lbl_Prima.BackColor = Drawing.Color.Yellow
                            lbl_Comision.BackColor = Drawing.Color.Yellow
                            lbl_OrdenPago.BackColor = Drawing.Color.Yellow
                        Else
                            txt_Cuota.Enabled = IIf(cod_estatus_op = 7, False, True)
                            lbl_Cuota.BackColor = IIf(cod_estatus_op = 7, Drawing.Color.LightGreen, Drawing.Color.Green)
                            lbl_Prima.BackColor = IIf(cod_estatus_op = 7, Drawing.Color.LightGreen, Drawing.Color.Green)
                            lbl_Comision.BackColor = IIf(cod_estatus_op = 7, Drawing.Color.LightGreen, Drawing.Color.Green)
                            lbl_OrdenPago.BackColor = IIf(cod_estatus_op = 7, Drawing.Color.LightGreen, Drawing.Color.Green)

                            lbl_Cuota.ForeColor = IIf(cod_estatus_op = 7, Drawing.Color.DarkBlue, Drawing.Color.White)
                            lbl_Prima.ForeColor = IIf(cod_estatus_op = 7, Drawing.Color.DarkBlue, Drawing.Color.White)
                            lbl_Comision.ForeColor = IIf(cod_estatus_op = 7, Drawing.Color.DarkBlue, Drawing.Color.White)
                            lbl_OrdenPago.ForeColor = IIf(cod_estatus_op = 7, Drawing.Color.DarkBlue, Drawing.Color.White)
                        End If

                        lbl_Cuota.Visible = False
                        'lbl_Cuota.Enabled = False

                        chk_SelCuota.Enabled = False
                        btn_BorrarCuota.Visible = False

                        lbl_Prima.Enabled = False
                        lbl_Comision.Enabled = False
                        lbl_prcCom.Enabled = False
                        lbl_prcPri.Enabled = False
                        lbl_Fecha.Enabled = False
                    Else
                        txt_Cuota.Visible = False
                    End If
                End If

                If imp_cambio = 0 Then
                    lbl_Cuota.Enabled = False
                    chk_SelCuota.Enabled = False
                    lbl_Prima.Enabled = False
                    lbl_Comision.Enabled = False
                    lbl_prcCom.Enabled = False
                    lbl_prcPri.Enabled = False
                    lbl_Fecha.Enabled = False
                End If

                If hid_Cancelada.Value = 1 Then
                    chk_SelCuota.Enabled = False
                End If

                If sn_Origen = "1" Then
                    btn_BorrarCuota.Visible = False
                    'lbl_Fecha.Enabled = False
                End If
            End If


        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: DATABOUND CUOTAS", ex.Message)
            LogError("(gvd_Cuotas_RowDataBound)" & ex.Message)
        End Try
    End Sub

    Private Sub gvd_CiasXBroker_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvd_CiasXBroker.RowCommand
        Try
            Dim gvd_Cuotas As GridView
            Dim SaldoPrima As Double
            Dim PrimaCedida As Double

            If e.CommandName.Equals("AñadirCuota") Then


                If chk_AplicaTodos.Checked = True Then
                    For Each Row In gvd_CiasXBroker.Rows
                        gvd_Cuotas = CType(gvd_CiasXBroker.Rows(Row.rowIndex).FindControl("gvd_Cuotas"), GridView)
                        PrimaCedida = CDbl(Replace(CType(gvd_CiasXBroker.Rows(Row.rowIndex).FindControl("lbl_PriReaseguro"), Label).Text, ",", ""))
                        SaldoPrima = PrimaCedida - ObtienePagado(gvd_Cuotas)

                        If SaldoPrima <> 0 Then
                            AñadirCuota(gvd_Cuotas, 0, 0)
                        End If
                    Next
                Else
                    Dim Index As Integer = e.CommandSource.NamingContainer.RowIndex
                    gvd_Cuotas = CType(gvd_CiasXBroker.Rows(Index).FindControl("gvd_Cuotas"), GridView)
                    PrimaCedida = CDbl(Replace(CType(gvd_CiasXBroker.Rows(Index).FindControl("lbl_PriReaseguro"), Label).Text, ",", ""))
                    SaldoPrima = PrimaCedida - ObtienePagado(gvd_Cuotas)
                    If SaldoPrima <> 0 Then
                        AñadirCuota(gvd_Cuotas, 0, 0)
                    Else
                        Mensaje("ORDEN DE PAGO", "No es posible agregar mas exhibiciones ya que no existe saldo por pagar")
                    End If
                End If

            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: ROWCOMMAND BROKER-COMPAÑIA", ex.Message)
            LogError("(gvd_CiasXBroker_RowCommand)" & ex.Message)
        End Try
    End Sub

    Private Function ObtienePagado(ByRef Gridview As GridView) As Double
        Dim nro_op As String
        Dim Prima As Double
        Dim Comision As Double
        Dim Pagado As Double = 0

        For Each Row In Gridview.Rows
            nro_op = TryCast(Row.FindControl("lbl_OrdenPago"), LinkButton).Text
            Prima = CDbl(Replace(TryCast(Row.FindControl("lbl_Prima"), TextBox).Text, ",", ""))
            Comision = CDbl(Replace(TryCast(Row.FindControl("lbl_Comision"), TextBox).Text, ",", ""))
            If nro_op <> "0" Then
                Pagado = Pagado + (Prima - Comision)
            End If
        Next
        Return Pagado
    End Function

    Private Sub AplicarTodo(ByRef GridView As GridView)
        Dim gvd_Cuotas As GridView

        Dim chk_SelCuota As CheckBox = New CheckBox
        Dim lbl_Cuota As LinkButton = New LinkButton
        Dim hid_Cuota As TextBox = New TextBox
        Dim hid_subcuota As HiddenField = New HiddenField
        Dim hid_Comision As TextBox = New TextBox
        Dim hid_Prima As TextBox = New TextBox

        Dim lbl_Fecha As TextBox = New TextBox
        Dim lbl_prcPri As TextBox = New TextBox
        Dim lbl_prcCom As TextBox = New TextBox


        Dim hid_codcptoPri As HiddenField = New HiddenField
        Dim hid_ConceptoPrima As HiddenField = New HiddenField
        Dim hid_codcptoCom As HiddenField = New HiddenField
        Dim hid_ConceptoComision As HiddenField = New HiddenField

        Dim hid_Cambio As TextBox = New TextBox
        Dim lbl_OrdenPago As LinkButton = New LinkButton

        Dim lbl_Prima As TextBox = New TextBox
        Dim lbl_Comision As TextBox = New TextBox

        For Each Row In gvd_CiasXBroker.Rows
            gvd_Cuotas = CType(gvd_CiasXBroker.Rows(Row.rowIndex).FindControl("gvd_Cuotas"), GridView)
            If GridView.ClientID <> gvd_Cuotas.ClientID Then
                'Recorre las cuotas que se aplicaran en todos los casos
                For Each RowOri In GridView.Rows

                    lbl_OrdenPago = TryCast(RowOri.FindControl("lbl_OrdenPago"), LinkButton)
                    chk_SelCuota = CType(RowOri.FindControl("chk_SelCuota"), CheckBox)
                    lbl_Cuota = CType(RowOri.FindControl("lbl_Cuota"), LinkButton)
                    hid_Cuota = CType(RowOri.FindControl("hid_Cuota"), TextBox)
                    hid_subcuota = TryCast(RowOri.FindControl("hid_subcuota"), HiddenField)
                    lbl_prcPri = TryCast(RowOri.FindControl("lbl_prcPri"), TextBox)
                    lbl_prcCom = TryCast(RowOri.FindControl("lbl_prcCom"), TextBox)
                    hid_codcptoPri = TryCast(RowOri.FindControl("hid_codcptoPri"), HiddenField)
                    hid_ConceptoPrima = TryCast(RowOri.FindControl("hid_ConceptoPrima"), HiddenField)
                    hid_codcptoCom = TryCast(RowOri.FindControl("hid_codcptoCom"), HiddenField)
                    hid_ConceptoComision = TryCast(RowOri.FindControl("hid_ConceptoComision"), HiddenField)
                    hid_Cambio = TryCast(RowOri.FindControl("hid_Cambio"), TextBox)
                    lbl_Fecha = CType(RowOri.FindControl("lbl_Fecha"), TextBox)



                Next
            End If
        Next
    End Sub

    Private Function GeneraDatatableCuotas() As DataTable
        Dim dtCuotas As DataTable
        dtCuotas = New DataTable

        dtCuotas.Columns.Add("sn_Seleccion")
        dtCuotas.Columns.Add("PrimaCedidaCiaBrok")
        dtCuotas.Columns.Add("ComisionCiaBrok")
        dtCuotas.Columns.Add("PrimaCedida")
        dtCuotas.Columns.Add("Comision")
        dtCuotas.Columns.Add("cod_moneda")
        dtCuotas.Columns.Add("Moneda")
        dtCuotas.Columns.Add("nro_cuota_reas")
        dtCuotas.Columns.Add("nro_cuota")
        dtCuotas.Columns.Add("nro_subcuota")
        dtCuotas.Columns.Add("fecha")
        dtCuotas.Columns.Add("pje_pri")
        dtCuotas.Columns.Add("pje_com")
        dtCuotas.Columns.Add("blnPendiente")
        dtCuotas.Columns.Add("nro_op")
        dtCuotas.Columns.Add("cod_estatus_op")
        dtCuotas.Columns.Add("fec_pago")
        dtCuotas.Columns.Add("fec_baja")
        dtCuotas.Columns.Add("sn_Origen")
        dtCuotas.Columns.Add("cod_cptoPri")
        dtCuotas.Columns.Add("ConceptoPrima")
        dtCuotas.Columns.Add("cod_cptoCom")
        dtCuotas.Columns.Add("ConceptoComision")
        dtCuotas.Columns.Add("imp_cambio")
        dtCuotas.Columns.Add("sn_cambio")
        dtCuotas.Columns.Add("Version")

        Return dtCuotas
    End Function

    Private Function AñadirCuota(ByRef GridView As GridView, ByVal nro_cuota As Integer, ByVal nro_subcuota As Integer, Optional ByVal blnAdd As Boolean = True) As Boolean
        Dim chk_SelCuota As CheckBox = New CheckBox
        Dim lbl_Cuota As LinkButton = New LinkButton
        Dim hid_Cuota As TextBox = New TextBox
        Dim txt_Cuota As TextBox = New TextBox
        Dim hid_Comision As TextBox = New TextBox
        Dim hid_subcuota As HiddenField = New HiddenField
        Dim hid_Prima As TextBox = New TextBox
        Dim lbl_Prima As TextBox = New TextBox
        Dim lbl_Comision As TextBox = New TextBox
        Dim lbl_Fecha As TextBox = New TextBox
        Dim lbl_prcPri As TextBox = New TextBox
        Dim lbl_prcCom As TextBox = New TextBox
        Dim lbl_OrdenPago As LinkButton = New LinkButton

        Dim hid_codcptoPri As HiddenField = New HiddenField
        Dim hid_ConceptoPrima As HiddenField = New HiddenField
        Dim hid_codcptoCom As HiddenField = New HiddenField
        Dim hid_ConceptoComision As HiddenField = New HiddenField

        Dim SumaPrcPri As Double = 0
        Dim SumaPrcCom As Double = 0
        Dim PrcPri As Double = 0
        Dim PrcCom As Double = 0

        Dim hid_Cambio As TextBox = New TextBox

        Dim cod_moneda As String = ""
        Dim Moneda As String = ""
        Dim blnPendiente As String = ""
        Dim cod_estatus_op As Integer = 0
        Dim fec_pago As String = ""
        Dim fec_baja As String = ""
        Dim sn_Seleccion As Boolean = False
        Dim sn_Origen As String = ""
        Dim imp_cambio As Double = 0
        Dim imp_cambio_actual = hid_TipoCambio.Value
        Dim Version As Integer = 1

        Dim dtCuotas As DataTable
        dtCuotas = New DataTable

        dtCuotas = GeneraDatatableCuotas()

        Dim nro_CuotaAux As Integer = -1
        Dim nro_SubCuotaAux As Integer = -1
        Dim SubCuotaMaxima As Integer = 0

        Dim dtIndices As DataTable
        dtIndices = New DataTable
        dtIndices.Columns.Add("nro_subcuota")

        'Obtiene todos los subindices de la Cuota que se duplica
        For Each Row In GridView.Rows
            hid_Cuota = CType(Row.FindControl("hid_Cuota"), TextBox)
            hid_subcuota = TryCast(Row.FindControl("hid_subcuota"), HiddenField)

            lbl_prcPri = TryCast(Row.FindControl("lbl_prcPri"), TextBox)
            lbl_prcCom = TryCast(Row.FindControl("lbl_prcCom"), TextBox)

            SumaPrcPri = SumaPrcPri + CDbl(lbl_prcPri.Text)
            SumaPrcCom = SumaPrcCom + CDbl(lbl_prcCom.Text)

            If hid_Cuota.Text = nro_cuota Then
                dtIndices.Rows.Add(hid_subcuota.Value)
            End If
        Next

        If Math.Abs(SumaPrcPri) > 100 Then
            PrcPri = 0
        Else
            PrcPri = IIf(SumaPrcPri < 0, -1, 1) * (100 - Math.Abs(SumaPrcPri))
        End If


        If Math.Abs(SumaPrcCom) > 100 Then
            PrcCom = 0
        Else
            PrcCom = IIf(SumaPrcCom < 0, -1, 1) * (100 - Math.Abs(SumaPrcCom))
        End If


        'Obtiene el maximo indice
        Dim Indices = dtIndices.AsEnumerable()
        SubCuotaMaxima = Indices.Max(Function(r) r("nro_subcuota"))


        For Each Row In GridView.Rows
            chk_SelCuota = CType(Row.FindControl("chk_SelCuota"), CheckBox)
            txt_Cuota = CType(Row.FindControl("txt_Cuota"), TextBox)
            lbl_Cuota = CType(Row.FindControl("lbl_Cuota"), LinkButton)
            hid_Cuota = CType(Row.FindControl("hid_Cuota"), TextBox)
            hid_subcuota = TryCast(Row.FindControl("hid_subcuota"), HiddenField)


            'Si se trata de la cuota y la subcuota que se duplicara
            If nro_cuota = nro_CuotaAux And nro_subcuota = nro_SubCuotaAux And blnAdd = True Then

                dtCuotas.Rows.Add(False, hid_Prima.Text, hid_Comision.Text, Replace(lbl_Prima.Text, ",", ""), Replace(lbl_Comision.Text, ",", ""), cod_moneda, Moneda,
                                  nro_cuota, nro_CuotaAux, SubCuotaMaxima + 1, Now, Replace(lbl_prcPri.Text, ",", ""), Replace(lbl_prcCom.Text, ",", ""), False, "0", 0, "", "", "",
                                  hid_codcptoPri.Value, hid_ConceptoPrima.Value, hid_codcptoCom.Value, hid_ConceptoComision.Value,
                                  IIf(cod_moneda = 0, 1, imp_cambio_actual), 0, Version)
            End If

            nro_CuotaAux = hid_Cuota.Text
            nro_SubCuotaAux = hid_subcuota.Value


            hid_Prima = TryCast(Row.FindControl("hid_Prima"), TextBox)
            hid_Comision = TryCast(Row.FindControl("hid_Comision"), TextBox)
            lbl_Prima = TryCast(Row.FindControl("lbl_Prima"), TextBox)
            lbl_Comision = TryCast(Row.FindControl("lbl_Comision"), TextBox)
            lbl_Fecha = CType(Row.FindControl("lbl_Fecha"), TextBox)
            lbl_prcPri = TryCast(Row.FindControl("lbl_prcPri"), TextBox)
            lbl_prcCom = TryCast(Row.FindControl("lbl_prcCom"), TextBox)
            lbl_OrdenPago = TryCast(Row.FindControl("lbl_OrdenPago"), LinkButton)

            hid_codcptoPri = TryCast(Row.FindControl("hid_codcptoPri"), HiddenField)
            hid_ConceptoPrima = TryCast(Row.FindControl("hid_ConceptoPrima"), HiddenField)
            hid_codcptoCom = TryCast(Row.FindControl("hid_codcptoCom"), HiddenField)
            hid_ConceptoComision = TryCast(Row.FindControl("hid_ConceptoComision"), HiddenField)

            hid_Cambio = TryCast(Row.FindControl("hid_Cambio"), TextBox)

            cod_moneda = GridView.DataKeys(Row.RowIndex)("cod_moneda")
            cod_moneda = GridView.DataKeys(Row.RowIndex)("cod_moneda")
            Moneda = GridView.DataKeys(Row.RowIndex)("Moneda")
            blnPendiente = GridView.DataKeys(Row.RowIndex)("blnPendiente")
            cod_estatus_op = GridView.DataKeys(Row.RowIndex)("cod_estatus_op")
            fec_pago = GridView.DataKeys(Row.RowIndex)("fec_pago")
            fec_baja = GridView.DataKeys(Row.RowIndex)("fec_baja")
            sn_Origen = GridView.DataKeys(Row.RowIndex)("sn_Origen")
            imp_cambio = GridView.DataKeys(Row.RowIndex)("imp_cambio")
            Version = GridView.DataKeys(Row.RowIndex)("imp_cambio")

            'Divide la Cuota y la subcuota
            If nro_cuota = hid_Cuota.Text And nro_subcuota = hid_subcuota.Value And blnAdd = True Then
                lbl_Prima.Text = Replace(lbl_Prima.Text, ",", "") / 2
                lbl_Comision.Text = Replace(lbl_Comision.Text, ",", "") / 2
                lbl_prcPri.Text = Replace(lbl_prcPri.Text, ",", "") / 2
                lbl_prcCom.Text = Replace(lbl_prcCom.Text, ",", "") / 2
            End If

            dtCuotas.Rows.Add(chk_SelCuota.Checked, hid_Prima.Text, hid_Comision.Text, Replace(lbl_Prima.Text, ",", ""), Replace(lbl_Comision.Text, ",", ""),
                              cod_moneda, Moneda, lbl_Cuota.Text, hid_Cuota.Text, hid_subcuota.Value, lbl_Fecha.Text, Replace(lbl_prcPri.Text, ",", ""),
                              Replace(lbl_prcCom.Text, ",", ""), blnPendiente, lbl_OrdenPago.Text, cod_estatus_op, fec_pago, fec_baja, sn_Origen,
                              hid_codcptoPri.Value, hid_ConceptoPrima.Value, hid_codcptoCom.Value, hid_ConceptoComision.Value, imp_cambio, hid_Cambio.Text, Version)
        Next

        'Valida si se añade o se duplica
        If blnAdd = True Then
            If nro_cuota = 0 Then
                dtCuotas.Rows.Add(False, hid_Prima.Text, hid_Comision.Text, Replace(hid_Prima.Text, ",", "") * (PrcPri / 100), Replace(hid_Comision.Text, ",", "") * (PrcCom / 100), cod_moneda, Moneda,
                                  lbl_Cuota.Text + 1, hid_Cuota.Text + 1, 0, Now, PrcPri, PrcCom, False, "0", 0, "", "", "",
                                  hid_codcptoPri.Value, hid_ConceptoPrima.Value, hid_codcptoCom.Value, hid_ConceptoComision.Value,
                                  IIf(cod_moneda = 0, 1, imp_cambio_actual), 0, Version)
            Else
                'Valida si la ultima cuota es la duplicada
                If hid_Cuota.Text = nro_cuota And hid_subcuota.Value = nro_subcuota Then
                    dtCuotas.Rows.Add(False, hid_Prima.Text, hid_Comision.Text, Replace(lbl_Prima.Text, ",", ""), Replace(lbl_Comision.Text, ",", ""), cod_moneda, Moneda,
                                      lbl_Cuota.Text, hid_Cuota.Text, SubCuotaMaxima + 1, Now, Replace(lbl_prcPri.Text, ",", ""), Replace(lbl_prcCom.Text, ",", ""), False, "0", 0, "", "", "",
                                      hid_codcptoPri.Value, hid_ConceptoPrima.Value, hid_codcptoCom.Value, hid_ConceptoComision.Value,
                                      IIf(cod_moneda = 0, 1, imp_cambio_actual), 0, Version)
                End If
            End If
        End If

        GridView.DataSource = dtCuotas
        GridView.DataBind()

        Dim lbl_Cuota_Link As LinkButton = TryCast(GridView.Rows(GridView.Rows.Count - 1).FindControl("lbl_Cuota"), LinkButton)
        lbl_Cuota_Link.Focus()

        Return True

    End Function

    Private Function QuitarCuota(ByRef GridView As GridView, ByVal nro_cuota As Integer, ByVal nro_subcuota As Integer) As Boolean
        Dim chk_SelCuota As CheckBox = New CheckBox
        Dim lbl_Cuota As LinkButton = New LinkButton
        Dim hid_Cuota As TextBox = New TextBox
        Dim hid_subcuota As HiddenField = New HiddenField
        Dim hid_Prima As TextBox = New TextBox
        Dim hid_Comision As TextBox = New TextBox
        Dim lbl_Prima As TextBox = New TextBox
        Dim lbl_Comision As TextBox = New TextBox
        Dim lbl_Fecha As TextBox = New TextBox
        Dim lbl_prcPri As TextBox = New TextBox
        Dim lbl_prcCom As TextBox = New TextBox
        Dim lbl_OrdenPago As LinkButton = New LinkButton

        Dim hid_codcptoPri As HiddenField = New HiddenField
        Dim hid_ConceptoPrima As HiddenField = New HiddenField
        Dim hid_codcptoCom As HiddenField = New HiddenField
        Dim hid_ConceptoComision As HiddenField = New HiddenField

        Dim hid_Cambio As TextBox = New TextBox

        Dim cod_moneda As String = ""
        Dim Moneda As String = ""
        Dim blnPendiente As String = ""
        Dim cod_estatus_op As Integer = 0
        Dim fec_pago As String = ""
        Dim fec_baja As String = ""
        Dim sn_Seleccion As Boolean = False
        Dim sn_Origen As String = ""
        Dim imp_cambio As Double = 0
        Dim Version As Integer = 0

        Dim dtCuotas As DataTable
        dtCuotas = New DataTable

        dtCuotas = GeneraDatatableCuotas()

        Dim nro_CuotaAux As Integer = 0

        For Each Row In GridView.Rows
            lbl_Cuota = CType(Row.FindControl("lbl_Cuota"), LinkButton)
            hid_Cuota = TryCast(Row.FindControl("hid_Cuota"), TextBox)
            hid_subcuota = TryCast(Row.FindControl("hid_subcuota"), HiddenField)
            chk_SelCuota = CType(Row.FindControl("chk_SelCuota"), CheckBox)
            hid_Prima = TryCast(Row.FindControl("hid_Prima"), TextBox)
            hid_Comision = TryCast(Row.FindControl("hid_Comision"), TextBox)
            lbl_Prima = TryCast(Row.FindControl("lbl_Prima"), TextBox)
            lbl_Comision = TryCast(Row.FindControl("lbl_Comision"), TextBox)
            lbl_Fecha = CType(Row.FindControl("lbl_Fecha"), TextBox)
            lbl_prcPri = TryCast(Row.FindControl("lbl_prcPri"), TextBox)
            lbl_prcCom = TryCast(Row.FindControl("lbl_prcCom"), TextBox)
            lbl_OrdenPago = TryCast(Row.FindControl("lbl_OrdenPago"), LinkButton)

            hid_codcptoPri = TryCast(Row.FindControl("hid_codcptoPri"), HiddenField)
            hid_ConceptoPrima = TryCast(Row.FindControl("hid_ConceptoPrima"), HiddenField)
            hid_codcptoCom = TryCast(Row.FindControl("hid_codcptoCom"), HiddenField)
            hid_ConceptoComision = TryCast(Row.FindControl("hid_ConceptoComision"), HiddenField)

            hid_Cambio = TryCast(Row.FindControl("hid_Cambio"), TextBox)

            nro_CuotaAux = GridView.DataKeys(Row.RowIndex)("nro_cuota")
            cod_moneda = GridView.DataKeys(Row.RowIndex)("cod_moneda")
            Moneda = GridView.DataKeys(Row.RowIndex)("Moneda")
            blnPendiente = GridView.DataKeys(Row.RowIndex)("blnPendiente")
            cod_estatus_op = GridView.DataKeys(Row.RowIndex)("cod_estatus_op")
            fec_pago = GridView.DataKeys(Row.RowIndex)("fec_pago")
            fec_baja = GridView.DataKeys(Row.RowIndex)("fec_baja")
            sn_Origen = GridView.DataKeys(Row.RowIndex)("sn_Origen")
            imp_cambio = GridView.DataKeys(Row.RowIndex)("imp_cambio")
            Version = GridView.DataKeys(Row.RowIndex)("Version")

            If nro_CuotaAux <> nro_cuota Then
                dtCuotas.Rows.Add(chk_SelCuota.Checked, hid_Prima.Text, hid_Comision.Text, lbl_Prima.Text, lbl_Comision.Text,
                                  cod_moneda, Moneda, lbl_Cuota.Text, hid_Cuota.Text, hid_subcuota.Value, lbl_Fecha.Text, lbl_prcPri.Text,
                                  lbl_prcCom.Text, blnPendiente, lbl_OrdenPago.Text, cod_estatus_op, fec_pago, fec_baja, sn_Origen,
                                  hid_codcptoPri.Value, hid_ConceptoPrima.Value, hid_codcptoCom.Value, hid_ConceptoComision.Value, imp_cambio, hid_Cambio.Text, Version)
            Else
                If hid_subcuota.Value <> nro_subcuota Then
                    dtCuotas.Rows.Add(chk_SelCuota.Checked, hid_Prima.Text, hid_Comision.Text, lbl_Prima.Text, lbl_Comision.Text,
                                  cod_moneda, Moneda, lbl_Cuota.Text, hid_Cuota.Text, hid_subcuota.Value, lbl_Fecha.Text, lbl_prcPri.Text,
                                  lbl_prcCom.Text, blnPendiente, lbl_OrdenPago.Text, cod_estatus_op, fec_pago, fec_baja, sn_Origen,
                                  hid_codcptoPri.Value, hid_ConceptoPrima.Value, hid_codcptoCom.Value, hid_ConceptoComision.Value, imp_cambio, hid_Cambio.Text, Version)
                End If
            End If

        Next

        GridView.DataSource = dtCuotas
        GridView.DataBind()

        Dim lbl_Cuota_Link As LinkButton = TryCast(GridView.Rows(GridView.Rows.Count - 1).FindControl("lbl_Cuota"), LinkButton)
        lbl_Cuota_Link.Focus()

        Return True

    End Function

    Protected Sub gvd_Cuotas_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim gvd_Cuotas As GridView

            If e.CommandName.Equals("Duplicar") Then

                Dim Index As Integer = e.CommandSource.NamingContainer.RowIndex
                Dim nro_Cuota As Integer = sender.DataKeys(Index)("nro_cuota")
                Dim nro_SubCuota As Integer = sender.DataKeys(Index)("nro_subcuota")

                If chk_AplicaTodos.Checked = True Then
                    For Each Row In gvd_CiasXBroker.Rows
                        gvd_Cuotas = CType(gvd_CiasXBroker.Rows(Row.rowIndex).FindControl("gvd_Cuotas"), GridView)
                        AñadirCuota(gvd_Cuotas, nro_Cuota, nro_SubCuota)
                    Next
                Else
                    AñadirCuota(sender, nro_Cuota, nro_SubCuota)
                End If

            ElseIf e.CommandName.Equals("QUitar") Then
                Dim Index As Integer = e.CommandSource.NamingContainer.RowIndex
                Dim nro_Cuota As Integer = sender.DataKeys(Index)("nro_cuota")
                Dim nro_subcuota As Integer = sender.DataKeys(Index)("nro_subcuota")

                If chk_AplicaTodos.Checked = True Then
                    For Each Row In gvd_CiasXBroker.Rows
                        gvd_Cuotas = CType(gvd_CiasXBroker.Rows(Row.rowIndex).FindControl("gvd_Cuotas"), GridView)
                        QuitarCuota(gvd_Cuotas, nro_Cuota, nro_subcuota)
                    Next
                Else
                    QuitarCuota(sender, nro_Cuota, nro_subcuota)
                End If

            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: ROWCOMMAND CUOTAS", ex.Message)
            LogError("(gvd_Cuotas_RowCommand)" & ex.Message)
        End Try
    End Sub

    Private Sub btn_Guardar_Click(sender As Object, e As EventArgs) Handles btn_Guardar.Click
        Try
            Dim Montos() As String

            EliminaCuotas(hid_idPv.Value, hid_nroreas.Value, txt_Capa.Text, Val(txt_Ramo.Text), txt_Contrato.Text, hid_nrotramo.Value)

            If ValidaCampoRequerido() Then
                Dim blnCambio As Boolean = GuardaCuotas()


                dtBusqueda = New DataTable
                dtBusqueda = Session("dtGeneral")

                Dim myRow() As Data.DataRow
                myRow = dtBusqueda.Select("id_pv ='" & hid_idPv.Value & "' AND nro_layer = '" & txt_Capa.Text & "' AND cod_ramo_contable = '" & Val(txt_Ramo.Text) & "' AND id_contrato = '" & txt_Contrato.Text & "'")
                myRow(0)("bln_Cambio") = blnCambio


                Dim img_Check As Image = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("img_Check"), Image)
                img_Check.Visible = blnCambio

                myRow = dtBusqueda.Select("id_pv ='" & hid_idPv.Value & "' AND Id_Pol=0")

                Montos = Split(ConsultaParcial(hid_idPv.Value), "|")
                myRow(0)("Parcial") = String.Format("{0:#,#0.00}", CDbl(Montos(0)))

                'Actualiza Cuotas
                'gvd_CiasXBroker.DataSource = ConsultaDetalle(hid_idPv.Value, txt_Capa.Text, Val(txt_Ramo.Text), txt_Contrato.Text, hid_nrotramo.Value, 0)
                'gvd_CiasXBroker.DataBind()

                gvd_CiasXBroker.DataSource = Nothing
                gvd_CiasXBroker.DataBind()


                For index As Integer = hid_Index.Value To 0 Step -1
                    If gvd_Reaseguro.DataKeys(index)("Id_Pol") = "0" Then
                        Dim lbl_Total2 As TextBox = TryCast(gvd_Reaseguro.Rows(index).FindControl("lbl_Total2"), TextBox)

                        Montos = Split(ConsultaParcial(hid_idPv.Value), "|")
                        lbl_Total2.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)))
                        Exit For
                    End If
                Next

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Modal Ordenes", "ClosePopup('#ExhibicionesModal');", True)

                Mensaje("ORDEN DE PAGO-:GUARDAR", "Los cambios fueron aplciados satisfactoriamente")
            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: GUARDA EXHIBICIONES", ex.Message)
            LogError("(btn_Guardar_Click)" & ex.Message)
        End Try
    End Sub

    Private Function ConsultaParcial(ByVal id_pv As String, Optional ByVal cod_broker As Integer = 0) As String
        Dim sCnn As String
        Dim da As SqlDataAdapter
        Dim dtRes As DataTable
        Dim Parciales As String = ""

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_SumaParcial '" & id_pv & "'," & cod_broker

        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        Return dtRes(0)("Parcial") & "|" & dtRes(0)("Impuesto")

    End Function

    Private Function ConsultaDetalle(ByVal id_pv As Double, ByVal nro_layer As Integer, ByVal cod_ramo_contable As Integer,
                                     ByVal id_contrato As String, ByVal nro_tramo As Integer, Optional ByVal sn_actualiza As Integer = 0,
                                     Optional ByVal nro_cuota As Integer = 0, Optional ByVal nro_rea As Integer = 0) As DataTable
        Dim sCnn As String
        Dim FiltroFecha As String = ""

        If IsDate(txtFecPagoDe.Text) And IsDate(txtFecPagoA.Text) Then
            If CDate(txtFecPagoDe.Text) <= CDate(txtFecPagoA.Text) Then
                FiltroFecha = " WHERE fecha >= ''" & FechaAIngles(txtFecPagoDe.Text) & "'' AND fecha < ''" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txtFecPagoA.Text))) & "''"
            End If
        End If

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_LstOP_Detalle " & id_pv & "," & nro_layer & "," & cod_ramo_contable & ",'" & id_contrato & "'," & nro_tramo & "," & sn_actualiza & ",'" & FiltroFecha & "'," & nro_cuota & "," & nro_rea
        Dim da As SqlDataAdapter

        dtDetalle = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtDetalle)

        If sn_actualiza = 0 Then
            Dim query = From row In dtDetalle.AsEnumerable()
                        Group row By cod_cia_reas_brok = row("cod_cia_reas_brok"), Broker = row("Broker"),
                                     cod_cia_reas_cia = row("cod_cia_reas_cia"), Compañia = row("Compañia"),
                                     PrimaCedidaCiaBrok = row("PrimaCedidaCiaBrok"), ComisionCiaBrok = row("ComisionCiaBrok"),
                                     nro_reas = row("nro_reas"), nro_tram = row("nro_tramo"),
                                     tipo_ISR = row("tipo_ISR"), ret_ISR = row("ret_ISR"), sn_Comp_RF = row("sn_Comp_RF")
                        Into CiasGrupo = Group
                        Select New With {
                                                            Key cod_cia_reas_brok,
                                                            Key Broker,
                                                            Key cod_cia_reas_cia,
                                                            Key Compañia,
                                                            Key PrimaCedidaCiaBrok,
                                                            Key ComisionCiaBrok,
                                                            Key nro_reas,
                                                            Key nro_tram,
                                                            Key tipo_ISR,
                                                            Key ret_ISR,
                                                            Key sn_Comp_RF
                                                        }


            Dim dtRes As DataTable
            dtRes = New DataTable

            dtRes.Columns.Add("cod_cia_reas_brok")
            dtRes.Columns.Add("Broker")
            dtRes.Columns.Add("cod_cia_reas_cia")
            dtRes.Columns.Add("Compañia")
            dtRes.Columns.Add("PrimaCedidaCiaBrok")
            dtRes.Columns.Add("ComisionCiaBrok")
            dtRes.Columns.Add("nro_reas")
            dtRes.Columns.Add("nro_tramo")
            dtRes.Columns.Add("tipo_ISR")
            dtRes.Columns.Add("ret_ISR")
            dtRes.Columns.Add("sn_Comp_RF")

            For Each Item In query
                dtRes.Rows.Add(Item.cod_cia_reas_brok, Item.Broker, Item.cod_cia_reas_cia, Item.Compañia, Item.PrimaCedidaCiaBrok, Item.ComisionCiaBrok, Item.nro_reas, Item.nro_tram, Item.tipo_ISR, Item.ret_ISR, Item.sn_Comp_RF)
            Next

            Return dtRes
        Else
            Return dtDetalle
        End If

    End Function

    Private Function ConsultaCuotas(ByVal id_pv As String) As DataTable
        Dim sCnn As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_CuotasTmp '" & id_pv & "'"

        Dim da As SqlDataAdapter

        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        Return dtRes
    End Function

    Private Sub EliminaCuotas(ByVal id_pv As String, Optional ByVal nro_reas As Integer = 0, Optional ByVal nro_layer As Integer = 0, Optional ByVal cod_ramo_contable As Integer = 0, Optional ByVal id_contrato As String = "", Optional ByVal nro_tramo As Integer = 0)
        Dim sCnn As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spD_CuotasTmp " & id_pv & "," & nro_reas & "," & nro_layer & "," & cod_ramo_contable & ",'" & id_contrato & "'," & nro_tramo

        Dim da As SqlDataAdapter

        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)
    End Sub

    Private Sub EliminaExhibiciones(ByVal id_pv As Double, ByVal nro_reas As Integer, ByVal nro_layer As Integer, ByVal cod_ramo_contable As Integer, ByVal id_contrato As String, ByVal nro_tramo As Integer, ByVal cod_broker As Integer, ByVal cod_cia As Integer, ByVal Cuotas As String)
        Dim sCnn As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spD_Exhibiciones " & id_pv & "," & nro_reas & "," & nro_layer & "," & cod_ramo_contable & ",'" & id_contrato & "'," & nro_tramo & "," & cod_broker & "," & cod_cia & ",'" & Cuotas & "'"

        Dim da As SqlDataAdapter

        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)
    End Sub

    Private Function ValidaCampoRequerido() As Boolean
        ValidaCampoRequerido = True
        If hid_TipoCambio.Value = 0 Then
            Mensaje("ORDEN DE PAGO-:Tipo de Cambio", "El Tipo de Cambio del día no ha sido capturado")
            ValidaCampoRequerido = False
            Exit Function
        End If

        For Each RowCias In gvd_CiasXBroker.Rows
            'Se obtiene el Grid de Cuotas por combinación
            Dim gvd_Cuotas As GridView = TryCast(RowCias.FindControl("gvd_Cuotas"), GridView)

            For Each RowCuotas As GridViewRow In gvd_Cuotas.Rows

                Dim chk_SelCuota As CheckBox = CType(RowCuotas.FindControl("chk_SelCuota"), CheckBox)
                Dim txt_Cuota As TextBox = TryCast(RowCuotas.FindControl("txt_Cuota"), TextBox)
                Dim lbl_prcPri As TextBox = TryCast(RowCuotas.FindControl("lbl_prcPri"), TextBox)
                Dim lbl_Fecha As TextBox = TryCast(RowCuotas.FindControl("lbl_Fecha"), TextBox)

                If Not IsNumeric(txt_Cuota.Text) Then
                    Mensaje("ORDEN DE PAGO-:Cuotas", "El formato del Número de Cuota es incorrecto, debe ser númerico")
                    ValidaCampoRequerido = False
                    txt_Cuota.Focus()
                    Exit Function
                ElseIf Not IsDate(lbl_Fecha.Text) Then
                    Mensaje("ORDEN DE PAGO-:Cuotas", "El formato de Fecha es incorrecto")
                    ValidaCampoRequerido = False
                    lbl_Fecha.Focus()
                    Exit Function
                End If

                'Se verifica si se guardará la exhibición
                If chk_SelCuota.Checked = True Then
                    If CDbl(lbl_prcPri.Text) = 0 Then
                        Mensaje("ORDEN DE PAGO-:Cuotas", "La Prima debe ser mayor a cero")
                        ValidaCampoRequerido = False
                        lbl_prcPri.Focus()
                        Exit Function
                    ElseIf CDate(lbl_Fecha.Text) < Today Then
                        Mensaje("ORDEN DE PAGO-:Cuotas", "La fecha de exhibición no puede ser menor al dia de hoy")
                        ValidaCampoRequerido = False
                        lbl_Fecha.Focus()
                        Exit Function
                    End If
                End If
            Next
        Next
    End Function
    Private Function GuardaCuotas() As Boolean
        Dim Resultado As String
        Dim strDatos As String = ""
        Dim strExhibicionAdd As String = ""

        Dim sn_Origen As String = ""
        Dim nro_cuota As String = ""
        Dim nro_cuota_reas As String = ""
        Dim nro_subcuota As String = ""
        Dim sn_cambio As String = ""
        Dim strCuotas As String = ""

        GuardaCuotas = False

        For Each RowCias As GridViewRow In gvd_CiasXBroker.Rows
            strCuotas = ""
            strDatos = ""
            strExhibicionAdd = ""
            Dim cod_cia_reas_brok As String = gvd_CiasXBroker.DataKeys(RowCias.RowIndex)("cod_cia_reas_brok")
            Dim cod_cia_reas_cia As String = gvd_CiasXBroker.DataKeys(RowCias.RowIndex)("cod_cia_reas_cia")
            Dim PrimaCedidaCiaBrok As Double = gvd_CiasXBroker.DataKeys(RowCias.RowIndex)("PrimaCedidaCiaBrok")
            Dim ComisionCiaBrok As Double = gvd_CiasXBroker.DataKeys(RowCias.RowIndex)("ComisionCiaBrok")

            'Se obtiene el Grid de Cuotas por combinación
            Dim gvd_Cuotas As GridView = TryCast(RowCias.FindControl("gvd_Cuotas"), GridView)

            'Valida si existen cambios en las cuotas para borrar y almacenar
            For Each RowCuotas As GridViewRow In gvd_Cuotas.Rows
                sn_Origen = gvd_Cuotas.DataKeys(RowCuotas.RowIndex)("sn_Origen")
                nro_cuota = gvd_Cuotas.DataKeys(RowCuotas.RowIndex)("nro_cuota")
                nro_cuota_reas = TryCast(RowCuotas.FindControl("txt_Cuota"), TextBox).Text
                nro_subcuota = gvd_Cuotas.DataKeys(RowCuotas.RowIndex)("nro_subcuota")
                sn_cambio = TryCast(RowCuotas.FindControl("hid_Cambio"), TextBox).Text
                If (sn_Origen = "" Or sn_Origen = "0") Or (sn_cambio = "1") Or (nro_cuota <> nro_cuota_reas) Then
                    strCuotas = strCuotas & "''" & nro_cuota & "-" & nro_subcuota & "'',"
                End If
            Next

            If Len(strCuotas) > 0 Then
                strCuotas = Mid(strCuotas, 1, Len(strCuotas) - 1)
                'Elimina exhibiciones adicionales cargadas
                EliminaExhibiciones(hid_idPv.Value, hid_nroreas.Value, txt_Capa.Text, Val(txt_Ramo.Text), txt_Contrato.Text, hid_nrotramo.Value, cod_cia_reas_brok, cod_cia_reas_cia, strCuotas)
            End If

            For Each RowCuotas As GridViewRow In gvd_Cuotas.Rows

                Dim chk_SelCuota As CheckBox = CType(RowCuotas.FindControl("chk_SelCuota"), CheckBox)

                nro_cuota = gvd_Cuotas.DataKeys(RowCuotas.RowIndex)("nro_cuota")
                nro_cuota_reas = TryCast(RowCuotas.FindControl("txt_Cuota"), TextBox).Text
                nro_subcuota = gvd_Cuotas.DataKeys(RowCuotas.RowIndex)("nro_subcuota")
                Dim pje_pri As String = TryCast(RowCuotas.FindControl("lbl_prcPri"), TextBox).Text
                Dim pje_com As String = TryCast(RowCuotas.FindControl("lbl_prcCom"), TextBox).Text
                Dim fecha As String = FechaAIngles(TryCast(RowCuotas.FindControl("lbl_Fecha"), TextBox).Text)
                Dim cod_cptoPri As Integer = gvd_Cuotas.DataKeys(RowCuotas.RowIndex)("cod_cptoPri")
                Dim cod_cptoCom As Integer = gvd_Cuotas.DataKeys(RowCuotas.RowIndex)("cod_cptoCom")

                sn_cambio = TryCast(RowCuotas.FindControl("hid_Cambio"), TextBox).Text

                'Se verifica si se guardará la exhibición
                If chk_SelCuota.Checked = True And chk_SelCuota.Enabled = True Then
                    Dim sn_Seleccion As String = IIf(chk_SelCuota.Checked = True, 1, 0)
                    Dim cod_moneda As String = gvd_Cuotas.DataKeys(RowCuotas.RowIndex)("cod_moneda")

                    strDatos = strDatos & "(@strKey," & cod_cia_reas_brok & "," & cod_cia_reas_cia & "," & nro_cuota & "," & nro_subcuota & "," &
                                                        sn_Seleccion & "," & cod_cptoPri & "," & pje_pri & "," & PrimaCedidaCiaBrok & "," &
                                                        cod_cptoCom & "," & pje_com & "," & ComisionCiaBrok & ",''" & fecha & "''," & cod_moneda & "),"
                End If

                'Evalua si se trata de una exhibicion nueva, o de la corrección de una exhibicion.
                sn_Origen = gvd_Cuotas.DataKeys(RowCuotas.RowIndex)("sn_Origen")
                If ((sn_Origen = "" Or sn_Origen = "0") Or (sn_cambio = "1") Or (nro_cuota <> nro_cuota_reas) Or (Val(pje_pri) = 0 And Val(pje_com) = 0)) And nro_cuota <> 99 Then
                    strExhibicionAdd = strExhibicionAdd & "(@strKey," & cod_cia_reas_brok & "," & cod_cia_reas_cia & "," &
                                                                        nro_cuota & "," & nro_cuota_reas & "," & nro_subcuota & ",''" & fecha & "''," &
                                                                        PrimaCedidaCiaBrok * (pje_pri / 100) & "," & pje_pri & "," & pje_com & "),"
                End If
            Next

            Dim strKey = hid_idPv.Value & "," & hid_nroreas.Value & "," & txt_Capa.Text & "," & Val(txt_Ramo.Text) & ",''" & txt_Contrato.Text & "''," & hid_nrotramo.Value


            'GUARDA CAMBIOS EN EXHIBICIONES
            If Len(strDatos) > 0 Then
                strDatos = Mid(strDatos, 1, Len(strDatos) - 1)
                Resultado = GuardaDatos("tmp_OPControlCambios", strKey, strDatos)

                If Resultado <> "1" Then
                    Mensaje("ORDEN DE PAGO-: GUARDA CUOTAS", Resultado)
                    Exit Function
                Else
                    GuardaCuotas = True
                End If
            End If


            'GUARDA NUEVAS EXHIBICIONES PROGRAMADAS ADICIONALES
            If Len(strExhibicionAdd) > 0 Then
                strExhibicionAdd = Mid(strExhibicionAdd, 1, Len(strExhibicionAdd) - 1)
                Resultado = GuardaDatos("mtramos_cias_pagos_facOP", strKey, strExhibicionAdd)

                If Resultado <> "1" Then
                    Mensaje("ORDEN DE PAGO-: GUARDA NUEVAS EXHIBICIONES ADICIONALES", Resultado)
                    Exit Function
                End If
            End If
        Next

    End Function

    Private Sub btn_GenerarOP_Click(sender As Object, e As EventArgs) Handles btn_GenerarOP.Click
        Try
            lbl_MntParcial.Text = 0
            lbl_MntImpuesto.Text = 0
            lbl_MntTotal.Text = 0
            gvd_OrdenPago.DataSource = Nothing
            gvd_OrdenPago.DataBind()
            lbl_MntParcial.Text = ""
            Dim Montos() As String

            If ValidaCampoRequerido() Then
                lbl_Cuotas.Visible = False
                ddl_Cuotas.Visible = False
                btn_EvaluaOP.Visible = False
                EliminaCuotas(hid_idPv.Value, hid_nroreas.Value, txt_Capa.Text, Val(txt_Ramo.Text), txt_Contrato.Text, hid_nrotramo.Value)
                GuardaCuotas()
                Session("dtCuotas") = ConsultaCuotas(hid_idPv.Value)
                EvaluaOrdenPago(Session("dtCuotas"))

                If gvd_OrdenPago.Rows.Count = 0 Then
                    btn_ConfirmarOP.Visible = False
                    Mensaje("ORDEN DE PAGO-: GENERA", "No existe información para generar Órden de Pago")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Modal Ordenes", "ClosePopup('#OrdenesModal');", True)
                Else
                    lbl_Genera.Visible = True
                    ddl_TipoGenera.Visible = True
                    ddl_TipoGenera.SelectedValue = "EN"
                    btn_ConfirmarOP.Visible = True
                    Montos = Split(ConsultaParcial(hid_idPv.Value), "|")
                    lbl_MntParcial.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)))
                    lbl_MntImpuesto.Text = String.Format("{0:#,#0.00}", CDbl(Montos(1)))
                    lbl_MntTotal.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)) - CDbl(Montos(1)))

                    dtBusqueda = New DataTable
                    dtBusqueda = Session("dtGeneral")

                    Dim myRow() As Data.DataRow
                    myRow = dtBusqueda.Select("id_pv ='" & hid_idPv.Value & "' AND Id_Pol=0")
                    myRow(0)("Parcial") = lbl_MntParcial.Text

                    ddl_MonedaEnd.SelectedValue = hid_Moneda.Value
                End If
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Modal Ordenes", "ClosePopup('#OrdenesModal');", True)
            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: EVALUA ORDEN PAGO", ex.Message)
            LogError("(btn_GenerarOP_Click)" & ex.Message)
        End Try
    End Sub

    Private Sub btn_ConfirmarOP_Click(sender As Object, e As EventArgs) Handles btn_ConfirmarOP.Click
        Try
            Dim PrimaPagada As Double = 0
            Dim ComPagada As Double = 0
            Dim strPolizas As String = ""
            Dim dtActualiza As DataTable

            For Each Row In gvd_OrdenPago.Rows
                Dim txt_FechaPago As TextBox = TryCast(Row.FindControl("txt_FechaPago"), TextBox)
                Dim hid_nro_cuenta As HiddenField = TryCast(Row.FindControl("hid_nro_cuenta"), HiddenField)
                Dim opt_TipoPago As RadioButtonList = TryCast(Row.FindControl("opt_TipoPago"), RadioButtonList)

                If Not IsDate(txt_FechaPago.Text) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "Open Modal Ordenes", "OpenPopup('#OrdenesModal');", True)
                    Mensaje("ORDEN DE PAGO-: GENERA", "La Fecha de Pago no es válida")
                    Exit Sub
                ElseIf CDate(txt_FechaPago.Text) < Now.ToString("dd/MM/yyyy") Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "Open Modal Ordenes", "OpenPopup('#OrdenesModal');", True)
                    Mensaje("ORDEN DE PAGO-: GENERA", "La Fecha de Pago no puede ser menor al día de hoy")
                    Exit Sub
                ElseIf (Len(hid_nro_cuenta.Value) = 0 Or hid_nro_cuenta.Value = "0") And opt_TipoPago.SelectedValue = "T" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "Open Modal Ordenes", "OpenPopup('#OrdenesModal');", True)
                    Mensaje("ORDEN DE PAGO-: GENERA", "Se debe especificar una Cuenta Bancaria")
                    Exit Sub
                End If
            Next

            'Genera Ordenes de Pago
            GeneraOrdenPago(hid_idPv.Value, Session("dtCuotas"))

            'Si se realiza mediante la pantalla de cuotas
            If hid_Paquete.Value = "0" Then
                'Actualiza Cuotas


                gvd_CiasXBroker.DataSource = ConsultaDetalle(hid_idPv.Value, txt_Capa.Text, Val(txt_Ramo.Text), txt_Contrato.Text, hid_nrotramo.Value, 0, 0, hid_nroreas.Value)
                gvd_CiasXBroker.DataBind()

                dtBusqueda = New DataTable
                dtBusqueda = Session("dtGeneral")

                dtActualiza = New DataTable
                dtActualiza = ConsultaDetalle(hid_idPv.Value, txt_Capa.Text, Val(txt_Ramo.Text), txt_Contrato.Text, hid_nrotramo.Value, 1, 0, hid_nroreas.Value)

                Dim Row() As Data.DataRow
                Row = dtBusqueda.Select("id_pv ='" & hid_idPv.Value & "'")

                Dim lbl_PrimaPagada As Label = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("lbl_PrimaPagada"), Label)
                Dim lbl_ComAplicada As Label = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("lbl_ComAplicada"), Label)

                Dim lbl_SaldoPrima As Label = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("lbl_SaldoPrima"), Label)
                Dim lbl_SaldoCom As Label = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("lbl_SaldoCom"), Label)

                Dim img_Check As Image = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("img_Check"), Image)


                For Each item In dtActualiza.AsEnumerable
                    For Each columna In Row
                        If item("Id_pol") = 0 Then 'Se trata del encabezado
                            If columna("Id_Pol") = 0 Then
                                columna("PrimaPagada") = item("Prima")
                                columna("ComPagada") = item("Comision")

                                columna("SaldoPrima") = columna("PrimaCedida") - columna("PrimaPagada")
                                columna("SaldoComision") = columna("Comision") - columna("ComPagada")

                                PrimaPagada = CDbl(columna("PrimaPagada"))
                                ComPagada = CDbl(columna("ComPagada"))
                                Exit For
                            End If
                        Else
                            If columna("Id_Pol") <> 0 Then
                                If item("nro_layer") = columna("nro_layer") And
                                   item("cod_ramo_contable") = columna("cod_ramo_contable") And
                                   item("id_contrato") = columna("id_contrato") And
                                   item("nro_tramo") = columna("nro_tramo") Then

                                    columna("PrimaPagada") = item("Prima")
                                    columna("ComPagada") = item("Comision")

                                    columna("SaldoPrima") = columna("PrimaCedida") - columna("PrimaPagada")
                                    columna("SaldoComision") = columna("Comision") - columna("ComPagada")

                                    columna("cod_estatus_op") = 1
                                    columna("bln_Cambio") = False

                                    lbl_PrimaPagada.Text = String.Format("{0:#,#0.00}", CDbl(columna("PrimaPagada")))
                                    lbl_ComAplicada.Text = String.Format("{0:#,#0.00}", CDbl(columna("ComPagada")))

                                    lbl_SaldoPrima.Text = String.Format("{0:#,#0.00}", CDbl(columna("SaldoPrima")))
                                    lbl_SaldoCom.Text = String.Format("{0:#,#0.00}", CDbl(columna("SaldoComision")))

                                    img_Check.Visible = False

                                    Exit For
                                End If
                            End If
                        End If
                    Next
                Next

                Dim div_capa As HtmlGenericControl = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("div_capa"), HtmlGenericControl)
                Dim div_prima As HtmlGenericControl = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("div_prima"), HtmlGenericControl)
                Dim div_comision As HtmlGenericControl = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("div_comision"), HtmlGenericControl)
                Dim lbl_TitCapa As Label = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("lbl_TitCapa"), Label)
                Dim lbl_TitRamo As Label = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("lbl_TitRamo"), Label)
                Dim lbl_TitContrato As Label = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("lbl_TitContrato"), Label)

                div_capa.Style("background-color") = "Yellow"
                div_prima.Style("background-color") = "Yellow"
                div_comision.Style("background-color") = "Yellow"
                div_capa.Style("color") = "Black"
                div_prima.Style("color") = "Black"
                div_comision.Style("color") = "Black"
                lbl_TitCapa.ForeColor = Drawing.Color.DarkBlue
                lbl_TitRamo.ForeColor = Drawing.Color.DarkBlue
                lbl_TitContrato.ForeColor = Drawing.Color.DarkBlue

                For index As Integer = hid_Index.Value To 0 Step -1
                    If gvd_Reaseguro.DataKeys(index)("Id_Pol") = "0" Then
                        Dim PrimaCedida As Double = gvd_Reaseguro.DataKeys(index)("PrimaCedida")
                        Dim ComisionCed As Double = gvd_Reaseguro.DataKeys(index)("Comision")


                        lbl_PrimaPagada = TryCast(gvd_Reaseguro.Rows(index).FindControl("lbl_PrimaPagada"), Label)
                        lbl_PrimaPagada.Text = String.Format("{0:#,#0.00}", PrimaPagada)

                        lbl_SaldoPrima = TryCast(gvd_Reaseguro.Rows(index).FindControl("lbl_SaldoPrima"), Label)
                        lbl_SaldoPrima.Text = String.Format("{0:#,#0.00}", PrimaCedida - PrimaPagada)


                        lbl_ComAplicada = TryCast(gvd_Reaseguro.Rows(index).FindControl("lbl_ComAplicada"), Label)
                        lbl_ComAplicada.Text = String.Format("{0:#,#0.00}", ComPagada)

                        lbl_SaldoCom = TryCast(gvd_Reaseguro.Rows(index).FindControl("lbl_SaldoCom"), Label)
                        lbl_SaldoCom.Text = String.Format("{0:#,#0.00}", ComisionCed - ComPagada)
                        Exit For
                    End If
                Next
            Else
                For Each Row In gvd_Reaseguro.Rows
                    If gvd_Reaseguro.DataKeys(Row.rowIndex)("Id_Pol") = "0" Then
                        strPolizas = strPolizas & "''" & gvd_Reaseguro.DataKeys(Row.rowIndex)("Poliza") & "'',"
                    End If
                Next

                'Obtiene el Grid General
                dtBusqueda = New DataTable
                dtBusqueda = Session("dtGeneral")

                Dim sCnn As String = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
                Session("sSel") = Replace(Session("sSel"), "@strPolizas", Mid(strPolizas, 1, Len(strPolizas) - 1))

                Dim da As SqlDataAdapter
                dtActualiza = New DataTable
                da = New SqlDataAdapter(Session("sSel"), sCnn)
                da.SelectCommand.CommandTimeout = 20000
                da.Fill(dtActualiza)


                For Each Item In dtActualiza.Rows
                    Dim Row() As Data.DataRow
                    If Item("Id_Pol") = 0 Then
                        Row = dtBusqueda.Select("id_pv ='" & Item("id_pv") & "' AND Id_Pol = 0")
                    Else
                        Row = dtBusqueda.Select("id_pv ='" & Item("id_pv") & "' AND nro_layer = '" & Item("nro_layer") & "' AND cod_ramo_contable = '" & Item("cod_ramo_contable") & "' AND id_contrato = '" & Item("id_contrato") & "' AND nro_tramo = '" & Item("nro_tramo") & "'")
                    End If

                    Row(0)("PrimaPagada") = Item("PrimaPagada")
                    Row(0)("ComPagada") = Item("ComPagada")
                    Row(0)("SaldoPrima") = Item("SaldoPrima")
                    Row(0)("SaldoComision") = Item("SaldoComision")
                    Row(0)("Parcial") = Item("Parcial")
                    Row(0)("bln_Cambio") = 0
                    Row(0)("cod_estatus_op") = 1
                Next
                Session("dtGeneral") = dtBusqueda
                gvd_Reaseguro.DataSource = Session("dtGeneral")
                gvd_Reaseguro.DataBind()

            End If

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: GENERA ORDEN PAGO", ex.Message)
            LogError("(btn_ConfirmarOP_Click)" & ex.Message)
        End Try
    End Sub

    Private Function ObtieneInfoAdicional() As String()
        Dim arrayAdicional() As String = Nothing
        Dim i As Integer = 0

        For Each Row As GridViewRow In gvd_OrdenPago.Rows
            Dim txt_FechaPago As TextBox = TryCast(Row.FindControl("txt_FechaPago"), TextBox)
            Dim opt_TipoPago As RadioButtonList = TryCast(Row.FindControl("opt_TipoPago"), RadioButtonList)
            Dim txt_texto As TextBox = TryCast(Row.FindControl("txt_texto"), TextBox)

            ReDim Preserve arrayAdicional(i)
            arrayAdicional(i) = txt_FechaPago.Text & "|" & IIf(opt_TipoPago.SelectedValue = "T", -1, 0) & "|" & txt_texto.Text
            i = i + 1

        Next

        Return arrayAdicional
    End Function

    Private Function GuardaTexto(ByVal nro_op As Integer, ByVal Texto As String) As String

        Dim sCnn As String


        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spI_TextoOP " & nro_op & ",'" & Texto & "','',''"

        Dim da As SqlDataAdapter

        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        Return dtRes(0)("Resultado")
    End Function

    'Private Sub btn_CerrarExhibiciones_Click(sender As Object, e As EventArgs) Handles btn_CerrarExhibiciones.Click
    '    Try
    '        hid_idPv.Value = 0
    '        hid_nroreas.Value = 0
    '        txt_Poliza.Text = ""
    '        txt_Capa.Text = ""
    '        txt_Ramo.Text = ""
    '        txt_Contrato.Text = ""
    '        hid_nrotramo.Value = 0
    '        gvd_CiasXBroker.DataSource = Nothing
    '        gvd_CiasXBroker.DataBind()

    '    Catch ex As Exception
    '        Mensaje("ORDEN DE PAGO-: CIERRA VENTANA EXHIBICIONES", ex.Message)
    '    End Try
    'End Sub

    Private Sub btn_BuscaEndoso_Click(sender As Object, e As EventArgs) Handles btn_BuscaEndoso.Click
        Try
            Dim strSel As String = ""


            'If chk_Vencidas.Checked = False Then
            'gvd_GrupoPolizas.AllowPaging = False
            If txtClaveRam.Text = "" Or txtSearchRam.Text = "" Then
                Mensaje("ORDEN DE PAGO-:Busqueda", "Se debe especificar un Ramo de Póliza")
                Exit Sub
            End If

            If txt_NoPoliza.Text = "" Then
                Mensaje("ORDEN DE PAGO-:Busqueda", "Se debe especificar un Número de Póliza")
                Exit Sub
            End If

            'Else
            '    gvd_GrupoPolizas.AllowPaging = True
            '    gvd_GrupoPolizas.PageSize = 13
            'End If


            Dim sCnn As String

            sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

            Dim Polizas As String = Replace(Replace(hid_Polizas.Value, "==", "'"), ",'''-1'',", "'")

            If Len(Polizas) = 0 Then
                Polizas = "'" & Polizas & "'"
            End If

            Dim sSel As String = "spS_ListaPoliza " & ddl_SucursalPol.SelectedValue & "," & IIf(txtClaveRam.Text = "", 1, txtClaveRam.Text) & "," & IIf(txt_NoPoliza.Text = "", 1, txt_NoPoliza.Text) & "," & Polizas & "," & IIf(chk_Vencidas.Checked = True, 1, 0) & ",'" & FechaAIngles(txt_FechaIni.Text) & "','" & FechaAIngles(txt_FechaFin.Text) & "'," & optAjuste.SelectedValue

            Dim da As SqlDataAdapter

            Dim dtRes As DataTable
            dtRes = New DataTable

            da = New SqlDataAdapter(sSel, sCnn)

            da.Fill(dtRes)

            Session("dtPolizas") = dtRes

            If dtRes.Rows.Count > 0 Then
                gvd_GrupoPolizas.DataSource = dtRes
                gvd_GrupoPolizas.DataBind()
                chk_Vencidas.Enabled = False
                txt_FechaIni.Enabled = False
                txt_FechaFin.Enabled = False
                ddl_SucursalPol.Enabled = False
                txtClaveRam.Enabled = False
                txtSearchRam.Enabled = False
                txt_NoPoliza.Enabled = False
                btn_BuscaEndoso.Enabled = False
                btn_CancelaEndoso.Enabled = True
            Else
                gvd_GrupoPolizas.DataSource = Nothing
                gvd_GrupoPolizas.DataBind()
                btn_CancelaEndoso_Click(Me, Nothing)
                Mensaje("ORDEN DE PAGO-:Busqueda", "No hay resultados que mostrar")
            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: BUSCA ENDOSOS", ex.Message)
            LogError("(btn_BuscaEndoso_Click)" & ex.Message)
        End Try
    End Sub

    Private Sub btn_CancelaEndoso_Click(sender As Object, e As EventArgs) Handles btn_CancelaEndoso.Click
        Try
            gvd_GrupoPolizas.DataSource = Nothing
            gvd_GrupoPolizas.DataBind()
            ddl_SucursalPol.Enabled = True
            txtClaveRam.Enabled = True
            txtSearchRam.Enabled = True
            txt_NoPoliza.Enabled = True
            btn_BuscaEndoso.Enabled = True
            btn_CancelaEndoso.Enabled = False
            chk_Vencidas.Enabled = True
            chk_Vencidas_CheckedChanged(Nothing, Nothing)
            Session("dtPolizas") = Nothing
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: CANCELA BUSQUEDA ENDOSOS", ex.Message)
            LogError("(btn_CancelaEndoso_Click)" & ex.Message)
        End Try
    End Sub

    Private Function ConsultaOrdenesPago() As DataTable
        Dim FiltroContrato As String = ""
        Dim FiltroBroCia As String = ""
        Dim FiltroPol As String = ""
        Dim FiltroFecha As String = ""
        Dim FiltroFechaGen As String = ""
        Dim FiltroRamoCont As String = ""
        Dim FiltroUsuario As String = ""
        Dim FiltroEstatus As String = ""
        Dim Elementos As String
        Dim sCnn As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Elementos = ObtieneElementos(gvd_Broker, "Bro", False)
        If Elementos <> "" Then
            FiltroBroCia = Elementos
        End If

        Elementos = ObtieneElementos(gvd_Compañia, "Cia", False)
        If Elementos <> "" Then
            FiltroBroCia = FiltroBroCia & IIf(Len(FiltroBroCia) > 0, ",", "") & Elementos
        End If


        Elementos = ObtieneElementos(gvd_Contrato, "Fac", True)
        If Elementos <> "" Then
            FiltroContrato = Elementos
        End If


        Elementos = ObtieneElementos(gvd_Poliza, "Pol", True)
        If Elementos <> "" Then
            FiltroPol = Elementos
        End If

        If IsDate(txtFecPagoDe.Text) And IsDate(txtFecPagoA.Text) Then
            If CDate(txtFecPagoDe.Text) <= CDate(txtFecPagoA.Text) Then
                FiltroFecha = " AND fec_pago >= ''" & FechaAIngles(txtFecPagoDe.Text) & "'' AND fec_pago < ''" & FechaAIngles(DateAdd(DateInterval.Day, 1, CDate(txtFecPagoA.Text))) & "''"
            End If
        End If

        'If Len(txtClaveRamCont.Text) > 0 Then
        '    FiltroRamoCont = txtClaveRamCont.Text
        'End If

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

        Dim sSel As String = "spS_OrdenPago 0,'" & FiltroBroCia & "','" & FiltroContrato & "','" & FiltroPol & "','" & FiltroFecha & "','" & FiltroRamoCont & "','" & FiltroUsuario & "','" & FiltroEstatus & "','" & FiltroFechaGen & "'"

        Dim da As SqlDataAdapter

        dtBusqueda = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.SelectCommand.CommandTimeout = 10000

        da.Fill(dtBusqueda)

        Session("dtOrdenPago") = dtBusqueda

        Return dtBusqueda

    End Function

    Private Sub btn_BuscaOP_Click(sender As Object, e As EventArgs) Handles btn_BuscaOP.Click
        Try
            Dim Ramos() As String

            gvd_LstOrdenPago.DataSource = ConsultaOrdenesPago()
            gvd_LstOrdenPago.DataBind()

            If gvd_LstOrdenPago.Rows.Count > 0 Then
                btn_BuscaOP.Enabled = False
                btn_Imprimir.Enabled = True

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

    Private Sub btn_Limpiar_Click(sender As Object, e As EventArgs) Handles btn_Limpiar.Click
        Try
            gvd_LstOrdenPago.DataSource = Nothing
            gvd_LstOrdenPago.DataBind()
            Session("dtOrdenPago") = Nothing
            btn_BuscaOP.Enabled = True
            btn_Imprimir.Enabled = False
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: LIMPIA ORDENES DE PAGO", ex.Message)
            LogError("(btn_Limpiar_Click)" & ex.Message)
        End Try
    End Sub



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
                    'btnCorregir.Visible = False
                    'btnRestaurar.Visible = False
                ElseIf IsDate(fec_pago) Then
                    lbl_OrdenPago.BackColor = Drawing.Color.Green
                    lbl_Estatus.BackColor = Drawing.Color.Green
                    lbl_OrdenPago.ForeColor = Drawing.Color.White
                    lbl_Estatus.ForeColor = Drawing.Color.White
                    btnCorregir.Visible = False
                    btnRestaurar.Visible = False
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

    Private Function ActualizaDistribucion(ByVal id_pv As Integer, ByVal MontoGenerar As Double, ByVal Cuotas As String) As Integer
        Dim sCnn As String = ""
        Dim sSel As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spU_CuotasDistribucion " & id_pv & "," & MontoGenerar & ",'" & Cuotas & "'"

        Dim da As SqlDataAdapter
        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        Return dtRes.Rows(0)("Distribucion")

    End Function

    Private Sub btn_EvaluaOP_Click(sender As Object, e As EventArgs) Handles btn_EvaluaOP.Click
        Try
            Dim Montos() As String
            hid_TipoCambio.Value = ObtieneTipoCambio(Today.ToString("dd/MM/yyyy"), 1)  'Tipo de Cambio Dolares

            If hid_TipoCambio.Value = 0 Then
                Mensaje("ORDEN DE PAGO-: TIPO DE CAMBIO", "No se puede realizar la operación ya que no se ha capturado el Tipo de Cambio del día")
                Exit Sub
            End If

            dtBusqueda = New DataTable
            dtBusqueda = Session("dtGeneral")

            Dim myRow() As Data.DataRow
            myRow = dtBusqueda.Select("id_pv ='" & hid_idPv.Value & "' AND id_pol > 0")


            For Each item In myRow
                txt_Poliza.Text = item("cod_suc") & "-" &
                                  item("cod_ramo") & "-" &
                                  item("nro_pol") & "-" &
                                  item("aaaa_endoso") & "-" &
                                  item("nro_endoso")

                txt_Capa.Text = item("nro_layer")
                txt_Ramo.Text = item("cod_ramo_contable") & " .- " & item("ramo_contable")
                txt_Contrato.Text = item("id_contrato")
                hid_nroreas.Value = item("nro_reas")
                hid_nrotramo.Value = item("nro_tramo")

                gvd_CiasXBroker.DataSource = ConsultaDetalle(hid_idPv.Value, txt_Capa.Text, Val(txt_Ramo.Text), txt_Contrato.Text, hid_nrotramo.Value, 0, ddl_Cuotas.SelectedValue, hid_nroreas.Value)
                gvd_CiasXBroker.DataBind()

                GuardaCuotas()
            Next

            'Verifica si se realiza por Monto en Distribución
            Dim lbl_Total2 As TextBox = TryCast(gvd_Reaseguro.Rows(hid_Index.Value).FindControl("lbl_Total2"), TextBox)
            If lbl_Total2.Enabled = True And Val(Replace(lbl_Total2.Text, ",", "")) <> 0 Then
                ActualizaDistribucion(hid_idPv.Value, Replace(lbl_Total2.Text, ",", ""), ddl_Cuotas.SelectedValue)
            End If

            Session("dtCuotas") = ConsultaCuotas(hid_idPv.Value)

            EvaluaOrdenPago(Session("dtCuotas"))

            If gvd_OrdenPago.Rows.Count > 0 Then
                lbl_Cuotas.Visible = False
                ddl_Cuotas.Visible = False
                btn_EvaluaOP.Visible = False
                btn_ConfirmarOP.Visible = True
                lbl_Genera.Visible = True
                ddl_TipoGenera.Visible = True
            End If

            Montos = Split(ConsultaParcial(hid_idPv.Value), "|")
            lbl_MntParcial.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)))
            lbl_MntImpuesto.Text = String.Format("{0:#,#0.00}", CDbl(Montos(1)))
            lbl_MntTotal.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)) - CDbl(Montos(1)))

            'lbl_MntParcial.Text = String.Format("{0:#,#0.00}", ConsultaParcial(hid_idPv.Value))
            myRow = dtBusqueda.Select("id_pv ='" & hid_idPv.Value & "' AND Id_Pol=0")
            myRow(0)("Parcial") = lbl_MntParcial.Text
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: EVALUA OP", ex.Message)
            LogError("(btn_EvaluaOP_Click)" & ex.Message)
        End Try
    End Sub

    Private Sub gvd_GrupoPolizas_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvd_GrupoPolizas.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim cod_grupo_endo As Integer = sender.DataKeys(e.Row.RowIndex)("cod_grupo_endo")

                Dim txt_Sufijo As TextBox = TryCast(e.Row.FindControl("txt_Sufijo"), TextBox)
                Dim txt_Endoso As TextBox = TryCast(e.Row.FindControl("txt_Endoso"), TextBox)
                Dim lbl_Desde As TextBox = TryCast(e.Row.FindControl("lbl_Desde"), TextBox)
                Dim lbl_Hasta As TextBox = TryCast(e.Row.FindControl("lbl_Hasta"), TextBox)
                Dim lbl_GrupoEndoso As TextBox = TryCast(e.Row.FindControl("lbl_GrupoEndoso"), TextBox)
                Dim lbl_GrupoTipoEndoso As TextBox = TryCast(e.Row.FindControl("lbl_GrupoTipoEndoso"), TextBox)
                Dim lbl_PrimaEmitida As TextBox = TryCast(e.Row.FindControl("lbl_PrimaEmitida"), TextBox)
                Dim lbl_PrimaAplicada As TextBox = TryCast(e.Row.FindControl("lbl_PrimaAplicada"), TextBox)
                Dim lbl_PrimaPagada As TextBox = TryCast(e.Row.FindControl("lbl_PrimaPagada"), TextBox)
                Dim lbl_Asegurado As TextBox = TryCast(e.Row.FindControl("lbl_Asegurado"), TextBox)
                Dim lbl_Emisor As TextBox = TryCast(e.Row.FindControl("lbl_Emisor"), TextBox)
                Dim lbl_Suscriptor As TextBox = TryCast(e.Row.FindControl("lbl_Suscriptor"), TextBox)
                Dim div_Ajuste As HtmlGenericControl = TryCast(e.Row.FindControl("div_Ajuste"), HtmlGenericControl)
                Dim txt_Ajuste As TextBox = TryCast(e.Row.FindControl("txt_Ajuste"), TextBox)

                If optAjuste.SelectedValue = 0 Then
                    div_Ajuste.Visible = False
                End If

                If cod_grupo_endo = 3 Or cod_grupo_endo = 15 Or cod_grupo_endo = 19 Then
                    txt_Sufijo.BackColor = Drawing.Color.Orange
                    txt_Endoso.BackColor = Drawing.Color.Orange
                    lbl_Desde.BackColor = Drawing.Color.Orange
                    lbl_Hasta.BackColor = Drawing.Color.Orange
                    lbl_GrupoEndoso.BackColor = Drawing.Color.Orange
                    lbl_GrupoTipoEndoso.BackColor = Drawing.Color.Orange
                    lbl_PrimaEmitida.BackColor = Drawing.Color.Orange
                    lbl_PrimaAplicada.BackColor = Drawing.Color.Orange
                    lbl_PrimaPagada.BackColor = Drawing.Color.Orange
                    lbl_Asegurado.BackColor = Drawing.Color.Orange
                    lbl_Emisor.BackColor = Drawing.Color.Orange
                    lbl_Suscriptor.BackColor = Drawing.Color.Orange
                    txt_Ajuste.BackColor = Drawing.Color.Orange
                ElseIf cod_grupo_endo = 1 Or cod_grupo_endo = 4 Then
                    txt_Sufijo.BackColor = Drawing.Color.LightGreen
                    txt_Endoso.BackColor = Drawing.Color.LightGreen
                    lbl_Desde.BackColor = Drawing.Color.LightGreen
                    lbl_Hasta.BackColor = Drawing.Color.LightGreen
                    lbl_GrupoEndoso.BackColor = Drawing.Color.LightGreen
                    lbl_GrupoTipoEndoso.BackColor = Drawing.Color.LightGreen
                    lbl_PrimaEmitida.BackColor = Drawing.Color.LightGreen
                    lbl_PrimaAplicada.BackColor = Drawing.Color.LightGreen
                    lbl_PrimaPagada.BackColor = Drawing.Color.LightGreen
                    lbl_Asegurado.BackColor = Drawing.Color.LightGreen
                    lbl_Emisor.BackColor = Drawing.Color.LightGreen
                    lbl_Suscriptor.BackColor = Drawing.Color.LightGreen
                    txt_Ajuste.BackColor = Drawing.Color.LightGreen
                End If
            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-: ROWDATABOUND", ex.Message)
            LogError("(gvd_GrupoPolizas_RowDataBound)" & ex.Message)
        End Try
    End Sub


    Private Sub chk_Vencidas_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Vencidas.CheckedChanged
        Try
            If chk_Vencidas.Checked = True Then
                ddl_SucursalPol.Enabled = False
                txtClaveRam.Enabled = False
                txtSearchRam.Enabled = False
                txt_NoPoliza.Enabled = False
            Else
                ddl_SucursalPol.Enabled = True
                txtClaveRam.Enabled = True
                txtSearchRam.Enabled = True
                txt_NoPoliza.Enabled = True
            End If

            If chk_Vencidas.Checked = True Then
                txt_FechaIni.Enabled = True
                txt_FechaFin.Enabled = True
            Else
                txt_FechaIni.Enabled = False
                txt_FechaFin.Enabled = False
            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(chk_Vencidas_CheckedChanged)" & ex.Message)
        End Try
    End Sub

    Private Function FechaAIngles(ByVal Fecha As String) As String
        If Fecha <> vbNullString Then
            Return String.Format("{0:MM/dd/yyyy}", CDate(Fecha))
            'Return String.Format("{0:dd/MM/yyyy}", CDate(Fecha))
        Else
            Return ""
        End If

    End Function

    Private Sub btn_Fraccionar_Click(sender As Object, e As EventArgs) Handles btn_Fraccionar.Click
        Try
            Dim gvd_Cuotas As GridView
            Dim hid_Prima As TextBox
            Dim hid_Comision As TextBox
            Dim lbl_IdBrokerOculto As Label
            Dim lbl_IdCiaOculto As Label
            Dim PrimaGenerar As Double
            Dim ComisionGenerar As Double
            Dim prcPrima As Double
            Dim prcCom As Double

            If CDbl(txt_MontoFraccionado.Text.Replace(",", "")) > CDbl(hid_PrimaReaseguro.Value) Then
                Mensaje("ORDEN DE PAGO-:", "No se puede fraccionar un Monto mayor a la Prima Neta de Reaseguro")
                Exit Sub
            ElseIf txt_MontoFraccionado.Text = 0 Or txt_MontoFraccionado.Text = vbNullString
                Mensaje("ORDEN DE PAGO-:", "Se debe especificar un Monto a fraccionar")
                Exit Sub
            End If

            Dim dtCuotas As DataTable
            dtCuotas = New DataTable
            dtCuotas.Columns.Add("id_pv")
            dtCuotas.Columns.Add("cod_cia_reas_brok")
            dtCuotas.Columns.Add("cod_cia_reas_cia")
            dtCuotas.Columns.Add("PrimaCedidaCiaBrok")
            dtCuotas.Columns.Add("ComisionCiaBrok")
            dtCuotas.Columns.Add("nro_cuota")
            dtCuotas.Columns.Add("nro_subcuota")
            dtCuotas.Columns.Add("Version")
            dtCuotas.Columns.Add("PrimaReaseguro")


            For Each RowCia In gvd_CiasXBroker.Rows
                gvd_Cuotas = CType(gvd_CiasXBroker.Rows(RowCia.rowIndex).FindControl("gvd_Cuotas"), GridView)
                lbl_IdBrokerOculto = CType(gvd_CiasXBroker.Rows(RowCia.rowIndex).FindControl("lbl_IdBrokerOculto"), Label)
                lbl_IdCiaOculto = CType(gvd_CiasXBroker.Rows(RowCia.rowIndex).FindControl("lbl_IdCiaOculto"), Label)
                hid_Prima = CType(gvd_CiasXBroker.Rows(RowCia.rowIndex).FindControl("hid_Prima"), TextBox)
                hid_Comision = CType(gvd_CiasXBroker.Rows(RowCia.rowIndex).FindControl("hid_Comision"), TextBox)

                For Each Row In gvd_Cuotas.Rows
                    Dim chk_SelCuota As CheckBox = CType(Row.FindControl("chk_SelCuota"), CheckBox)
                    If chk_SelCuota.Checked = True Then
                        Dim hid_Cuota As TextBox = TryCast(Row.FindControl("hid_Cuota"), TextBox)
                        Dim hidSubcuota As TextBox = TryCast(Row.FindControl("hidSubcuota"), TextBox)
                        Dim hid_Version As TextBox = TryCast(Row.FindControl("hid_Version"), TextBox)
                        dtCuotas.Rows.Add(hid_idPv.Value, lbl_IdBrokerOculto.Text, lbl_IdCiaOculto.Text, hid_Prima.Text, hid_Comision.Text, hid_Cuota.Text, hidSubcuota.Text, hid_Version.Text, hid_Prima.Text - hid_Comision.Text)
                    End If
                Next
            Next

            If dtCuotas.Rows.Count = 0 Then
                Mensaje("ORDEN DE PAGO-:", "Se debe seleccionar al menos una exhibicion para fraccionar un Monto")
                Exit Sub
            End If

            Dim PrimaXCias = From row In dtCuotas.AsEnumerable()
                             Group row By cod_cia_reas_brok = row("cod_cia_reas_brok"),
                                          cod_cia_reas_cia = row("cod_cia_reas_cia"),
                                          PrimaCedidaCiaBrok = row("PrimaCedidaCiaBrok"),
                                          ComisionCiaBrok = row("ComisionCiaBrok")
                             Into CiasGrupo = Group
                             Select New With {
                                                    Key cod_cia_reas_brok,
                                                    Key cod_cia_reas_cia,
                                                    Key PrimaCedidaCiaBrok,
                                                    Key ComisionCiaBrok,
                                                    Key .Cuotas = CiasGrupo.Count(Function(r) r.Field(Of String)("cod_cia_reas_brok"))
                                             }

            Dim dtPrimas As DataTable
            dtPrimas = New DataTable

            dtPrimas.Columns.Add("cod_cia_reas_brok")
            dtPrimas.Columns.Add("cod_cia_reas_cia")
            dtPrimas.Columns.Add("PrimaCedidaCiaBrok")
            dtPrimas.Columns.Add("ComisionCiaBrok")
            dtPrimas.Columns.Add("Cuotas")

            For Each Item In PrimaXCias
                dtPrimas.Rows.Add(Item.cod_cia_reas_brok, Item.cod_cia_reas_cia, Item.PrimaCedidaCiaBrok, Item.ComisionCiaBrok, Item.Cuotas)
            Next

            'Dim PrimaTotal = From row In dtCuotas.AsEnumerable()
            '                 Group row By id_pv = row("id_pv"),
            '                              cod_cia_reas_brok = row("cod_cia_reas_brok"),
            '                              cod_cia_reas_cia = row("cod_cia_reas_cia")
            '                 Into TotalGrupo = Group
            '                 Select New With {
            '                                     Key .PrimaCedTotal = TotalGrupo.Max(Function(r) r.Field(Of String)("PrimaReaseguro"))
            '                                 }

            'For Each Item In PrimaTotal
            '    PrimaCedTotal = PrimaCedTotal + Item.PrimaCedTotal
            'Next


            For Each item In dtCuotas.Rows
                'Se obtiene los Datos Totales por cada Corredor-Compañia
                Dim Cuotas() As Data.DataRow
                Cuotas = dtPrimas.Select("cod_cia_reas_brok ='" & item("cod_cia_reas_brok") & "' AND cod_cia_reas_cia ='" & item("cod_cia_reas_cia") & "'")

                PrimaGenerar = CDbl(txt_MontoFraccionado.Text.Replace(",", "")) * (item("PrimaReaseguro") / hid_PrimaReaseguro.Value) + (CDbl(txt_MontoFraccionado.Text.Replace(",", "")) * (item("PrimaReaseguro") / hid_PrimaReaseguro.Value)) * (item("ComisionCiaBrok") / item("PrimaReaseguro"))
                ComisionGenerar = (CDbl(txt_MontoFraccionado.Text.Replace(",", "")) * (item("PrimaReaseguro") / hid_PrimaReaseguro.Value)) * (item("ComisionCiaBrok") / item("PrimaReaseguro"))
                PrimaGenerar = PrimaGenerar / Cuotas(0)("Cuotas")
                ComisionGenerar = ComisionGenerar / Cuotas(0)("Cuotas")
                prcPrima = (PrimaGenerar / item("PrimaCedidaCiaBrok")) * 100
                prcCom = (ComisionGenerar / item("ComisionCiaBrok")) * 100

                For Each RowCia In gvd_CiasXBroker.Rows
                    gvd_Cuotas = CType(gvd_CiasXBroker.Rows(RowCia.rowIndex).FindControl("gvd_Cuotas"), GridView)
                    lbl_IdBrokerOculto = CType(gvd_CiasXBroker.Rows(RowCia.rowIndex).FindControl("lbl_IdBrokerOculto"), Label)
                    lbl_IdCiaOculto = CType(gvd_CiasXBroker.Rows(RowCia.rowIndex).FindControl("lbl_IdCiaOculto"), Label)
                    For Each Row In gvd_Cuotas.Rows
                        Dim chk_SelCuota As CheckBox = CType(Row.FindControl("chk_SelCuota"), CheckBox)
                        If chk_SelCuota.Checked = True Then
                            Dim hid_Cuota As TextBox = TryCast(Row.FindControl("hid_Cuota"), TextBox)
                            Dim hidSubcuota As TextBox = TryCast(Row.FindControl("hidSubcuota"), TextBox)
                            Dim hid_Version As TextBox = TryCast(Row.FindControl("hid_Version"), TextBox)
                            If lbl_IdBrokerOculto.Text = item("cod_cia_reas_brok") And lbl_IdCiaOculto.Text = item("cod_cia_reas_cia") Then
                                If hid_Cuota.Text = item("nro_cuota") And hidSubcuota.Text = item("nro_subcuota") And hid_Version.Text = item("Version") Then
                                    Dim lbl_prcPri As TextBox = TryCast(Row.FindControl("lbl_prcPri"), TextBox)
                                    Dim lbl_Prima As TextBox = TryCast(Row.FindControl("lbl_Prima"), TextBox)
                                    Dim lbl_prcCom As TextBox = TryCast(Row.FindControl("lbl_prcCom"), TextBox)
                                    Dim lbl_Comision As TextBox = TryCast(Row.FindControl("lbl_Comision"), TextBox)
                                    lbl_prcPri.Text = String.Format("{0:#,#0.00}", prcPrima)
                                    lbl_Prima.Text = String.Format("{0:#,#0.00}", PrimaGenerar)
                                    lbl_prcCom.Text = String.Format("{0:#,#0.00}", prcCom)
                                    lbl_Comision.Text = String.Format("{0:#,#0.00}", ComisionGenerar)
                                End If
                            End If
                        End If
                    Next
                Next
            Next

            txt_MontoFraccionado.Text = 0
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(btn_Fraccionar_Click)" & ex.Message)
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

    Private Sub ddl_TipoGenera_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_TipoGenera.SelectedIndexChanged
        Try
            Dim sCnn As String = ""
            Dim sSel As String = ""
            Dim Montos() As String

            If ddl_TipoGenera.SelectedValue = "EN" Then
                lbl_MntParcial.Text = 0
                lbl_MntImpuesto.Text = 0
                lbl_MntTotal.Text = 0
                gvd_OrdenPago.DataSource = Nothing
                gvd_OrdenPago.DataBind()
                lbl_MntParcial.Text = ""

                gvd_Endosos.DataSource = Nothing
                gvd_Endosos.DataBind()

                Session("dtCuotas") = ConsultaCuotas(hid_idPv.Value)
                EvaluaOrdenPago(Session("dtCuotas"))

                Montos = Split(ConsultaParcial(hid_idPv.Value), "|")
                lbl_MntParcial.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)))
                lbl_MntImpuesto.Text = String.Format("{0:#,#0.00}", CDbl(Montos(1)))
                lbl_MntTotal.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)) - CDbl(Montos(1)))
            Else
                sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

                sSel = "spS_EndososTemp " & IIf(ddl_TipoGenera.SelectedValue = "GE", hid_idPv.Value, 0) & "," & ddl_MonedaEnd.SelectedValue

                Dim da As SqlDataAdapter
                Dim dtRes As DataTable
                dtRes = New DataTable

                da = New SqlDataAdapter(sSel, sCnn)

                da.Fill(dtRes)

                gvd_Endosos.DataSource = dtRes
                gvd_Endosos.DataBind()

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Open Modal", "OpenPopup('#EndososModal');", True)
            End If

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(ddl_TipoGenera_SelectedIndexChanged)" & ex.Message)
        End Try
    End Sub

    Private Sub btn_OkEndosos_Click(sender As Object, e As EventArgs) Handles btn_OkEndosos.Click
        Try
            Dim Montos() As String
            Dim strDatos As String = ""

            For Each row As GridViewRow In gvd_Endosos.Rows
                Dim chk_SelPol = DirectCast(row.FindControl("chk_SelPol"), CheckBox)
                If chk_SelPol.Checked = True Then
                    strDatos = strDatos & "," & DirectCast(row.FindControl("hid_idpv"), HiddenField).Value
                End If
            Next

            If Len(strDatos) > 0 Then
                strDatos = Mid(strDatos, 2, Len(strDatos) - 1)
            Else
                Exit Sub
            End If

            lbl_MntParcial.Text = 0
            lbl_MntImpuesto.Text = 0
            lbl_MntTotal.Text = 0
            gvd_OrdenPago.DataSource = Nothing
            gvd_OrdenPago.DataBind()
            lbl_MntParcial.Text = ""

            Session("dtCuotas") = ConsultaCuotas(strDatos)
            EvaluaOrdenPago(Session("dtCuotas"))

            Montos = Split(ConsultaParcial(strDatos), "|")
            lbl_MntParcial.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)))
            lbl_MntImpuesto.Text = String.Format("{0:#,#0.00}", CDbl(Montos(1)))
            lbl_MntTotal.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)) - CDbl(Montos(1)))

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Modal", "ClosePopup('#EndososModal');", True)

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(btn_OkEndosos_Click)" & ex.Message)
        End Try
    End Sub

    Private Sub btn_CnlEndosos_Click(sender As Object, e As EventArgs) Handles btn_CnlEndosos.Click
        Try
            Dim Montos() As String

            ddl_TipoGenera.SelectedValue = "EN"
            lbl_MntParcial.Text = 0
            lbl_MntImpuesto.Text = 0
            lbl_MntTotal.Text = 0
            gvd_OrdenPago.DataSource = Nothing
            gvd_OrdenPago.DataBind()
            lbl_MntParcial.Text = ""

            Session("dtCuotas") = ConsultaCuotas(hid_idPv.Value)
            EvaluaOrdenPago(Session("dtCuotas"))

            Montos = Split(ConsultaParcial(hid_idPv.Value), "|")
            lbl_MntParcial.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)))
            lbl_MntImpuesto.Text = String.Format("{0:#,#0.00}", CDbl(Montos(1)))
            lbl_MntTotal.Text = String.Format("{0:#,#0.00}", CDbl(Montos(0)) - CDbl(Montos(1)))

            gvd_Endosos.DataSource = Nothing
            gvd_Endosos.DataBind()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Modal", "ClosePopup('#EndososModal');", True)

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(btn_CnlEndosos_Click)" & ex.Message)
        End Try
    End Sub

    Private Sub ddl_MonedaEnd_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_MonedaEnd.SelectedIndexChanged
        Try
            ddl_TipoGenera_SelectedIndexChanged(ddl_TipoGenera, Nothing)
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(ddl_MonedaEnd_SelectedIndexChanged)" & ex.Message)
        End Try
    End Sub

    Private Sub gvd_LstOrdenPago_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvd_LstOrdenPago.RowCommand
        Try

            If e.CommandName.Equals("Corregir") Then
                Dim Index As Integer = e.CommandSource.NamingContainer.RowIndex
                Dim nro_op As Integer = gvd_LstOrdenPago.DataKeys(Index)("nro_op")
                Dim id_imputacion As Integer = gvd_LstOrdenPago.DataKeys(Index)("id_imputacion")

                id_imputacion = CorrigeImputacion(nro_op, 0)
                id_imputacion = RecuperaMovimientos(nro_op, id_imputacion)

                Mensaje("ORDEN DE PAGO-:", "Se ha corregido el consecutivo de Imputación")

                btn_BuscaOP_Click(Nothing, Nothing)

            ElseIf e.CommandName.Equals("Restaurar") Then
                Dim Index As Integer = e.CommandSource.NamingContainer.RowIndex
                Dim nro_op As Integer = gvd_LstOrdenPago.DataKeys(Index)("nro_op")
                Dim id_imputacion As Integer = gvd_LstOrdenPago.DataKeys(Index)("id_imputacion")

                id_imputacion = RecuperaMovimientos(nro_op, id_imputacion)

                Mensaje("ORDEN DE PAGO-:", "Se han recuperado los movimientos originales de la Orden de Pago")
            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(gvd_LstOrdenPago_RowCommand)" & ex.Message)
        End Try
    End Sub

    Private Sub optAjuste_SelectedIndexChanged(sender As Object, e As EventArgs) Handles optAjuste.SelectedIndexChanged
        Try
            gvd_Poliza.DataSource = Nothing
            gvd_Poliza.DataBind()

            btn_CancelaEndoso_Click(btn_CancelaEndoso, Nothing)
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(optAjuste_SelectedIndexChanged)" & ex.Message)
        End Try
    End Sub


    'Protected Sub chk_DescartaPago_CheckedChanged(sender As Object, e As EventArgs)
    '    Try
    '        Dim gr As GridViewRow = DirectCast(DirectCast(DirectCast(sender, CheckBox).Parent, DataControlFieldCell).Parent, GridViewRow)
    '        Dim id_pv As Integer = gvd_Reaseguro.DataKeys(gr.RowIndex)("id_pv")

    '        Dim ConsultaBD As ConsultaBD
    '        ConsultaBD = New ConsultaBD

    '        If sender.checked = True Then
    '            ConsultaBD.InsertaPolNoPago(id_pv, Master.cod_usuario)
    '        Else
    '            ConsultaBD.BorraPolNoPago(id_pv)
    '        End If

    '        dtBusqueda = New DataTable
    '        dtBusqueda = Session("dtGeneral")

    '        Dim myRow() As Data.DataRow
    '        myRow = dtBusqueda.Select("id_pv ='" & id_pv & "'")
    '        For Each row In myRow
    '            row("sn_NoPago") = IIf(sender.checked = True, 1, 0)
    '        Next

    '    Catch ex As Exception
    '        Mensaje("ORDEN DE PAGO-:", ex.Message)
    '        LogError("(chk_DescartaPago_CheckedChanged)" & ex.Message)
    '    End Try
    'End Sub

    Private Sub btn_Descartadas_Click(sender As Object, e As EventArgs) Handles btn_Descartadas.Click
        Try
            Dim ConsultaBD As ConsultaBD
            ConsultaBD = New ConsultaBD

            gvd_Descartadas.DataSource = ConsultaBD.ConsultaPolNoPago
            gvd_Descartadas.DataBind()

            If gvd_Descartadas.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Open PopUp", "OpenPopup('#DescartadasModal');", True)
            Else
                Mensaje("ORDEN DE PAGO", "La consulta no devolvió ningun resultado")
            End If

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(btn_Descartadas_Click)" & ex.Message)
        End Try
    End Sub

    Private Sub btn_QuitarNoPago_Click(sender As Object, e As EventArgs) Handles btn_QuitarNoPago.Click
        Try
            Dim strIdPv As String = ""
            Dim ConsultaBD As ConsultaBD
            ConsultaBD = New ConsultaBD

            For Each row In gvd_Descartadas.Rows
                Dim chk_SelPol As CheckBox = DirectCast(row.FindControl("chk_SelPol"), CheckBox)
                If chk_SelPol.Checked = True Then
                    Dim hid_idpv As HiddenField = DirectCast(row.FindControl("hid_idpv"), HiddenField)

                    strIdPv = strIdPv & IIf(Len(strIdPv) > 0, ",", "") & hid_idpv.Value
                End If
            Next

            If Len(strIdPv) > 0 Then
                ConsultaBD.BorraPolNoPago(strIdPv)
                gvd_Descartadas.DataSource = ConsultaBD.ConsultaPolNoPago
                gvd_Descartadas.DataBind()
            Else
                Mensaje("ORDEN DE PAGO", "No se ha seleccionado ningún elemento")
            End If

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:", ex.Message)
            LogError("(btn_QuitarNoPago_Click)" & ex.Message)
        End Try
    End Sub

    Private Sub gvd_GrupoPolizas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvd_GrupoPolizas.PageIndexChanging
        Try
            Dim myRow() As Data.DataRow

            dtPolizas = New DataTable
            dtPolizas = Session("dtPolizas")
            For Each Row In gvd_GrupoPolizas.Rows
                Dim chk_SelPol As CheckBox = DirectCast(Row.FindControl("chk_SelPol"), CheckBox)
                Dim chk_NoPago As CheckBox = DirectCast(Row.FindControl("chk_NoPago"), CheckBox)
                Dim id_pv As Integer = gvd_GrupoPolizas.DataKeys(Row.RowIndex)("id_pv")

                myRow = dtPolizas.Select("id_pv ='" & id_pv & "'")
                myRow(0)("tSEL_Val") = chk_SelPol.Checked
                myRow(0)("sn_NoPago") = chk_NoPago.Checked
            Next

            gvd_GrupoPolizas.PageIndex = e.NewPageIndex
            gvd_GrupoPolizas.DataSource = dtPolizas
            gvd_GrupoPolizas.DataBind()
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:  ", ex.Message)
            LogError("(gvd_GrupoPolizas_PageIndexChanging)" & ex.Message)
        End Try
    End Sub
    Protected Sub chk_NoPago_CheckedChanged(sender As Object, e As EventArgs)
        Try
            Dim gr As GridViewRow = DirectCast(DirectCast(DirectCast(sender, CheckBox).Parent, DataControlFieldCell).Parent, GridViewRow)
            Dim id_pv As Integer = gvd_GrupoPolizas.DataKeys(gr.RowIndex)("id_pv")

            Dim ConsultaBD As ConsultaBD
            ConsultaBD = New ConsultaBD

            If sender.checked = True Then
                ConsultaBD.InsertaPolNoPago(id_pv, Master.cod_usuario)
            Else
                ConsultaBD.BorraPolNoPago(id_pv)
            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:  ", ex.Message)
            LogError("(chk_NoPago_CheckedChanged)" & ex.Message)
        End Try
    End Sub

    'Private Sub gvd_GrupoPolizas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvd_GrupoPolizas.RowCommand
    '        Try

    '            If e.CommandName.Equals("Aclaracion") Then
    '                Dim Index As Integer = e.CommandSource.NamingContainer.RowIndex
    '                Dim id_pv As Integer
    '                id_pv = gvd_GrupoPolizas.DataKeys(Index)("id_pv")
    '                ObtieneDatosRTF(id_pv)
    '            End If
    '        Catch ex As Exception
    '            Mensaje("ORDEN DE PAGO:", ex.Message)
    '        End Try
    'End Sub

End Class







