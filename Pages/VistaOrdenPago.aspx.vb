Imports System.Data
Imports System.Data.SqlClient
Partial Class Pages_VistaOrdenPago
    Inherits System.Web.UI.Page

    Private clCatalogo As Catalogo

    Private Enum Operacion
        Ninguna = 0
        Generar = 1
        Modificar = 2
        Anular = 3
        Consulta = 4
        Bloqueo = 5
    End Enum


    Private Enum enRecordsetEnum
        enGenerales
        enReaseguro
        enImputacion
        enCtasBancarias
        enDocumentos
    End Enum

    Private Resultados(5) As DataTable

    Private Function Mensaje(ByVal strSegmento As String, ByVal strMsg As String) As Boolean
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Mensaje", "EvaluaMensaje('" & strSegmento & "','" & Replace(Replace(strMsg, "'", ""), vbCrLf, " ") & "');", True)
        Return True
    End Function


    Private Sub Pages_VistaOrdenPago_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            chk_Todo.Checked = True
            chk_Todo.Enabled = False
            Session.Timeout = 60000

            Master.HostName = Session("HostName")
            Session.Add("blnExpiro", "1")
            Session.Add("dtDocumentos", Nothing)

            Master.Usuario = IIf(Request.QueryString("Usuario") = Nothing, "Inicia Sesión", Request.QueryString("Usuario"))
            Master.cod_usuario = IIf(Request.QueryString("cod_usuario") = Nothing, "", Request.QueryString("cod_usuario"))
            Master.cod_suc = IIf(Request.QueryString("cod_suc") = Nothing, 0, Request.QueryString("cod_suc"))
            Master.cod_sector = IIf(Request.QueryString("cod_sector") = Nothing, 0, Request.QueryString("cod_sector"))

            'Se obtiene el número de orden de Pago
            LlenaSucursal()
            LlenaTipo()
            LlenaOrigen()
            LlenaMoneda()
            LlenaAnulacion()

            txt_Orden.Text = IIf(Request.QueryString("nro_op") = Nothing, 0, Request.QueryString("nro_op"))

            Header.Title = "Orden Pago: " & txt_Orden.Text
            If txt_Orden.Text <> "0" Then
                If Consulta() Then
                    hid_Operacion.Value = Operacion.Consulta
                    LlenaControl()
                    EdoControl(Operacion.Consulta)
                Else
                    Mensaje("ORDEN DE PAGO", "La Órden de Pago no existe")
                End If
            Else
                hid_Operacion.Value = Operacion.Ninguna
                EdoControl(Operacion.Ninguna)
            End If
        Else
            If Session("blnExpiro") Is Nothing Then
                Mensaje("ORDEN DE PAGO", "La sesión ha expirado por inactividad")
                hid_Operacion.Value = Operacion.Bloqueo
                EdoControl(Operacion.Bloqueo)
            End If
        End If
    End Sub
    Private Sub EdoControl(ByVal intOperacion As Integer)

        'Si se intenta modifcar o anular se evalua el estatus
        If intOperacion = Operacion.Modificar Or intOperacion = Operacion.Anular Then
            'Se valida que no se encuentre autorizada
            If Val(txt_Estatus.Text) > 2 Then
                EdoControl(Operacion.Consulta)
                Mensaje("ORDEN DE PAGO", "El estatus de esta Órden de Pago es: " & txt_Estatus.Text & ", por lo tanto no puede ser modificada o anulada")
                Exit Sub
            End If

            If IsDate(txt_FecAnulacion.Text) Then
                EdoControl(Operacion.Consulta)
                Mensaje("ORDEN DE PAGO", "Esta Orden de Pago fue anulada con anterioridad, solo puede ser consultada")
                Exit Sub
            End If

            If IsDate(txt_FecContabilidad.Text) Then
                EdoControl(Operacion.Consulta)
                Mensaje("ORDEN DE PAGO", "Esta Orden de Pago ya fue autorizada por Contabilidad, solo puede ser consultada")
                Exit Sub
            End If

            If IsDate(txt_FecTesoreria.Text) Then
                Mensaje("ORDEN DE PAGO", "Esta Orden de Pago ya fue autorizada por Tesoreria, al modificarla perdera dicha autorización")
            End If

            Resultados(enRecordsetEnum.enReaseguro) = ConsultaImputacion(True)
        End If

        Dim nro_op As String = IIf(Request.QueryString("nro_op") = Nothing, 0, Request.QueryString("nro_op"))

        'If nro_op <> txt_Orden.Text Then
        '    chk_Todo.Enabled = False
        'Else
        '    chk_Todo.Enabled = True
        'End If

        Select Case intOperacion
            Case Operacion.Generar, Operacion.Modificar
                txt_Orden.Enabled = False
                txtFecha.Enabled = True
                ddl_Sucursal.Enabled = True
                txt_Cheque.Enabled = True
                ddl_SucursalPago.Enabled = True
                ddl_MonedaPago.Enabled = False
                opt_TipoPago.Enabled = True
                btn_CuentasBancarias.Enabled = True
                btn_Aceptar.Visible = True
                btn_Cancelar.Visible = True
                'btn_Generar.Visible = False
                btn_Modificar.Visible = False
                btn_Texto.Visible = True
                btn_Anular.Visible = False
                'btn_Consulta.Visible = False
                'btn_Imprimir.Visible = False
                gvd_Detalle.Enabled = True
                lbl_espacioBtn.Width = 62
                btn_AñadirCuota.Enabled = True
                btn_QuitarCuota.Enabled = True
                lbl_Anula.Visible = False
                ddl_ConceptoAnula.Visible = False
                btn_CuentasBancarias.Enabled = True
                chk_Todo.Enabled = False
                txt_TipoCambio.Text = String.Format("{0:#,#0.00}", ObtieneTipoCambio(Today.ToString("dd/MM/yyyy"), ddl_MonedaPago.SelectedValue))
                btn_Imprimir.Visible = True
                btn_Documentos.Visible = True

            Case Operacion.Anular, Operacion.Consulta
                txt_Orden.Enabled = False
                txtFecha.Enabled = False
                ddl_Sucursal.Enabled = False
                txt_Cheque.Enabled = False
                ddl_SucursalPago.Enabled = False
                ddl_MonedaPago.Enabled = False
                opt_TipoPago.Enabled = False
                btn_CuentasBancarias.Enabled = True
                btn_Aceptar.Visible = IIf(intOperacion = Operacion.Anular, True, False)
                btn_Cancelar.Visible = True
                'btn_Generar.Visible = False
                btn_Modificar.Visible = IIf(intOperacion = Operacion.Anular, False, True)
                btn_Texto.Visible = True
                btn_Anular.Visible = IIf(intOperacion = Operacion.Anular, False, True)
                'btn_Consulta.Visible = False
                'btn_Imprimir.Visible = IIf(intOperacion = Operacion.Consulta, True, False)
                gvd_Detalle.Enabled = False

                If intOperacion = Operacion.Consulta Then
                    lbl_espacioBtn.Width = 156
                Else
                    lbl_espacioBtn.Width = 62
                End If
                btn_AñadirCuota.Enabled = False
                btn_QuitarCuota.Enabled = False

                lbl_Anula.Visible = IIf(intOperacion = Operacion.Anular, True, False)
                ddl_ConceptoAnula.Visible = IIf(intOperacion = Operacion.Anular, True, False)

                btn_CuentasBancarias.Enabled = True

                btn_Imprimir.Visible = True
                btn_Documentos.Visible = True


            Case Operacion.Bloqueo
                txt_Orden.Enabled = False
                txtFecha.Enabled = False
                ddl_Sucursal.Enabled = False
                txt_Cheque.Enabled = False
                ddl_SucursalPago.Enabled = False
                ddl_MonedaPago.Enabled = False
                opt_TipoPago.Enabled = False
                btn_CuentasBancarias.Enabled = False
                btn_Aceptar.Visible = False
                btn_Cancelar.Visible = False
                btn_Cerrar.Visible = False
                btn_Modificar.Visible = False
                btn_Texto.Visible = False
                btn_Anular.Visible = False
                gvd_Detalle.Enabled = False
                lbl_espacioBtn.Width = 156
                btn_AñadirCuota.Enabled = False
                btn_QuitarCuota.Enabled = False
                lbl_Anula.Visible = False
                ddl_ConceptoAnula.Visible = False
                btn_CuentasBancarias.Enabled = False
                chk_Todo.Enabled = False
                btn_Imprimir.Enabled = False
                btn_Documentos.Enabled = False

            Case Else
                txt_Orden.Enabled = True
                txtFecha.Enabled = False
                ddl_Sucursal.Enabled = False
                txt_Cheque.Enabled = False
                ddl_SucursalPago.Enabled = False
                ddl_MonedaPago.Enabled = False
                opt_TipoPago.Enabled = False
                btn_CuentasBancarias.Enabled = False
                btn_Aceptar.Visible = False
                btn_Cancelar.Visible = False
                'btn_Generar.Visible = True
                btn_Modificar.Visible = True
                btn_Texto.Visible = False
                btn_Anular.Visible = True
                'btn_Consulta.Visible = True
                'btn_Imprimir.Visible = False
                gvd_Detalle.Enabled = False
                lbl_espacioBtn.Width = 250
                btn_AñadirCuota.Enabled = False
                btn_QuitarCuota.Enabled = False

                lbl_Anula.Visible = False
                ddl_ConceptoAnula.Visible = False

                btn_CuentasBancarias.Enabled = False

                chk_Todo.Enabled = False

                btn_Imprimir.Visible = False

                btn_Documentos.Visible = False

                txt_Orden.Focus()

        End Select

        ddl_Tipo.Enabled = False
        ddl_OrigenPago.Enabled = False
        txtClaveCia.Enabled = False
        txtSearchCia.Enabled = False



    End Sub

    Private Sub LlenaSucursal()
        clCatalogo = New Catalogo

        ddl_Sucursal.DataValueField = "Clave"
        ddl_Sucursal.DataTextField = "Descripcion"
        ddl_Sucursal.DataSource = clCatalogo.ObtieneCatalogo("Suc")
        ddl_Sucursal.DataBind()

        ddl_SucursalPago.DataValueField = "Clave"
        ddl_SucursalPago.DataTextField = "Descripcion"
        ddl_SucursalPago.DataSource = ddl_Sucursal.DataSource
        ddl_SucursalPago.DataBind()
    End Sub

    Private Sub LlenaTipo()
        clCatalogo = New Catalogo

        ddl_Tipo.DataValueField = "Clave"
        ddl_Tipo.DataTextField = "Descripcion"
        ddl_Tipo.DataSource = clCatalogo.ObtieneCatalogo("Abo")
        ddl_Tipo.DataBind()
    End Sub

    Private Sub LlenaOrigen()
        clCatalogo = New Catalogo

        ddl_OrigenPago.DataValueField = "Clave"
        ddl_OrigenPago.DataTextField = "Descripcion"
        ddl_OrigenPago.DataSource = clCatalogo.ObtieneCatalogo("Ori")
        ddl_OrigenPago.DataBind()
    End Sub

    Private Sub LlenaMoneda()
        clCatalogo = New Catalogo

        ddl_MonedaPago.DataValueField = "Clave"
        ddl_MonedaPago.DataTextField = "Descripcion"
        ddl_MonedaPago.DataSource = clCatalogo.ObtieneCatalogo("Mon")
        ddl_MonedaPago.DataBind()
    End Sub

    Private Sub LlenaAnulacion()
        clCatalogo = New Catalogo

        ddl_ConceptoAnula.DataValueField = "Clave"
        ddl_ConceptoAnula.DataTextField = "Descripcion"
        ddl_ConceptoAnula.DataSource = clCatalogo.ObtieneCatalogo("Anu")
        ddl_ConceptoAnula.DataBind()
    End Sub

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
    Private Function ConsultaImputacion(ByVal blnTodo As Boolean) As DataTable
        Dim sCnn As String
        Dim nro_op As String
        Dim id_pv As Double = 0
        Dim nro_layer As Integer = 0
        Dim id_contrato As String = "X"
        Dim nro_tramo As Integer = 0
        Dim cod_ramo_contable As Integer = 0
        Dim cod_cia As Integer = 0
        Dim nro_cuota As Integer = 0
        Dim dt As DataTable
        Dim da As SqlDataAdapter

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        nro_op = IIf(Request.QueryString("nro_op") = Nothing, 0, Request.QueryString("nro_op"))

        If blnTodo = False And nro_op = txt_Orden.Text Then
            id_pv = IIf(Request.QueryString("id_pv") = Nothing, 0, Request.QueryString("id_pv"))
            nro_layer = IIf(Request.QueryString("nro_layer") = Nothing, 0, Request.QueryString("nro_layer"))
            id_contrato = IIf(Request.QueryString("id_contrato") = Nothing, "X", Request.QueryString("id_contrato"))
            nro_tramo = IIf(Request.QueryString("nro_tramo") = Nothing, 0, Request.QueryString("nro_tramo"))
            cod_ramo_contable = IIf(Request.QueryString("cod_ramo_contable") = Nothing, 0, Request.QueryString("cod_ramo_contable"))
            cod_cia = IIf(Request.QueryString("cod_cia") = Nothing, 0, Request.QueryString("cod_cia"))
            nro_cuota = IIf(Request.QueryString("nro_cuota") = Nothing, 0, Request.QueryString("nro_cuota"))
        End If

        Dim sSel As String = "spS_DetalleImputacion " & txt_Orden.Text & "," & id_pv & "," & nro_layer & "," & id_contrato & "," & nro_tramo & "," & cod_ramo_contable & "," & cod_cia & "," & nro_cuota

        dt = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function

    Private Function Consulta() As Boolean
        Dim sCnn As String

        Consulta = False

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_OrdenPago " & txt_Orden.Text

        Dim da As SqlDataAdapter

        Resultados(enRecordsetEnum.enGenerales) = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(Resultados(enRecordsetEnum.enGenerales))

        If Resultados(enRecordsetEnum.enGenerales).Rows.Count > 0 Then

            Resultados(enRecordsetEnum.enReaseguro) = ConsultaImputacion(chk_Todo.Checked)

            sSel = "spS_CtasBancarias " & Resultados(enRecordsetEnum.enGenerales).Rows(0)("id_persona") & "," & Resultados(enRecordsetEnum.enGenerales).Rows(0)("cod_moneda")

            Resultados(enRecordsetEnum.enCtasBancarias) = New DataTable

            da = New SqlDataAdapter(sSel, sCnn)
            da.Fill(Resultados(enRecordsetEnum.enCtasBancarias))


            sSel = "spS_DocumentosXOP " & txt_Orden.Text

            Resultados(enRecordsetEnum.enDocumentos) = New DataTable

            da = New SqlDataAdapter(sSel, sCnn)
            da.Fill(Resultados(enRecordsetEnum.enDocumentos))

            Session("dtDocumentos") = Resultados(enRecordsetEnum.enDocumentos)

            Consulta = True
        End If

    End Function

    Private Function ObtieneCtaBanco() As String
        Dim nro_cta As String = ""
        Dim id_Cuenta As String = ""
        Dim cod_banco As String = ""
        Dim Swift As String = ""

        For Each row In gvd_CuentasBancarias.Rows
            Dim chk_SelCta As CheckBox = TryCast(row.FindControl("chk_SelCta"), CheckBox)
            If chk_SelCta.Checked = True Then
                nro_cta = gvd_CuentasBancarias.DataKeys(row.RowIndex)("txt_numero_cuenta")
                id_Cuenta = gvd_CuentasBancarias.DataKeys(row.RowIndex)("id_cuenta_bancaria")
                cod_banco = gvd_CuentasBancarias.DataKeys(row.RowIndex)("cod_banco")
                Swift = gvd_CuentasBancarias.DataKeys(row.RowIndex)("txt_swift")
                Exit For
            End If
        Next

        ObtieneCtaBanco = nro_cta & "|" & id_Cuenta & "|" & cod_banco & "|" & Swift
    End Function

    Private Function AnulaOrdenPago() As Boolean
        Dim sCnn As String
        Dim dtAnula As DataTable

        AnulaOrdenPago = False

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spD_OrdenPago " & txt_Orden.Text & "," & hid_IdImputacion.Value & "," & Master.cod_usuario & "," &
                                                ddl_Sucursal.SelectedValue & "," & ddl_ConceptoAnula.SelectedValue & "," &
                                                ddl_SucursalPago.SelectedValue & ",8"


        Dim da As SqlDataAdapter

        dtAnula = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtAnula)

        AnulaOrdenPago = True

        Return AnulaOrdenPago

    End Function


    Private Function Inserta() As Double
        Dim nro_cta As String = "0"
        Dim id_Cuenta As String = "0"
        Dim cod_banco As String = "0"
        Dim Swift As String = ""

        Dim indMonto As Integer = 0
        Dim indMontoReas As Integer = 0
        Dim strMontos(0) As String
        Dim strMontosReas(0) As String
        Dim pagina As Integer

        Dim no_correlativo As Integer = 0
        Dim Monto As Double = 0
        Dim Pje As Double = 0
        Dim blnMontos As Boolean = False
        Dim Resultado As String = ""
        Dim ArrConcepto() As String
        Dim Concepto As String

        Inserta = 0

        If opt_TipoPago.SelectedValue = -1 Then
            Dim ctaBancaria() As String = Split(ObtieneCtaBanco, "|")
            If UBound(ctaBancaria) > 0 Then
                nro_cta = ctaBancaria(0)
                id_Cuenta = ctaBancaria(1)
                cod_banco = ctaBancaria(2)
                Swift = ctaBancaria(3)
            Else
                Inserta = 0
                Mensaje("ORDEN DE PAGO", "Se debe seleccionar una Cuenta Bancaria")
                Exit Function
            End If
        End If

        EliminaImputacion(hid_IdImputacion.Value)

        indMonto = 0
        ReDim Preserve strMontos(indMonto)
        strMontos(indMonto) = ""
        indMontoReas = 0
        ReDim Preserve strMontosReas(indMontoReas)
        strMontosReas(indMontoReas) = ""

        For Each row In gvd_Detalle.Rows
            Dim txt_pje As TextBox = TryCast(row.FindControl("txt_pje"), TextBox)
            Dim txt_Importe As TextBox = TryCast(row.FindControl("txt_Importe"), TextBox)
            Dim txt_Concepto As TextBox = TryCast(row.FindControl("txt_Concepto"), TextBox)
            Dim hid_codConcepto As HiddenField = TryCast(row.FindControl("hid_codConcepto"), HiddenField)
            Dim hid_DescConcepto As HiddenField = TryCast(row.FindControl("hid_DescConcepto"), HiddenField)

            Monto = CDbl(Replace(txt_Importe.Text, ",", ""))
            Pje = CDbl(Replace(txt_pje.Text, ",", ""))

            With gvd_Detalle

                ArrConcepto = Split(txt_Concepto.Text, "|")
                If UBound(ArrConcepto) > 0 Then
                    Concepto = ArrConcepto(UBound(ArrConcepto))
                Else
                    Concepto = txt_Concepto.Text
                End If

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

                strMontos(indMonto) = strMontos(indMonto) & "(@strKey," & no_correlativo & ",NULL,''" & .DataKeys(row.RowIndex)("Cuenta") & "'',''" & .DataKeys(row.RowIndex)("cod_deb_cred") & "''," &
                                                             .DataKeys(row.RowIndex)("cod_moneda") & "," & Monto & "," & Monto * txt_TipoCambio.Text & "," &
                                                             IIf(.DataKeys(row.RowIndex)("cod_moneda") = 1, txt_TipoCambio.Text, 1) & ",''" & Concepto & "''),"

                strMontosReas(indMontoReas) = strMontosReas(indMontoReas) & "(@strKey," & no_correlativo & "," & hid_codConcepto.Value & "," & .DataKeys(row.RowIndex)("cod_broker") & "," & .DataKeys(row.RowIndex)("cod_cia") & "," &
                                                                            "11,1,''" & .DataKeys(row.RowIndex)("id_contrato") & "''," & .DataKeys(row.RowIndex)("nro_tramo") & "," & IIf(.DataKeys(row.RowIndex)("cod_deb_cred") = "D", -1, 1) * Monto & "," &
                                                                            .DataKeys(row.RowIndex)("cod_suc") & "," & .DataKeys(row.RowIndex)("ramo_pol") & "," & .DataKeys(row.RowIndex)("nro_pol") & "," & .DataKeys(row.RowIndex)("aaaa_endoso") & "," & .DataKeys(row.RowIndex)("nro_endoso") & "," &
                                                                            .DataKeys(row.RowIndex)("cod_suc") & ",1,NULL,NULL," & CStr(Now.ToString("yyyyMM")) & "," & .DataKeys(row.RowIndex)("cod_ramo_contable") & "," & Pje & "," & .DataKeys(row.RowIndex)("nro_cuota") & ",''" & .DataKeys(row.RowIndex)("fecha_fac") & "''," &
                                                                            "0,0,0,0," & .DataKeys(row.RowIndex)("Capa") & "),"

                no_correlativo = no_correlativo + 1
            End With
        Next

        '8 es Modulo Reaseguro
        Dim strKey = hid_IdImputacion.Value & ",8"


        For pagina = 0 To UBound(strMontos)
            If Len(strMontos(pagina)) > 0 Then
                blnMontos = True

                strMontos(pagina) = Mid(strMontos(pagina), 1, Len(strMontos(pagina)) - 1)

                Resultado = GuardaDatos("tmp_imputacion_general", strKey, strMontos(pagina))

                If Resultado <> "1" Then
                    Mensaje("ORDEN DE PAGO-: GUARDA CUOTAS", Resultado)
                    Exit Function
                End If
            End If
        Next


        For pagina = 0 To UBound(strMontosReas)
            If Len(strMontosReas(pagina)) > 0 Then
                blnMontos = True

                strMontosReas(pagina) = Mid(strMontosReas(pagina), 1, Len(strMontosReas(pagina)) - 1)

                Resultado = GuardaDatos("tmp_imputacion_reas", strKey, strMontosReas(pagina))

                If Resultado <> "1" Then
                    Mensaje("ORDEN DE PAGO-: GUARDA CUOTAS", Resultado)
                    Exit Function
                End If
            End If
        Next


        Inserta = InsertaOrdenPago(txt_Orden.Text, hid_IdImputacion.Value, Replace(txt_MontoTotal.Text, ",", ""), ddl_MonedaPago.SelectedValue, ObtieneTipoCambio(Today.ToString("dd/MM/yyyy"), ddl_MonedaPago.SelectedValue),
                                   txtClaveCia.Text, txtSearchCia.Text, txt_Cheque.Text, ddl_Sucursal.SelectedValue, hid_IdPersona.Value,
                                   hid_NroNit.Value, nro_cta, Swift, id_Cuenta, cod_banco, FechaAIngles(txtFecha.Text), opt_TipoPago.SelectedValue, ddl_SucursalPago.SelectedValue)

    End Function

    Private Function InsertaOrdenPago(ByVal nro_op As Integer, ByVal id_imputacion As Double, ByVal MontoTotal As Double, ByVal Moneda As Integer,
                                      ByVal impCambio As Double, ByVal cod_broker As Integer, ByVal Broker As String, ByVal Cheque As String, ByVal cod_suc As Integer,
                                      ByVal id_persona As Integer, ByVal nro_nit As String, ByVal Cuenta As String, ByVal Swift As String, ByVal id_Cuenta As Integer,
                                      ByVal cod_banco As Integer, ByVal FechaPago As String, ByVal sn_transferencia As Integer, ByVal cod_sucPago As Integer) As Double
        Dim sCnn As String = ""
        Dim sSel As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        ' 8 es modulo reaseguro
        ' 13 es Reaseguradora
        sSel = " DECLARE @nro_op int  " &
               "  EXEC spI_OrdenPago -1,2," & nro_op & "," & cod_suc & "," & Master.cod_sector & ",13,NULL,NULL,NULL,NULL," & cod_broker & ",'" & Broker & " " & id_persona & " " & nro_nit & "','" & Cheque & "'," &
                                      Moneda & "," & IIf(Moneda = 1, impCambio, 0) & "," & MontoTotal & "," & "'" & FechaPago & "',NULL,NULL,NULL,NULL,NULL," & id_imputacion & ",NULL,'" & Master.cod_usuario & "',NULL,''," &
                                      "@nro_op_out = @nro_op output,@cod_origen_pago=8, @id_persona = " & id_persona & ", @id_cuenta_bancaria = " & id_Cuenta & "," &
                                      "@sn_transferencia = " & sn_transferencia & ", @cod_suc_pago=" & cod_sucPago & ", @nro_cuenta_transferencia='" & Cuenta & "',@cod_banco_transferencia=" & cod_banco


        Dim da As SqlDataAdapter

        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        sSel = "EXEC spu_mov_autoriz " & id_imputacion & ", null, null, null, null, null," & dtRes.Rows(0)(0)
        Dim dtmov As DataTable
        dtmov = New DataTable
        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtmov)

        Return dtRes.Rows(0)(0)
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

    Private Sub LlenaControl()
        With Resultados(enRecordsetEnum.enGenerales)
            hid_IdImputacion.Value = .Rows(0)("id_imputacion")
            txtFecha.Text = String.Format("{0:dd/MM/yyyy}", .Rows(0)("fec_estim_pago"))
            ddl_Sucursal.SelectedValue = .Rows(0)("cod_suc")
            ddl_Tipo.SelectedValue = .Rows(0)("cod_abona")
            txtClaveCia.Text = IIf(IsDBNull(.Rows(0)("cod_abona_vrs")), "", .Rows(0)("cod_abona_vrs"))
            hid_IdPersona.Value = .Rows(0)("id_persona")
            hid_NroNit.Value = .Rows(0)("nro_nit")
            txtSearchCia.Text = .Rows(0)("txt_otros")
            ddl_OrigenPago.SelectedValue = .Rows(0)("cod_origen_pago")
            txt_Cheque.Text = .Rows(0)("txt_cheque_a_nom")
            ddl_MonedaPago.SelectedValue = .Rows(0)("cod_moneda")
            txt_Estatus.Text = .Rows(0)("cod_estatus_op") & "-" & .Rows(0)("estatus")
            txt_Texto.Text = .Rows(0)("Texto")
            opt_TipoPago.Items(0).Selected = IIf(.Rows(0)("sn_transferencia") = 0, True, False)
            opt_TipoPago.Items(1).Selected = IIf(.Rows(0)("sn_transferencia") = -1, True, False)
            'txt_TipoCambio.Text = String.Format("{0:#,#0.00}", ObtieneTipoCambio(Today.ToString("dd/MM/yyyy"), .Rows(0)("cod_moneda")))
            txt_TipoCambio.Text = String.Format("{0:#,#0.00}", .Rows(0)("imp_cambio"))
            txt_Solicita.Text = If(IsDBNull(.Rows(0)("Solicitante")), "", .Rows(0)("Solicitante"))
            txt_Tesoreria.Text = If(IsDBNull(.Rows(0)("Tesoreria")), "", .Rows(0)("Tesoreria"))
            txt_Contabilidad.Text = If(IsDBNull(.Rows(0)("Contabilidad")), "", .Rows(0)("Contabilidad"))
            txt_Anulacion.Text = If(IsDBNull(.Rows(0)("Baja")), "", .Rows(0)("Baja"))
            txt_FecGenera.text = IIf(IsDBNull(.Rows(0)("fec_generacion")), "", .Rows(0)("fec_generacion"))
            txt_FecTesoreria.Text = IIf(IsDBNull(.Rows(0)("fec_autoriz_sector")), "", .Rows(0)("fec_autoriz_sector"))
            txt_FecContabilidad.Text = IIf(IsDBNull(.Rows(0)("fec_autoriz_contab")), "", .Rows(0)("fec_autoriz_contab"))
            txt_FecAnulacion.Text = IIf(IsDBNull(.Rows(0)("fec_baja")), "", .Rows(0)("fec_baja"))
        End With

        gvd_Detalle.DataSource = Resultados(enRecordsetEnum.enReaseguro)
        gvd_Detalle.DataBind()

        gvd_CuentasBancarias.DataSource = Resultados(enRecordsetEnum.enCtasBancarias)
        gvd_CuentasBancarias.DataBind()

        gvd_Documentos.DataSource = Resultados(enRecordsetEnum.enDocumentos)
        gvd_Documentos.DataBind()

    End Sub


    Private Sub LimpiaControl()
        hid_IdImputacion.Value = 0
        txtFecha.Text = vbNullString
        ddl_Sucursal.SelectedIndex = 0
        ddl_Tipo.SelectedIndex = 0
        txtClaveCia.Text = vbNullString
        txtSearchCia.Text = vbNullString
        ddl_OrigenPago.SelectedIndex = 0
        txt_Cheque.Text = vbNullString
        ddl_MonedaPago.SelectedIndex = 0
        txt_TipoCambio.Text = "0.00"
        gvd_CuentasBancarias.DataSource = Nothing
        gvd_CuentasBancarias.DataBind()
        gvd_Detalle.DataSource = Nothing
        gvd_Detalle.DataBind()
        gvd_Documentos.DataSource = Nothing
        gvd_Documentos.DataBind()
        txt_Solicita.Text = vbNullString
        txt_Tesoreria.Text = vbNullString
        txt_FecGenera.Text = vbNullString
        txt_FecTesoreria.Text = vbNullString
        txt_Contabilidad.Text = vbNullString
        txt_Anulacion.Text = vbNullString
        txt_FecContabilidad.Text = vbNullString
        txt_FecAnulacion.Text = vbNullString
        txt_Texto.Text = vbNullString
    End Sub

    Private Function ValidaCampoRequerido() As Boolean
        ValidaCampoRequerido = False

        If txt_Orden.Text = vbNullString Then
            Mensaje("ORDEN DE PAGO", "Se debe especificar la Órden de Pago")
            txt_Orden.Focus()
            Exit Function
        End If
        ValidaCampoRequerido = True
    End Function

    Private Sub btn_Modificar_Click(sender As Object, e As EventArgs) Handles btn_Modificar.Click
        Try
            If ValidaCampoRequerido() Then
                If Consulta() Then
                    hid_Operacion.Value = Operacion.Modificar
                    LlenaControl()
                    EdoControl(Operacion.Modificar)
                Else
                    Mensaje("ORDEN DE PAGO", "La Órden de Pago no existe")
                End If
            End If

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:Modificar", ex.Message)
        End Try
    End Sub

    Private Sub btn_Anular_Click(sender As Object, e As EventArgs) Handles btn_Anular.Click
        Try
            If ValidaCampoRequerido() Then
                If Consulta() Then
                    hid_Operacion.Value = Operacion.Anular
                    LlenaControl()
                    EdoControl(Operacion.Anular)
                Else
                    Mensaje("ORDEN DE PAGO", "La Órden de Pago no existe")
                End If
            End If
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:Anular", ex.Message)
        End Try
    End Sub

    'Private Sub btn_Consulta_Click(sender As Object, e As EventArgs) Handles btn_Consulta.Click
    '    Try
    '        If ValidaCampoRequerido() Then
    '            If Consulta() Then
    '                hid_Operacion.Value = Operacion.Consulta
    '                LlenaControl()
    '                EdoControl(Operacion.Consulta)
    '            Else
    '                Mensaje("ORDEN DE PAGO-:Consulta", "La Órden de Pago no existe")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Mensaje("ORDEN DE PAGO-:Consulta", ex.Message)
    '    End Try
    'End Sub

    Private Sub btn_Aceptar_Click(sender As Object, e As EventArgs) Handles btn_Aceptar.Click
        Try
            Dim ConsultaBD As ConsultaBD
            ConsultaBD = New ConsultaBD
            Select Case hid_Operacion.Value
                Case Operacion.Modificar
                    If Val(txt_TipoCambio.Text) = 0 Then
                        Mensaje("ORDEN DE PAGO", "No se ha capturado el tipo de cambio del día, imposible guardar la Órden de Pago")
                        Exit Sub
                    End If
                    If Inserta() > 0 Then
                        GuardaTexto(txt_Orden.Text, txt_Texto.Text)
                        Mensaje("ORDEN DE PAGO", "La Órden de Pago " & txt_Orden.Text & " se actualizó satisfactoriamente")
                        ConsultaBD.InsertaBitacora(Master.cod_usuario, Master.HostName, "Actualización", "Orden de Pago: " & txt_Orden.Text)
                    Else
                        Mensaje("ORDEN DE PAGO", "La Órden de Pago " & txt_Orden.Text & " no fue actualizada")
                    End If

                Case Operacion.Anular
                    If AnulaOrdenPago() Then
                        Mensaje("ORDEN DE PAGO", "La Órden de Pago " & txt_Orden.Text & " ha sido anulada satisfactoriamente")
                        ConsultaBD.InsertaBitacora(Master.cod_usuario, Master.HostName, "Anulación", "Orden de Pago: " & txt_Orden.Text)
                    Else
                        Mensaje("ORDEN DE PAGO", "La Órden de Pago " & txt_Orden.Text & " no ha sido anulada")
                    End If
            End Select

            hid_Operacion.Value = Operacion.Ninguna
            EdoControl(Operacion.Ninguna)
            LimpiaControl()
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:Aceptar", ex.Message)
        End Try
    End Sub

    Private Sub btn_Cancelar_Click(sender As Object, e As EventArgs) Handles btn_Cancelar.Click
        Try
            hid_Operacion.Value = Operacion.Ninguna
            EdoControl(Operacion.Ninguna)
            LimpiaControl()
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:Cancelar", ex.Message)
        End Try
    End Sub

    Private Sub btn_Cerrar_Click(sender As Object, e As EventArgs) Handles btn_Cerrar.Click
        Try
            hid_Operacion.Value = Operacion.Ninguna
            EdoControl(Operacion.Ninguna)
            LimpiaControl()
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:Cerrar", ex.Message)
        End Try
    End Sub

    Private Sub btn_QuitarCuota_Click(sender As Object, e As EventArgs) Handles btn_QuitarCuota.Click
        Try
            Dim dtDetalle As DataTable
            dtDetalle = New DataTable

            dtDetalle.Columns.Add("id_pv")
            dtDetalle.Columns.Add("nro_reas")
            dtDetalle.Columns.Add("id_imputacion")
            dtDetalle.Columns.Add("cod_modulo")
            dtDetalle.Columns.Add("id_contrato")
            dtDetalle.Columns.Add("nro_tramo")
            dtDetalle.Columns.Add("cod_ramo_contable")
            dtDetalle.Columns.Add("RamoContable")
            dtDetalle.Columns.Add("Capa")
            dtDetalle.Columns.Add("cod_broker")
            dtDetalle.Columns.Add("Broker")
            dtDetalle.Columns.Add("cod_cia")
            dtDetalle.Columns.Add("Compañia")
            dtDetalle.Columns.Add("cod_cpto")
            dtDetalle.Columns.Add("desc_concepto")
            dtDetalle.Columns.Add("Concepto")
            dtDetalle.Columns.Add("Poliza")
            dtDetalle.Columns.Add("nro_cuota")
            dtDetalle.Columns.Add("pje_fac")
            dtDetalle.Columns.Add("imp_mo")
            dtDetalle.Columns.Add("fecha_fac")
            dtDetalle.Columns.Add("cod_moneda")
            dtDetalle.Columns.Add("imp_cambio")
            dtDetalle.Columns.Add("cod_deb_cred")
            dtDetalle.Columns.Add("Cuenta")
            dtDetalle.Columns.Add("cod_suc")
            dtDetalle.Columns.Add("ramo_pol")
            dtDetalle.Columns.Add("nro_pol")
            dtDetalle.Columns.Add("aaaa_endoso")
            dtDetalle.Columns.Add("nro_endoso")
            dtDetalle.Columns.Add("cod_profit_center")
            dtDetalle.Columns.Add("cod_subprofit_center")
            dtDetalle.Columns.Add("aaaamm_movimiento")
            dtDetalle.Columns.Add("cod_major")
            dtDetalle.Columns.Add("cod_minor")
            dtDetalle.Columns.Add("cod_class_peril")
            dtDetalle.Columns.Add("sn_ogis")
            dtDetalle.Columns.Add("aaaa_ejercicio_stro")
            dtDetalle.Columns.Add("nro_stro")
            dtDetalle.Columns.Add("prima_cedida")
            dtDetalle.Columns.Add("comisiones")

            'Recorre el Grid
            For Each row In gvd_Detalle.Rows

                Dim chk_Seleccion As CheckBox = TryCast(row.FindControl("chk_Seleccion"), CheckBox)

                If chk_Seleccion.Checked = False Then

                    Dim txt_pje As TextBox = TryCast(row.FindControl("txt_pje"), TextBox)
                    Dim txt_Importe As TextBox = TryCast(row.FindControl("txt_Importe"), TextBox)
                    Dim txt_Concepto As TextBox = TryCast(row.FindControl("txt_Concepto"), TextBox)
                    Dim hid_codConcepto As HiddenField = TryCast(row.FindControl("hid_codConcepto"), HiddenField)
                    Dim hid_DescConcepto As HiddenField = TryCast(row.FindControl("hid_DescConcepto"), HiddenField)



                    With gvd_Detalle
                        dtDetalle.Rows.Add(.DataKeys(row.RowIndex)("id_pv"), .DataKeys(row.RowIndex)("nro_reas"), .DataKeys(row.RowIndex)("id_imputacion"),
                                           .DataKeys(row.RowIndex)("cod_modulo"), .DataKeys(row.RowIndex)("id_contrato"), .DataKeys(row.RowIndex)("nro_tramo"),
                                           .DataKeys(row.RowIndex)("cod_ramo_contable"), .DataKeys(row.RowIndex)("RamoContable"),
                                           .DataKeys(row.RowIndex)("Capa"), .DataKeys(row.RowIndex)("cod_broker"), .DataKeys(row.RowIndex)("Broker"),
                                           .DataKeys(row.RowIndex)("cod_cia"), .DataKeys(row.RowIndex)("Compañia"),
                                           hid_codConcepto.Value, hid_DescConcepto.Value, txt_Concepto.Text,
                                           .DataKeys(row.RowIndex)("Poliza"), .DataKeys(row.RowIndex)("nro_cuota"), Replace(txt_pje.Text, ",", ""),
                                           Replace(txt_Importe.Text, ",", ""), .DataKeys(row.RowIndex)("fecha_fac"), .DataKeys(row.RowIndex)("cod_moneda"),
                                           .DataKeys(row.RowIndex)("imp_cambio"), .DataKeys(row.RowIndex)("cod_deb_cred"), .DataKeys(row.RowIndex)("Cuenta"),
                                           .DataKeys(row.RowIndex)("cod_suc"), .DataKeys(row.RowIndex)("ramo_pol"), .DataKeys(row.RowIndex)("nro_pol"),
                                           .DataKeys(row.RowIndex)("aaaa_endoso"), .DataKeys(row.RowIndex)("nro_endoso"), .DataKeys(row.RowIndex)("cod_profit_center"),
                                           .DataKeys(row.RowIndex)("cod_subprofit_center"), .DataKeys(row.RowIndex)("aaaamm_movimiento"), .DataKeys(row.RowIndex)("cod_major"),
                                           .DataKeys(row.RowIndex)("cod_minor"), .DataKeys(row.RowIndex)("cod_class_peril"), .DataKeys(row.RowIndex)("sn_ogis"),
                                           .DataKeys(row.RowIndex)("aaaa_ejercicio_stro"), .DataKeys(row.RowIndex)("nro_stro"), .DataKeys(row.RowIndex)("prima_cedida"),
                                           .DataKeys(row.RowIndex)("comisiones"))
                    End With
                End If
            Next
            gvd_Detalle.DataSource = dtDetalle
            gvd_Detalle.DataBind()

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:Quita Cuota", ex.Message)
        End Try
    End Sub

    Private Function ObtieneTipoCambio(ByVal Fecha As String, ByVal cod_moneda As Integer) As Double
        Dim sCnn As String = ""
        Dim sSel As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "spS_TipoCambio '" & Fecha & "'," & cod_moneda

        Dim da As SqlDataAdapter

        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        Return dtRes.Rows(0)(0)
    End Function

    Private Sub chk_Todo_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Todo.CheckedChanged
        Try
            gvd_Detalle.DataSource = ConsultaImputacion(chk_Todo.Checked)
            gvd_Detalle.DataBind()
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:MOSTRAR TODO", ex.Message)
        End Try
    End Sub

    Private Sub btn_Añadir_Click(sender As Object, e As EventArgs) Handles btn_Añadir.Click
        Try
            Dim dtDocumentos As DataTable
            Dim cod_version As Integer = 0
            Dim txt_Descripcion As TextBox

            dtDocumentos = Session("dtDocumentos")

            If gvd_Documentos.Rows.Count > 0 Then
                cod_version = CType(gvd_Documentos.Rows(gvd_Documentos.Rows.Count - 1).FindControl("lbl_Version"), Label).Text + 1
            End If

            dtDocumentos.Rows.Clear()

            For Each Row In gvd_Documentos.Rows
                Dim chk_SelDoc As CheckBox = TryCast(Row.FindControl("chk_SelDoc"), CheckBox)
                Dim lbl_Version As Label = TryCast(Row.FindControl("lbl_Version"), Label)
                txt_Descripcion = TryCast(Row.FindControl("txt_Descripcion"), TextBox)
                Dim txt_FechaSolicitud As TextBox = TryCast(Row.FindControl("txt_FechaSolicitud"), TextBox)
                Dim txt_FechaEntrega As TextBox = TryCast(Row.FindControl("txt_FechaEntrega"), TextBox)

                If chk_SelDoc.Checked = False Then
                    dtDocumentos.Rows.Add(txt_Orden.Text, lbl_Version.Text, txt_Descripcion.Text, txt_FechaSolicitud.Text, txt_FechaEntrega.Text)
                End If
            Next

            dtDocumentos.Rows.Add(txt_Orden.Text, cod_version, "", String.Format("{0:dd/MM/yyyy}", Now), "")

            Session("dtDocumentos") = dtDocumentos

            gvd_Documentos.DataSource = dtDocumentos
            gvd_Documentos.DataBind()

            txt_Descripcion = CType(gvd_Documentos.Rows(gvd_Documentos.Rows.Count - 1).FindControl("txt_Descripcion"), TextBox)
            txt_Descripcion.Focus()

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:AÑADIR DOCUMENTO", ex.Message)
        End Try
    End Sub

    Private Sub btn_Quitar_Click(sender As Object, e As EventArgs) Handles btn_Quitar.Click
        Try
            Dim dtDocumentos As DataTable

            dtDocumentos = Session("dtDocumentos")

            dtDocumentos.Rows.Clear()

            For Each Row In gvd_Documentos.Rows
                Dim chk_SelDoc As CheckBox = TryCast(Row.FindControl("chk_SelDoc"), CheckBox)
                Dim lbl_Version As Label = TryCast(Row.FindControl("lbl_Version"), Label)
                Dim txt_Descripcion As TextBox = TryCast(Row.FindControl("txt_Descripcion"), TextBox)
                Dim txt_FechaSolicitud As TextBox = TryCast(Row.FindControl("txt_FechaSolicitud"), TextBox)
                Dim txt_FechaEntrega As TextBox = TryCast(Row.FindControl("txt_FechaEntrega"), TextBox)

                If chk_SelDoc.Checked = False Then
                    dtDocumentos.Rows.Add(txt_Orden.Text, lbl_Version.Text, txt_Descripcion.Text, txt_FechaSolicitud.Text, txt_FechaEntrega.Text)
                End If
            Next

            Session("dtDocumentos") = dtDocumentos

            gvd_Documentos.DataSource = dtDocumentos
            gvd_Documentos.DataBind()

        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:QUITAR DOCUMENTO", ex.Message)
        End Try
    End Sub

    Private Sub btn_OkDocumentos_Click(sender As Object, e As EventArgs) Handles btn_OkDocumentos.Click
        Try
            Dim strDoctos As String = vbNullString
            Dim Resultado As String
            Dim strKey As String

            For Each Row In gvd_Documentos.Rows
                Dim lbl_Version As Label = TryCast(Row.FindControl("lbl_Version"), Label)
                Dim txt_Descripcion As TextBox = TryCast(Row.FindControl("txt_Descripcion"), TextBox)
                Dim txt_FechaSolicitud As TextBox = TryCast(Row.FindControl("txt_FechaSolicitud"), TextBox)
                Dim txt_FechaEntrega As TextBox = TryCast(Row.FindControl("txt_FechaEntrega"), TextBox)

                If txt_Descripcion.Text = vbNullString Then
                    Mensaje("ORDEN DE PAGO-: DOCUMENTOS", "Se debe espcificar una descripción")
                    Exit Sub
                ElseIf Not IsDate(txt_FechaSolicitud.text) Then
                    Mensaje("ORDEN DE PAGO-: DOCUMENTOS", "La fecha de Solicitud no es válida")
                    Exit Sub
                ElseIf txt_FechaEntrega.Text <> vbNullString And Not IsDate(txt_FechaSolicitud.text) Then
                    Mensaje("ORDEN DE PAGO-: DOCUMENTOS", "La Fecha de Entrega no es válida ")
                    Exit Sub
                End If

                EliminaDocumentos(txt_Orden.Text)

                strDoctos = strDoctos & "(@strKey," & lbl_Version.Text & ",''" & txt_Descripcion.Text & "'',''" & txt_FechaSolicitud.Text & "''," & IIf(txt_FechaEntrega.Text = vbNullString, "NULL", "''" & txt_FechaEntrega.Text & "''") & "),"
            Next

            If Len(strDoctos) > 0 Then
                strKey = txt_Orden.Text
                strDoctos = Mid(strDoctos, 1, Len(strDoctos) - 1)
                Resultado = GuardaDatos("rDocumentosXOP", strKey, strDoctos)

                If Resultado <> "1" Then
                    Mensaje("ORDEN DE PAGO-: DOCUMENTOS", Resultado)
                    Exit Sub
                End If
            End If


            Mensaje("ORDEN DE PAGO-: DOCUMENTOS", "Los cambios fueron guardados")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Close Modal", "ClosePopup('#DocumentosModal');", True)
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:GUARDAR", ex.Message)
        End Try
    End Sub

    Private Function EliminaDocumentos(ByVal nro_orden As Integer) As Boolean
        Dim sCnn As String = ""
        Dim sSel As String

        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        sSel = "DELETE FROM rDocumentosXOP WHERE nro_op = " & nro_orden

        Dim da As SqlDataAdapter

        Dim dtRes As DataTable
        dtRes = New DataTable

        da = New SqlDataAdapter(sSel, sCnn)

        da.Fill(dtRes)

        Return True
    End Function

    Private Function FechaAIngles(ByVal Fecha As String) As String
        Return String.Format("{0:MM/dd/yyyy}", CDate(Fecha))
    End Function

    Private Sub btn_Imprimir_Click(sender As Object, e As EventArgs) Handles btn_Imprimir.Click
        Try
            Dim server As String = "http://siigmxapp02/ReportServer_SIIGMX02?%2fReportesGMX%2fOrdenPago&rs%3AFormat=PDF&rc:Parameters=false&nro_op=@nro_op"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "ImprimirOrden", "ImprimirOrden('" & server & "','" & txt_Orden.Text & "');", True)
        Catch ex As Exception
            Mensaje("ORDEN DE PAGO-:IMPRIMIR", ex.Message)
        End Try
    End Sub



    'Private Sub gvd_CuentasBancarias_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvd_CuentasBancarias.RowDataBound
    '    Try

    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '            Dim chk_SelCta As CheckBox = CType(e.Row.FindControl("chk_SelCta"), CheckBox)
    '            chk_SelCta.Attributes.Add("OnClick", "CambioSeleccion('" & chk_SelCta.ClientID & "','Unica')")
    '        End If
    '    Catch ex As Exception
    '        Mensaje("ORDEN DE PAGO-:ROWDATABOUND", ex.Message)
    '    End Try
    'End Sub
End Class
