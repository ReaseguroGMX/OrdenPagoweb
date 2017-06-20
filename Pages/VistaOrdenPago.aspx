<%@ Page Title="" Language="VB" MasterPageFile="~/Pages/SiteMaster.master" AutoEventWireup="false" CodeFile="VistaOrdenPago.aspx.vb" Inherits="Pages_VistaOrdenPago" %>
<%@ MasterType VirtualPath="~/Pages/SiteMaster.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentMaster" Runat="Server">
     <script src="../Scripts/VistaOrdenPago.js"></script>
     <script src="../Scripts/jquery.numeric.js"></script>
     <script src="../Scripts/jquery.maskedinput.js"></script>
     <script type="text/javascript"> 
         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageLoadOrden);
    </script> 

    <div style="width:1200px; min-width:1200px; overflow-y:scroll;">
            <asp:UpdatePanel runat="server" ID="upOrdenPago">
                <ContentTemplate>

                    <div class="modal-body" style="height:480px">

                                <table style="width:1080px; min-width:1080px; border:0px; align:center">
                                <tr>
                                    <td>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="140px">Número Órden</asp:label>
                                                        <asp:TextBox runat="server" ID="txt_Orden" Width="175px" Height="22px"  CssClass="form-control LetraDetalle Derecha" ></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hid_IdImputacion" value="0" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="130px">Fecha de Pago</asp:label>
                                                        <asp:TextBox runat="server" ID="txtFecha" CssClass="form-control LetraDetalle Fecha Centro" Width="170px" Height="22px" ></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="120px">Sucursal</asp:label>
                                                        <asp:DropDownList runat="server" ID="ddl_Sucursal" CssClass="form-control LetraDetalle" Width="215px" Height="22px"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="140px">Tipo</asp:label>
                                                        <asp:DropDownList runat="server" ID="ddl_Tipo" CssClass="form-control LetraDetalle" Width="175px" Height="22px"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="130px">Compañia</asp:label>
                                                        <asp:TextBox runat="server" ID="txtClaveCia" CssClass="form-control LetraDetalle Centro"  Width="60px" Height="22px" ></asp:TextBox>
                                                        <button type="button" id="btn_SelCia" class="btn btn-info" data-toggle="modal" data-target="#CatalogoModal" style="Width:36px; Height:22px;" >...</button>
                                                        <asp:TextBox runat="server" ID="txtSearchCia" CssClass="form-control LetraDetalle" Width="480px" Height="22px" ></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hid_IdPersona" value="0" />
                                                        <asp:HiddenField runat="server" ID="hid_NroNit" value="0" />
                                                    </div>
                                                </div>  
                                            </div>
                                        </div>

                                        <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <div class="input-group">
                                                            <asp:label runat="server" class="col-md-1 control-label" Width="140px">Nombre Cheque</asp:label>
                                                            <asp:TextBox runat="server" ID="txt_Cheque" CssClass="form-control LetraDetalle" Width="935px" Height="22px" ></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="140px">Origen Pago</asp:label>
                                                        <asp:DropDownList runat="server" ID="ddl_OrigenPago" CssClass="form-control LetraDetalle" Width="175px" Height="22px"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="130px">Sucursal Pago</asp:label>
                                                        <asp:DropDownList runat="server" ID="ddl_SucursalPago" CssClass="form-control LetraDetalle" Width="195px" Height="22px"></asp:DropDownList>
                                                    </div>
                                                </div>  
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="120px">Moneda Pago</asp:label>
                                                        <asp:DropDownList runat="server" ID="ddl_MonedaPago" CssClass="form-control LetraDetalle" Width="215px" Height="22px"></asp:DropDownList>
                                                    </div>
                                                </div>  
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="140px">Tipo Pago</asp:label>
                                                        <asp:RadioButtonList runat="server" ID="opt_TipoPago" >
                                                            <asp:ListItem Value ="0" Selected="True" >Cheque</asp:ListItem>
                                                            <asp:ListItem Value="-1" >Transferencia</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="col-md-4" >
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="130px">Tipo Cambio</asp:label>
                                                        <asp:TextBox runat="server" ID="txt_TipoCambio" CssClass="form-control LetraDetalle" Width="45px" Height="22px" Enabled="false" ></asp:TextBox>
                                                        <label style="width:6px;" ></label>
                                                        <asp:Button runat="server" ID="btn_CuentasBancarias" data-toggle="modal" data-target="#CuentasModal" Text="Cuentas Bancarias" Font-Size="9px" Font-Bold="true" Height="22px" CssClass="btn btn-info" Width="140px" />
                                                    </div>
                   
                                                    <div class="input-group">
                                                        <asp:label ID="lbl_Anula" runat="server" class="col-md-1 control-label" Width="130px" Visible="false">Cpto. Anulación</asp:label>
                                                        <asp:DropDownList runat="server" ID="ddl_ConceptoAnula" Visible="false" CssClass="form-control LetraDetalle" Width="195px" Height="22px"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:label runat="server" class="col-md-1 control-label" Width="120px">Estatus</asp:label>
                                                        <asp:TextBox runat="server" ID="txt_Estatus" Enabled="false"  CssClass="form-control LetraDetalle" Width="215px" Height="22px" ></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="input-group">
                                                    <asp:label  runat="server" class="col-md-1 control-label" Width="120px" >Monto a Pagar</asp:label>
                                                    <asp:textbox runat="server" ID="txt_MontoTotal" Text="0.00" CssClass="form-control LetraDetalle Derecha" Width="215px" Height="22px" Enabled="false"></asp:textbox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clear padding5"></div>
                                        <div  style="width:100%; height:130px; overflow-y: scroll;  overflow-x: hidden; ">
                                            <asp:GridView runat="server" ID="gvd_Detalle" GridLines="Both" AutoGenerateColumns="false"  
                                                          DataKeyNames="id_pv,nro_reas,id_imputacion,cod_modulo,id_contrato,nro_tramo,cod_ramo_contable,RamoContable,
                                                                        Capa,cod_broker,Broker,cod_cia,Compañia,cod_cpto,desc_concepto,Concepto,
                                                                        Poliza,nro_cuota,pje_fac,imp_mo,fecha_fac,cod_moneda,imp_cambio,cod_deb_cred,Cuenta,
                                                                        cod_suc,ramo_pol,nro_pol,aaaa_endoso,nro_endoso,cod_profit_center,cod_subprofit_center,
                                                                        aaaamm_movimiento,cod_major,cod_minor,cod_class_peril,sn_ogis,aaaa_ejercicio_stro,nro_stro,
                                                                        prima_cedida,comisiones" 
                                                          CssClass="LetraDetalleGrid" >
                                                <Columns>
                                                     
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                             <asp:CheckBox runat="server" ID="chk_Seleccion" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Póliza">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl_Poliza" Text='<%# Eval("Poliza") %>' Width="100px"   ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Contrato">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl_idContrato" Text='<%# Eval("id_contrato") %>' Width="60px"   ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Ramo Contable">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl_RamoContable" Text='<%# Eval("RamoContable") %>' Width="100px"    ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Capa">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl_Capa" Text='<%# Eval("Capa") %>' Width="25px"   ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Broker">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl_Broker" Text='<%# Eval("Broker") %>' Width="250px"   ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Compañia">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl_Compañia" Text='<%# Eval("Compañia") %>' Width="230px"    ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Cuota">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl_nroCuota" Text='<%# Eval("nro_cuota")  %>' Width="40px"    ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Descripción">
                                                        <ItemTemplate>
                                                            <asp:textbox class="form-control Descripcion" runat="server" ID="txt_Concepto"  Text='<%# Eval("Concepto") %>' Width="100px" ></asp:textbox>
                                                            <asp:HiddenField runat="server" ID="hid_codConcepto" Value='<%# Eval("cod_cpto") %>' />
                                                            <asp:HiddenField runat="server" ID="hid_DescConcepto" Value='<%# Eval("desc_concepto") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="% Fac" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:textbox runat="server" ID="txt_pje"  Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("pje_fac")))  %>' Width="50px" CssClass="form-control Prc"></asp:textbox>
                                                            <asp:textbox runat="server" ID="hid_debcred" Text='<%#  Eval("cod_deb_cred")  %>'  CssClass="NoDisplay cod_deb_cred" ></asp:textbox>
                                                            <asp:textbox runat="server" ID="hid_primacedida" Text='<%# Eval("prima_cedida")  %>'  CssClass="NoDisplay prima_cedida" ></asp:textbox>
                                                            <asp:textbox runat="server" ID="hid_comisiones" Text='<%#  Eval("comisiones")  %>'  CssClass="NoDisplay comisiones" ></asp:textbox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Importe">
                                                        <ItemTemplate>
                                                            <asp:textbox runat="server" ID="txt_Importe" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_mo")))  %>' Width="70px" ForeColor="DarkBlue"   CssClass="form-control Monto"  ></asp:textbox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle CssClass="GridviewScrollHeader" /> 
                                                <EditRowStyle BackColor="#999999" />
                                            </asp:GridView> 
                                        </div>

                                        

                                        <div style="width:100%; padding-left:875px;">
                                            <div class="input-group">
                                                <asp:CheckBox runat="server" ID="chk_Todo" Text="Mostrar Todo"  CssClass="LetraDetalleGrid" AutoPostBack="true" />

                                                <asp:Button ID="btn_AñadirCuota" runat="server" CssClass="btn btn-success" Font-Size="11px"  Text="Añadir" Height="24px" />
                                                <asp:Button ID="btn_QuitarCuota" runat="server" CssClass="btn btn-danger" Font-Size="11px"  Text="Quitar" Height="24px" />
                                            </div>
                                        </div>

                                        <div class="clear padding10"></div>

                                        <div class="row">
                                             <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:label runat="server" class="col-md-1 control-label" Width="80px">Reaseguro</asp:label>
                                                            <asp:TextBox runat="server" ID="txt_Solicita" Enabled="false"  CssClass="form-control LetraDetalle" Width="160px" Height="20px" ></asp:TextBox>
                                                            <div class="clear padding3"></div>
                                                            <asp:label runat="server" class="col-md-1 control-label" Width="80px">Tesoreria</asp:label>
                                                            <asp:TextBox runat="server" ID="txt_Tesoreria" Enabled="false"  CssClass="form-control LetraDetalle" Width="160px" Height="20px" ></asp:TextBox>
                                                        </div>
                                             </div>

                                             <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:label runat="server" class="col-md-1 control-label" Width="120px">Fec. Genera</asp:label>
                                                            <asp:TextBox runat="server" ID="txt_FecGenera" CssClass="form-control LetraDetalle Centro" Enabled="false"  Width="120px" Height="20px" ></asp:TextBox>
                                                            <div class="clear padding3"></div>
                                                            <asp:label runat="server" class="col-md-1 control-label" Width="120px">Fec. Tesoreria</asp:label>
                                                            <asp:TextBox runat="server" ID="txt_FecTesoreria" CssClass="form-control LetraDetalle Centro" Enabled="false"  Width="120px" Height="20px" ></asp:TextBox>
                                                        </div>
                                             </div>
                                             
                                             <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:label runat="server" class="col-md-1 control-label" Width="90px">Contabilidad</asp:label>
                                                            <asp:TextBox runat="server" ID="txt_Contabilidad" Enabled="false"  CssClass="form-control LetraDetalle" Width="150px" Height="20px" ></asp:TextBox>
                                                            <div class="clear padding3"></div>
                                                            <asp:label runat="server" class="col-md-1 control-label" Width="90px">Anulación</asp:label>
                                                            <asp:TextBox runat="server" ID="txt_Anulacion" Enabled="false"  CssClass="form-control LetraDetalle" Width="150px" Height="20px" ></asp:TextBox>
                                                        </div>
                                             </div>

                                             <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:label runat="server" class="col-md-1 control-label" Width="120px">Fec. Contable</asp:label>
                                                            <asp:TextBox runat="server" ID="txt_FecContabilidad" CssClass="form-control LetraDetalle Centro" Enabled="false"  Width="120px" Height="20px" ></asp:TextBox>
                                                            <div class="clear padding3"></div>
                                                            <asp:label runat="server" class="col-md-1 control-label" Width="120px">Fec. Anulación</asp:label>
                                                            <asp:TextBox runat="server" ID="txt_FecAnulacion" CssClass="form-control LetraDetalle Centro" Enabled="false"  Width="120px" Height="20px" ></asp:TextBox>
                                                        </div>
                                                 </div>
                                        </div>
                                    </td>
                                </tr>
                                </table>

                        <div class="row">        
                            <div class="col-md-8" style="border:1px solid gray; border-width: 1px 0 0 0; padding: 0px 0 0 0">
                                <div class="form-group">
                                    <div class="input-group">
                                        <%--<asp:Button runat="server" ID="btn_Generar" Text="Generar"  CssClass="btn btn-primary" Width="100px" />--%>
                                        <asp:Button runat="server" ID="btn_Modificar" Text="Modificar"  CssClass="btn btn-info" Width="100px" />
                                        <asp:Button runat="server" ID="btn_Anular" Text="Anular"  CssClass="btn btn-warning" Width="100px" />
                                        <asp:Button runat="server" ID="btn_Texto" Text="Texto" data-toggle="modal" data-target="#TextoModal"  CssClass="btn btn-info" Width="100px" Visible="false" />
                                        <asp:Button runat="server" ID="btn_Imprimir" Text="Imprimir"  CssClass="btn btn-default" Width="100px" />
                                        <asp:Button runat="server" ID="btn_Documentos" Text="Documentos"  data-toggle="modal" data-target="#DocumentosModal"  CssClass="btn btn-link" Width="100px" />
                                        <%--<asp:Button runat="server" ID="btn_Imprimir" Text="Imprimir"  CssClass="btn btn-default" Width="100px"  />--%>
                                        <%-- <asp:Button runat="server" ID="btn_Consulta" Text="Consultar"  CssClass="btn btn-default" Width="100px" />--%>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4" style="border:1px solid gray; border-width: 1px 0 0 0; padding: 0px 0 0 0">
                                <div class="form-group">
                                    <div class="input-group">
                                            <asp:Label runat="server" ID="lbl_espacioBtn" ></asp:Label>
                                            <asp:Button runat="server" ID="btn_Aceptar" Text="Aceptar"  CssClass="btn btn-success" Width="90px"  />
                                            <asp:Button runat="server" ID="btn_Cancelar" Text="Cancelar"   CssClass="btn btn-danger" Width="90px"  />
                                            <asp:Button runat="server" ID="btn_Cerrar" Text="Cerrar"   CssClass="btn btn-danger" Width="90px"  />
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField runat="server" ID="hid_Operacion" Value="0" />
                        </div>
                    </div>
             </ContentTemplate>
            </asp:UpdatePanel>  
    </div>


    <div id="MensajeModal" style="width:400px; height:185px"  class="modalPoliza">
            <%--<div class="modal-content">--%>
                <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">
                            <asp:Label runat="server" ID="lbl_TitMensaje" Text="" style="color:darkblue;"></asp:Label>
                        </h4>
                </div>

                <div class="modal-body" style="height:143px">
                    <asp:UpdatePanel runat="server" ID="upMensaje">
                        <ContentTemplate>
                            <asp:label ID="txt_Mensaje" runat="server" BorderStyle="None" Width="368px" Height="80px" TextMode="MultiLine"></asp:label>
                            <div class="clear padding5"></div>
                            <div style="width:100%; text-align:right;">
                                    <button type="button" id="btn_CnlMensaje" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cerrar</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            <%--</div>--%>
    </div>

    <!-- Modal -->
    <div id="TextoModal" style="width:600px; height:200px"  class="modalTexto">
            <%--<div class="modal-content">--%>
                <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                        <h4 class="modal-title"><label id="lblTexto" style="color:darkblue;">Detalle de Pago</label></h4>
                </div>

                <div class="modal-body" style="height:178px">
                    <asp:UpdatePanel runat="server" ID="upTexto">
                        <ContentTemplate>
                          
                            <div class="form-group">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txt_Texto" Font-Size="11px" TextMode="MultiLine" Width="567" Height="95" ></asp:TextBox>
                                </div>
                            </div>  

                            <div style="width:100%; text-align:right;">
                                    <button type="button" id="btn_CnlTexto" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cerrar</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
           <%-- </div>--%>
    </div>

        <!-- Modal -->
    <div id="CuentasModal" style="width:700px; height:200px"  class="modalTexto">
            <%--<div class="modal-content">--%>
                <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                        <h4 class="modal-title"><label id="lblCuentas" style="color:darkblue;">Cuentas Bancarias</label></h4>
                </div>

                <div class="modal-body" style="height:178px">
                    <asp:UpdatePanel runat="server" ID="upCuentasBancarias">
                        <ContentTemplate>
                          
                            <asp:Panel runat="server" ID="pnlCtasBancarias" Width="100%" Height="100px" ScrollBars="Vertical">
                                <asp:GridView runat="server" ID="gvd_CuentasBancarias" AutoGenerateColumns="false" ForeColor="#333333"
                                                DataKeyNames="txt_numero_cuenta,id_cuenta_bancaria,cod_banco,txt_swift"
                                                GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                    <asp:checkbox runat="server" ID="chk_SelCta" CssClass="Select" Checked='<%# IIf(Eval("sn_default") = -1, True, False) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField >
                                        <asp:TemplateField HeaderText="Beneficiario">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbl_Beneficiario" Text='<%# Eval("txt_nombre_beneficiario") %>' Width="235px" CssClass="form-control" Font-Size="10px" ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Banco">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbl_Banco" Text='<%# Eval("txt_nombre") %>' Width="100px" CssClass="form-control" Font-Size="10px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tipo Cuenta">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbl_TipoCuenta" Text='<%# Eval("TipoDeCuenta") %>' Width="100px" CssClass="form-control" Font-Size="10px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cuenta">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbl_NroCuenta" Text='<%# Eval("txt_numero_cuenta") %>' Width="120px" CssClass="form-control" Font-Size="10px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Moneda">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbl_Moneda" Text='<%# Eval("Moneda") %>' Width="80px" CssClass="form-control" Font-Size="10px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </asp:GridView>
                            </asp:Panel>

                            <div style="width:100%; text-align:right;">
                                    <button type="button" id="btn_CnlCuenta" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cerrar</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            <%--</div>--%>
    </div>

    <!-- Modal -->
    <div id="DocumentosModal" style="width:700px; height:300px"  class="modalTexto">
            <%--<div class="modal-content">--%>
                <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                        <h4 class="modal-title"><label id="lblDocumentos" style="color:darkblue;">Control Documental</label></h4>
                </div>

                <div class="modal-body" style="height:278px">
                    <asp:UpdatePanel runat="server" ID="up_Documentos">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="pnlDocumentos" Width="100%" Height="165px" ScrollBars="Vertical">
                                <asp:GridView runat="server" ID="gvd_Documentos" AutoGenerateColumns="false" ForeColor="#333333"
                                                DataKeyNames="cod_version,txt_desc,fec_solicitud,fec_entrega"
                                                GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                    <asp:checkbox runat="server" ID="chk_SelDoc" CssClass="Select" />
                                            </ItemTemplate>
                                        </asp:TemplateField >
                                         <asp:TemplateField HeaderText="Ver.">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbl_Version" Text='<%# Eval("cod_version") %>' Height="22px" Width="50px" CssClass="form-control" ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Descripcion">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txt_Descripcion" Text='<%# Eval("txt_desc") %>' Height="22px" Width="400px" CssClass="form-control" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fec Solicita">
                                            <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txt_FechaSolicitud"  Text='<%# Eval("fec_solicitud") %>' CssClass="form-control FechaSB" Width="90px" Height="22px" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fec Entrega">
                                            <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txt_FechaEntrega"  Text='<%# Eval("fec_entrega") %>' CssClass="form-control FechaSB" Width="90px" Height="22px" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </asp:GridView>
                            </asp:Panel>

                             <div style="width:100%; text-align:right;">
                                    <asp:Button ID="btn_Añadir" runat="server" CssClass="btn btn-primary" Font-Size="11px"  Text="Agregar" Height="24px" />
                                    <asp:Button ID="btn_Quitar" runat="server" CssClass="btn btn-danger" Font-Size="11px"  Text="Quitar" Height="24px" />
                            </div>

                             <div class="clear padding10"></div>

                            <div style="width:100%; text-align:right;">
                                    <asp:Button runat="server" Text="Guardar" id="btn_OkDocumentos" CssClass="btn btn-success" style="height:30px; width:80px;" />
                                    <button type="button" id="btn_CnlDocumentos" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cerrar</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            <%--</div>--%>
    </div>

    <asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
        <ProgressTemplate>
            <div id="overlay">
                <div id="modalprogress" style="width:150px; height:95px">
                        <img src="../Images/gmx_mini.png" />
                        Procesando.....
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>  
</asp:Content>

