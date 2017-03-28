<%@ Page Title="" Language="VB" MasterPageFile="~/Pages/SiteMaster.master" AutoEventWireup="false" CodeFile="FirmasElectronicas.aspx.vb" Inherits="Pages_FirmasElectronicas" %>
<%@ MasterType VirtualPath="~/Pages/SiteMaster.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentMaster" Runat="Server">
    <script src="../Scripts/jquery.maskedinput.js"></script>
    <script src="../Scripts/FirmasElectronicas.js"></script>

    
    <script type="text/javascript"> 
         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageLoad);
    </script> 

    <asp:UpdatePanel runat="server" ID="upOcultos">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hid_Url" Value="" />
                <asp:Button ID="btn_CerrarSesion" runat="server" CssClass="NoDisplay CerrarSesion" />
                <asp:HiddenField ID="hid_CierraSesion" runat="server" Value="0" />
            </ContentTemplate>
   </asp:UpdatePanel>    

    <div class="panel-heading">
        <strong>Listado Órdenes de Pago</strong>
    </div>
                                
    <div class="panel-body ventana3">
        <div class="row">
            <div class="col-md-6">
                <asp:UpdatePanel runat="server" ID="upFechasGen">
                    <ContentTemplate>
                        <table>
                                <tr>
                                    <td>
                                        <div class="form-group">
                                            <div class="input-group">
                                                <asp:label runat="server" class="col-md-1 control-label" Width="100px">Generada</asp:label>
                                                <asp:TextBox runat="server" ID="txtFecGeneraDe" CssClass="form-control Fecha" Width="110px" Height="26px" ></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="form-group">
                                            <div class="input-group">
                                                <asp:label runat="server" class="col-md-1 control-label" Width="10px">A</asp:label>
                                                <asp:TextBox runat="server" ID="txtFecGeneraA" CssClass="form-control Fecha" Width="110px" Height="26px" ></asp:TextBox>
                                            </div>
                                        </div>  
                                    </td>
                                </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="input-group">
                            <asp:UpdatePanel runat="server" ID="upUsuarios">
                            <ContentTemplate>
                                <div class="panel-heading">
                                    <strong>Usuario Solicitante</strong>
                                </div>
                                <div class="clear padding10"></div>
                                <asp:Panel runat="server" ID="pnlUsuario" Width="415px" Height="130px" ScrollBars="Both">
                                    <asp:GridView runat="server" ID="gvd_Usuario" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="chk_SelUsu" value="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField >
                                            <asp:TemplateField HeaderText="Clave">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_ClaveUsu" Text='<%# Eval("Clave") %>' Width="80px" Font-Size="10px" ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripción">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_Desc" Text='<%# Eval("Descripcion") %>' Width="280px" Font-Size="10px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button Text="X" Height="26px" runat="server" CssClass="Delete btn btn-danger" />
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
                                <div style="width:100%;  text-align:right">
                                    <button type="button" id="btn_AddUsuario" class="btn btn-success" data-toggle="modal" data-target="#EsperaModal"  >Añadir</button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                        </div>
                    </div>
                </div>
                                     
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="input-group">
                            <asp:UpdatePanel runat="server" ID="upEstatus">
                            <ContentTemplate>
                                <div class="panel-heading">
                                    <strong>Estatus Órden Pago</strong>
                                </div>
                                <div class="clear padding10"></div>
                                <asp:Panel runat="server" ID="pnlEstatus" Width="415px" Height="130px" ScrollBars="Both">
                                    <asp:GridView runat="server" ID="gvd_Estatus" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="chk_SelEst" value="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField >
                                            <asp:TemplateField HeaderText="Clave">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_ClaveEst" Text='<%# Eval("Clave") %>' Width="50px" Font-Size="10px" ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripción">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_Desc" Text='<%# Eval("Descripcion") %>' Width="310px" Font-Size="10px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button Text="X" Height="26px" runat="server" CssClass="Delete btn btn-danger" />
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
                                <div style="width:100%;  text-align:right">
                                    <button type="button" id="btn_AddEstatus" class="btn btn-success" data-toggle="modal" data-target="#EsperaModal"  >Añadir</button>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                        </div>
                    </div>
                </div>
        </div>
                                    
        <div class="row">
            <div class="col-md-12">
                    <div class="form-group">
                        <div class="input-group">
                            <asp:UpdatePanel runat="server" ID="upLstOP">
                            <ContentTemplate>
                                <div class="panel-heading">
                                    <strong>Órdenes de Pago</strong>
                                </div>
                                <div class="clear padding10"></div>
                                <asp:Panel runat="server" ID="pnlOrdenP" Width="860px" Height="440px" ScrollBars="Vertical">
                                    <asp:GridView runat="server" ID="gvd_LstOrdenPago" AutoGenerateColumns="false" ForeColor="#333333" 
                                                    GridLines="Horizontal"  ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10" 
                                                    DataKeyNames="nro_op,id_imputacion,id_pv,cod_estatus_op,fec_baja,fec_autoriz_sector,fec_autoriz_contab,fec_pago">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Filtro" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                        <asp:CheckBox runat="server"  Width="40px" ID="chk_SelOp" Checked='<%# Eval("tSEl_Val") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField >

                                            <asp:TemplateField HeaderText="Nro Op">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lbl_OrdenPago" Text='<%# Eval("nro_op")%>' CssClass="form-control DetExh Link" Height="25px"></asp:LinkButton>
                                                    <%--<asp:Label runat="server" ID="lbl_OrdenPago" Text='<%# Eval("nro_op") %>' Width="60px" Font-Size="10px" ></asp:Label>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Impresión" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                        <asp:CheckBox runat="server" Width="70px" ID="chk_Impresion" Checked='<%# Eval("sn_impresion") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField >

                                            <asp:TemplateField HeaderText="Solicitante" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                        <asp:CheckBox runat="server" Text="Firma" Width="90px" ID="chk_FirmaSol" Checked='<%# Eval("sn_Solicita") %>' AutoPostBack="true"/>
                                                </ItemTemplate>
                                            </asp:TemplateField >

                                            <asp:TemplateField HeaderText="Jefe Inmediato" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                        <asp:CheckBox runat="server" Text="Firma" Width="100px" ID="chk_FirmaJefe" Checked='<%# Eval("sn_JefeDirecto") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField >

                                            <asp:TemplateField HeaderText="Dirección Area" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                        <asp:CheckBox runat="server" Text="Firma" Width="100px" ID="chk_FirmaDir" Checked='<%# Eval("sn_DireccionArea") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField >

                                            <asp:TemplateField HeaderText="Contabilidad" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                        <asp:CheckBox runat="server" Text="Firma" Width="100px" ID="chk_FirmaCon" Checked='<%# Eval("sn_Contabilidad") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField >

                                            <asp:TemplateField HeaderText="Asegurado">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_Asegurado" Text='<%# Eval("Asegurado")%>' Width="200px" Font-Size="9px" Height="25px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ramos Contables" ControlStyle-Width="200px">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddl_RamosContables"  Height="25px" Font-Size="9px" 
                                                                        runat="server">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_RamosContables" 
                                                                Text='<%# DataBinder.Eval(Container.DataItem, "Ramos") %>' 
                                                                Visible="false"     
                                                                runat="server"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Broker / Compañia">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_BroCia" Text='<%# Eval("txt_otros") %>' Width="350px" Font-Size="9px" ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fec Pago">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_FechaPago" Text='<%# Eval("Fec_Pago") %>' Width="70px" Font-Size="10px" ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Usuario">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_Usuario" Text='<%# Eval("Solicitante") %>' Width="150px" Font-Size="10px"  ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Monto">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_Monto" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("Monto")))  %>' Width="90px" Font-Size="10px"  ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Estatus">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbl_Estatus" Text='<%# Eval("estatus") %>' Width="100px" Font-Size="10px" ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                   
                                        </Columns>
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="11px" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                </asp:Panel>
                                <div style="width:100%; text-align:right;">
                                               
                                        <asp:Button runat="server" ID="btn_BuscaOP" Text="Buscar OP" CssClass="btn btn-primary" />
                                        <asp:Button runat="server" ID="btn_Imprimir" Text="Imprimir" CssClass="btn btn-default" />

                                        <asp:Button runat="server" ID="btn_Solicitante" Text="Solicitate" CssClass="btn btn-primary" data-toggle="modal" data-target="#AutorizaModal" Enabled="false" />
                                        <asp:Button runat="server" ID="btn_Jefe" Text="Jefe Inmediato" CssClass="btn btn-primary" data-toggle="modal" data-target="#AutorizaModal" Enabled="false" />
                                        <asp:Button runat="server" ID="btn_Direccion" Text="Dirección Area" CssClass="btn btn-primary" data-toggle="modal" data-target="#AutorizaModal" Enabled="false" />
                                        <asp:Button runat="server" ID="btn_Contabilidad" Text="Contabilidad" CssClass="btn btn-primary" data-toggle="modal" data-target="#AutorizaModal" Enabled="false" />
                                        <asp:Button runat="server" ID="btn_Limpiar" Text="Limpiar" CssClass="btn btn-danger" />
                                </div>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
            </div>
        </div>
    </div>

     <div id="CatalogoModal" style="width:400px; height:440px"  class="modal">
          <div class="modal-content">
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"><label id="lblCatalogo" style="color:darkblue;">Catálogo</label></h4>
                    <asp:HiddenField runat="server" ID="hid_Control" Value="" />
               </div>

               <div class="modal-body" style="height:398px">
                   <asp:UpdatePanel runat="server" ID="upCatalogo">
                       <ContentTemplate>

                        <div class="input-group">
                            <asp:label runat="server" class="col-md-1 control-label" Width="50px">Filtro:</asp:label>
                            <input type="text" id="txtFiltrar" class="form-control" style="width:290px; height:26px; font-size:12px;" />
                        </div>

                           
                          <asp:Panel runat="server" ID="pnlCatalogo" Width="100%" Height="320px" ScrollBars="Vertical">
                              <asp:GridView runat="server" ID="gvd_Catalogo" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                  <Columns>
                                       <asp:TemplateField HeaderText="">
                                          <ItemTemplate>
                                              
                                          </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:BoundField  ItemStyle-Width="90px" ItemStyle-Height="10px" DataField="Clave" HeaderText="Clave" HeaderStyle-HorizontalAlign="Center"  />
                                       <asp:BoundField ItemStyle-Width="320px" ItemStyle-Height="10px" DataField="Descripcion" HeaderText="Descripcion" HeaderStyle-HorizontalAlign="Center"  />
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
                             <asp:Button runat="server" id="btn_OkCatalogo" class="btn btn-success" Text="Aceptar"  style="height:30px; width:80px;" />
                             <button type="button" id="btn_CnlCatalogo" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cancelar</button>
                             <asp:HiddenField runat="server" ID="hid_Seleccion" Value="" />
                             <asp:HiddenField runat="server" ID="hid_Catalogo" Value="" />
                          </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
          </div>
    </div>

    <div id="AutorizaModal" style="width:350px; height:360px"  class="modalAutoriza">
       <%-- <div class="modal-content" width:100%">--%>
            <div class="panel-heading">
                <strong>FIRMAS ELECTRÓNICAS</strong>
            </div>
            <div class="panel-body" style="height:360px; width:100%">
                <div class="row">
                    <div class="text-center">
                        <img class="profile-img" src ="../Images/icono-pago.png" alt=""/>
                    </div>
                </div>
                <br />
                <asp:UpdatePanel runat="server" ID="upAutoriza">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-sm-12 col-md-10">

                                    <div class="input-group">
                                        <asp:label runat="server" class="col-md-1 control-label" Width="86px">Usuario</asp:label>
                                        <span class="input-group-addon">
                                            <i class="glyphicon glyphicon-user"></i>
                                        </span>
                                        <asp:TextBox runat="server" ID="txt_usuario" class="form-control User" Width="150px" Font-Size="11px" ></asp:TextBox>
                                     </div>
                                
                                    <div class="clear padding10"></div>

                                    <div class="input-group"> 
                                        <asp:label runat="server" class="col-md-1 control-label" Width="86px">Contraseña</asp:label>
                                        <span class="input-group-addon">
                                            <i class="glyphicon glyphicon-lock"></i>
                                        </span>
                                        <asp:TextBox runat="server" ID="txt_contraseña" class="form-control Password" Width="150px" TextMode="Password" Font-Size="11px" ></asp:TextBox>
                                    </div>

                                    <div class="clear padding10"></div>
                                    
                                    <div class="input-group">
                                        <asp:label runat="server" ID="lbl_Asocia" class="col-md-1 control-label" Width="86px"></asp:label>
                                        <span class="input-group-addon">
                                            <i class="glyphicon glyphicon-user" ></i>
                                        </span>
                                        <asp:DropDownList runat="server" ID="ddl_Usuarios" CssClass="form-control" Width="150px"></asp:DropDownList>
                                    </div>
                                
                            </div>
                            
                            <div class="clear padding10"></div>
                            <div class="clear padding10"></div>

                            <div style="width:90%; text-align:right;">
                                <asp:Button ID="btnAceptar" runat="server" Text="Firmar" Width="125px"  class="btn btn-primary" />
                                <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" Width="125px"  class="btn btn-danger CierraAutoriza" />
                            </div>
                          
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
       <%-- </div>--%>
    </div>

    <div id="EsperaModal" style="width:150px; height:95px"  class="modalWait">
        <img src="../Images/gmx_mini.png" />
        Procesando.....
    </div>

     <!-- Modal -->
    <div id="MensajeModal" style="width:400px; height:185px"  class="modalAutoriza">
         <%-- <div class="modal-content">--%>
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                     <h4 class="modal-title">
                         <asp:Label runat="server" ID="lbl_TitMensaje" Text="" Width="400px" style="color:darkblue;"></asp:Label>
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
</asp:Content>

