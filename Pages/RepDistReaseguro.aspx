<%@ Page Title="" Language="VB" MasterPageFile="~/Pages/SiteMaster.master" AutoEventWireup="false" CodeFile="RepDistReaseguro.aspx.vb" Inherits="Pages_RepDistReaseguro" %>
<%@ MasterType VirtualPath="~/Pages/SiteMaster.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentMaster" Runat="Server">
    <script src="../Scripts/RepDistReaseguro.js"></script>
    <script src="../Scripts/jquery.numeric.js"></script>
    <script src="../Scripts/jquery.maskedinput.js"></script>

     <script type="text/javascript"> 
         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageLoad);
    </script> 

     <asp:HiddenField runat="server" ID="hid_Ventanas" Value="0|0|0|0|" />


     <div style="width:900px; min-width:900px; overflow-x:hidden">
            <div class="panel-heading">
                <input type="image" src="../Images/collapse.png" id="coVentana0"  />
                <input type="image" src="../Images/expand.png"   id="exVentana0"  />
                <strong>Filtro Broker / Compañia</strong>
            </div>

            <div class="panel-body ventana0" >
                <div class="row">
                    <div class="col-md-6">
                        <div class="panel-heading">
                            <strong>Broker</strong>
                        </div>
                        
                        <div class="form-group">
                            <div class="input-group">
                                <asp:UpdatePanel runat="server" ID="upBroker">
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="pnlBroker" Width="415px" Height="130px" ScrollBars="Both">
                                                <asp:GridView runat="server" ID="gvd_Broker" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal" ShowHeaderWhenEmpty="true" >
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                    <asp:HiddenField runat="server" ID="chk_SelBro" value="false"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField >
                                                        <asp:TemplateField HeaderText="Clave">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl_ClaveBro" Text='<%# Eval("Clave") %>' Width="50px" Font-Size="10px" ></asp:Label>
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
                                                <button type="button" id="btn_AddBroker" class="btn btn-success" data-toggle="modal" data-target="#EsperaModal" >Añadir</button>
                                            </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="panel-heading">
                            <strong>Compañia</strong>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <asp:UpdatePanel runat="server" ID="upCompañia">
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="pnlCompañia" Width="415px" Height="130px" ScrollBars="Both">
                                                <asp:GridView runat="server" ID="gvd_Compañia" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                                            <ItemTemplate>
                                                                <asp:HiddenField runat="server" ID="chk_SelCia" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Clave" ItemStyle-CssClass="ClaveCia">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl_ClaveCia" Text='<%# Eval("Clave") %>' Width="50px" Font-Size="9.5px" ></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Descripción" ItemStyle-CssClass="DescripcionCia">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl_Desc" Text='<%# Eval("Descripcion") %>' Width="310px" Font-Size="9.5px"></asp:Label>
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
                                                <button type="button" id="btn_AddCia" class="btn btn-success" data-toggle="modal" data-target="#EsperaModal" >Añadir</button>
                                            </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel> 
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="panel-heading">
                <input type="image" src="../Images/collapse.png" id="coVentana1"  />
                <input type="image" src="../Images/expand.png"   id="exVentana1"  />
                <strong>Filtro Pólizas</strong>
            </div>

            <div class="panel-body ventana1" >
                <div class="col-md-12">
                    <div class="panel-heading">
                        <strong>Póliza</strong>
                    </div>
                    <div class="form-group">
                        <div class="input-group">
                            <asp:UpdatePanel runat="server" ID="upPoliza">
                                <ContentTemplate>
                                    <asp:HiddenField runat="server" ID="hid_Polizas" Value="" />
                                    <asp:Panel runat="server" ID="pnlPoliza" Width="930" Height="200px" ScrollBars="Both">
                                            <asp:GridView runat="server" ID="gvd_Poliza" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="chk_SelPol" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Clave" ItemStyle-CssClass="ClaveCia">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl_ClavePol" Text='<%# Eval("Clave") %>' Width="100px" Font-Size="9.5px" ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Grupo Endoso">
                                                        <ItemTemplate>
                                                            <asp:label runat="server" ID="lbl_DescripcionPol" Enabled="false" Text='<%# Eval("Descripcion")   %>' Width="260px" Font-Size="9.5px"  ></asp:label>
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
                                            <button type="button" id="btn_AddPol" class="btn btn-success" data-toggle="modal" data-target="#PolizaModal" >Añadir</button>
                                        </div>
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                        </div>
                    </div>                                                
                </div>
            </div>


            <div class="row">
                <div class="col-md-12">
                    <div class="panel-heading">
                    <input type="image" src="../Images/collapse.png" id="coVentana2"  />
                    <input type="image" src="../Images/expand.png"   id="exVentana2"  />
                    <strong>Filtro Ramos Contables / Productos</strong>
                    </div>
                    <div class="panel-body ventana2" >
                        <asp:UpdatePanel runat="server" ID="upAdicionales">
                            <ContentTemplate>
                                    <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:UpdatePanel runat="server" ID="upRamoContable">
                                                        <ContentTemplate>
                                                            <div class="panel-heading">
                                                                <strong>Ramo Contable</strong>
                                                            </div>
                                                            <div class="clear padding10"></div>
                                                            <asp:Panel runat="server" ID="Panel1" Width="415px" Height="130px" ScrollBars="Both">
                                                                <asp:GridView runat="server" ID="gvd_RamoContable" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="">
                                                                            <ItemTemplate>
                                                                                    <asp:HiddenField runat="server" ID="chk_SelRamC" value="false"/>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField >
                                                                        <asp:TemplateField HeaderText="Clave">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lbl_ClaveRamC" Text='<%# Eval("Clave") %>' Width="80px" Font-Size="10px" ></asp:Label>
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
                                                                <button type="button" id="btn_AddRamoContable" class="btn btn-success" data-toggle="modal" data-target="#EsperaModal"  >Añadir</button>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel> 
                                                    </div>
                                                </div>
                                            </div>
                                     
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <asp:UpdatePanel runat="server" ID="upProducto">
                                                        <ContentTemplate>
                                                            <div class="panel-heading">
                                                                <strong>Producto</strong>
                                                            </div>
                                                            <div class="clear padding10"></div>
                                                            <asp:Panel runat="server" ID="Panel2" Width="415px" Height="130px" ScrollBars="Both">
                                                                <asp:GridView runat="server" ID="gvd_Producto" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="">
                                                                            <ItemTemplate>
                                                                                    <asp:HiddenField runat="server" ID="chk_SelPro" value="false"/>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField >
                                                                        <asp:TemplateField HeaderText="Clave">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lbl_ClavePro" Text='<%# Eval("Clave") %>' Width="50px" Font-Size="10px" ></asp:Label>
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
                                                                <button type="button" id="btn_AddProducto" class="btn btn-success" data-toggle="modal" data-target="#EsperaModal"  >Añadir</button>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel> 
                                                    </div>
                                                </div>
                                            </div>
                                    </div>
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                    </div>
                </div>
            </div>
     </div>

    <div id="EsperaModal" style="width:150px; height:95px"  class="modalWait">
        <img src="../Images/gmx_mini.png" />
        Procesando.....
    </div>

     <!-- Modal -->
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
                              <asp:GridView runat="server" ID="gvd_Catalogo" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal" CssClass=".gvd_Catalogo1"  ShowHeaderWhenEmpty="true" >
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
</asp:Content>

