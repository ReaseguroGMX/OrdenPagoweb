<%@ Page Title="" Language="VB" MasterPageFile="SiteMaster.master" AutoEventWireup="false" ClientIDMode="AutoID" CodeFile="OrdenPago.aspx.vb" Inherits="Pages_OrdenPago" %>
<%@ MasterType VirtualPath="~/Pages/SiteMaster.master" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentMaster" Runat="Server">
    <script src="../Scripts/jquery.maskedinput.js"></script>
    <script src="../Scripts/Controls.js"></script>
    <script src="../Scripts/jquery.numeric.js"></script>


 <script type="text/javascript"> 
     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageLoad);

</script> 

        <asp:UpdatePanel runat="server" ID="upOcultos">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hid_Url" Value="" />
                <asp:Button ID="btn_CerrarSesion" runat="server" CssClass="NoDisplay CerrarSesion" />
                <asp:HiddenField ID="hid_CierraSesion" runat="server" Value="0" />
                <asp:HiddenField runat="server" ID="hid_idPv" Value="0" />
            </ContentTemplate>
        </asp:UpdatePanel>    
       
        <%--<div class="text-center">
            <img class="profile-img" src ="../Images/icono-pago.png" alt=""/> 
        </div>
               

        <div class="text-center">
            <img class="profile-img" src="../Images/icono-ambiente.png" />
        </div>--%>

        <%-----------------------------------Sección 1----------------------------------------------------------------------------------------------------%>
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
                                                <asp:GridView runat="server" ID="gvd_Broker" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
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


            <%-----------------------------------Sección 2----------------------------------------------------------------------------------------------------%>
            <div class="panel-heading">
                <input type="image" src="../Images/collapse.png" id="coVentana1"  />
                <input type="image" src="../Images/expand.png"   id="exVentana1"  />
                <strong>Filtro Facultativos / Pólizas</strong>
            </div>

            <div class="panel-body ventana1" >
                <div class="row">
                    <div class="col-md-6">
                            <div class="panel-heading">
                            <strong>Facultativos</strong>
                        </div>
                        <div class="clear padding5"></div>
                            <asp:UpdatePanel runat="server" ID="upFechas">
                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="input-group">
                                            <table>
                                            <tr>
                                                <td>
                                                    <asp:label runat="server" class="col-md-1 control-label" Width="90px">Fec. Fac</asp:label>
                                                    <asp:TextBox runat="server" ID="txt_FechaDe" CssClass="form-control Fecha" Width="110px" Height="26px" ></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:label runat="server" class="col-md-1 control-label" Width="38px">A</asp:label>
                                                    <asp:TextBox runat="server" ID="txt_FechaA" CssClass="form-control Fecha" Width="110px" Height="26px" ></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>  
                            </ContentTemplate>
                        </asp:UpdatePanel>
                                            
                                                            
                        <div class="form-group">
                            <div class="input-group">
                                <asp:UpdatePanel runat="server" ID="upContrato">
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="pnlContrato" Width="415px" Height="100px" ScrollBars="Both">
                                            <asp:GridView runat="server" ID="gvd_Contrato" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="chk_SelFac" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Clave" ItemStyle-CssClass="ClaveCia">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl_ClaveFac" Text='<%# Eval("Clave") %>' Width="60px" Font-Size="9.5px" ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Descripción" ItemStyle-CssClass="DescripcionCia">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl_Desc" Text='<%# Eval("Descripcion") %>' Width="300px" Font-Size="9.5px"></asp:Label>
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
                                            <button type="button" id="btn_AddCtr" class="btn btn-success" data-toggle="modal" data-target="#EsperaModal" >Añadir</button>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div> 
                    </div>


                    <div class="col-md-6">
                        <div class="panel-heading">
                            <strong>Póliza</strong>
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <asp:UpdatePanel runat="server" ID="upPoliza">
                                    <ContentTemplate>
                                        <asp:HiddenField runat="server" ID="hid_Polizas" Value="" />
                                        <asp:HiddenField runat="server" ID="hid_HTML" Value="" />
                                        <div class="clear padding5"></div>
                                        <div class="input-group">
                                            <asp:label runat="server" class="col-md-1 control-label" Width="120px">Busqueda por:</asp:label>
                                            <asp:RadioButtonList runat="server" ID="optAjuste" CssClass="rbl" RepeatDirection="Horizontal" AutoPostBack="true">
                                                <asp:ListItem Selected="True" Value="0" Text="Endosos"></asp:ListItem>
                                                <asp:ListItem  Value="1" Text="Endosos y Ajustes"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="clear padding10"></div>
                                        <div class="clear padding3"></div>
                                        <asp:Panel runat="server" ID="pnlPoliza" Width="415px" Height="100px" ScrollBars="Both">
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
            </div>



            <div class="row">
                <div class="col-md-12">
                    <div class="panel-heading">
                    <input type="image" src="../Images/collapse.png" id="coVentana2"  />
                    <input type="image" src="../Images/expand.png"   id="exVentana2"  />
                    <strong>Filtros Adicionales</strong>
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

                                    <table>
                                    <tr>
                                        <td>
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <asp:label runat="server" class="col-md-1 control-label" Width="100px">Fec. Pago</asp:label>
                                                    <asp:TextBox runat="server" ID="txtFecPagoDe" CssClass="form-control Fecha" Width="110px" Height="26px" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <asp:label runat="server" class="col-md-1 control-label" Width="10px">A</asp:label>
                                                    <asp:TextBox runat="server" ID="txtFecPagoA" CssClass="form-control Fecha" Width="110px" Height="26px" ></asp:TextBox>
                                                </div>
                                            </div>  
                                        </td>
                                        <td>
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <asp:label runat="server" class="col-md-1 control-label" Width="130px">Moneda</asp:label>
                                                    <asp:DropDownList runat="server" ID="ddl_Moneda" CssClass="form-control" Width="300px" Height="26px"></asp:DropDownList>
                                                </div>
                                            </div>  
                                        </td>
                                    </tr>
                                                     
                                    <tr>
                                        <td  colspan="2" style="border:solid; border-width:1px; border-color:midnightblue; vertical-align:top;" >
                                            <asp:label runat="server" class="col-md-1 control-label" Width="120px">Cobranzas</asp:label>
                                            <asp:RadioButtonList runat="server" ID="opt_Cobranzas" RepeatDirection="Vertical" CssClass="rbl" Width="300px"  >
                                                <asp:ListItem Text="Todos" Value="-1" Selected="True"/>
                                                <asp:ListItem Text="Cobrados" value="1" />
                                                <asp:ListItem Text="Compensados" value="2" />
                                                <asp:ListItem Text="Parcial Cobrado" Value="3" />
                                                <asp:ListItem Text="No Cobrados/No Compensados" Value="0" />
                                                <asp:ListItem Text="Cancelados" Value="4" />
                                                <%--<asp:ListItem Text="Primas en Deposito" Value="3" />--%>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="border:solid; border-width:1px; border-color:midnightblue; vertical-align:top;">
                                            <asp:label runat="server" class="col-md-1 control-label" Width="120px">Reaseguro</asp:label>
                                            <asp:RadioButtonList runat="server" ID="opt_Estatus" RepeatDirection="Vertical" CssClass="rbl" Width="300px" >
                                                <asp:ListItem Text="Todos" Value="-1" Selected="True" />
                                                <asp:ListItem Text="Pagados" value="1" />
                                                <asp:ListItem Text="Pagados en Exceso" Value="2" />
                                                <asp:ListItem Text="Saldo Pendiente" Value="0" />
                                                <asp:ListItem Text="Solicitados" Value="3" />
                                                <asp:ListItem Text="Recuperaciones" Value="4" />
                                                <asp:ListItem Text="Temporales" Value="5" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                                               
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                    </div>
                </div>
            </div>


            <div class="panel-heading">
                <input type="image" src="../Images/collapse.png" id="coVentana3"  />
                <input type="image" src="../Images/expand.png"   id="exVentana3"  />
                <strong>Filtro Órdenes de Pago</strong>
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
                                        <asp:Panel runat="server" ID="pnlUsuario" Width="415px" Height="100px" ScrollBars="Both">
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
                                        <asp:Panel runat="server" ID="pnlEstatus" Width="415px" Height="100px" ScrollBars="Both">
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
                                        <asp:Panel runat="server" ID="pnlOrdenP" Width="860px" Height="200px" ScrollBars="Vertical">
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
                                                   
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:Button CommandName="Corregir" runat="server" ID="btnCorregir" CssClass="btn-success" ToolTip="Corrige consecutivo de Imputación" Text="Corregir" /> 
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:Button CommandName="Restaurar" runat="server" ID="btnRestaurar" CssClass="btn-primary" ToolTip="Recupera Movimientos" Text="Recuperar" /> 
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

                                                <asp:Button runat="server" ID="btn_Limpiar" Text="Limpiar" CssClass="btn btn-danger" />
                                        </div>
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                    </div>
                </div>
            </div>


                
            </div>

            <div class="clear padding5"></div>

            <div  style="width:900px; border:5px solid gray; border-width: 2px 0 0 0; text-align:right; padding: 0 0 0 620px; "  >
                <asp:UpdatePanel runat="server" ID="upBusqueda">
                    <ContentTemplate>
                        <div class="form-group">
                            <div class="input-group">
                                <asp:Button runat="server" ID="btn_Buscar" CssClass="form-control btn-primary" Text="Buscar" Width="140px" />
                                <asp:Button runat="server" ID="btn_CnlBuscar" CssClass="form-control btn-danger" Text="Cancelar Busqueda" Width="140px" />
                                <asp:HiddenField runat="server" ID="hid_Ventanas" Value="0|0|0|1|" />
                             </div>
                        </div>
                        <div class="clear padding2"></div>
                        <asp:Button runat="server" ID="btn_Descartadas" CssClass="form-control btn-info" Text="Pólizas No Sujetas a Pago" Width="280px" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>



    <div class="clear padding5"></div>

    <div class="row" >
        <div class="col-md-12"> 

            <table>
                <tr>
                    <td colspan="2">
                        <div class="col-md-2" style="background-color:lightblue; color:darkblue; text-align:center"><label>Encabezado</label> </div> 
                        <div class="col-md-2" style="background-color:lightgreen; color:Green; text-align:center""><label>Pagado</label></div> 
                        <div class="col-md-2" style="background-color:yellow; color:black; text-align:center""><label>Solicitado</label></div> 
                        <div class="col-md-2" style="background-color:lightgray; color:Green; text-align:center""><label>SobrePagado</label></div> 
                        <div class="col-md-2" style="background-color:orangered; color:white; text-align:center""><label>Vencido</label></div> 
                        <div class="col-md-2" style="background-color:white; color:black; text-align:center""><label>Sin vencer</label></div> 
                    </td>
                </tr>
                <tr>
                    <td style="width:180px">
                        <label runat="server" class="form-control" style="text-align:center; font-size:12px; background-color:#284775; color:white;" >PÓLIZA</label>
                    </td>
                    <td style="width:720px">
                        <label runat="server" class="form-control" style="text-align:center; font-size:12px; background-color:#284775; color:white;" >DISTRIBUCIÓN</label>
                    </td>
                </tr>
            </table>
                                                     
            <asp:UpdatePanel runat="server" ID="upReaseguro">
                <ContentTemplate>

                    <asp:Panel runat="server" ID="pnlreaseguro" Width="900px"  ScrollBars="None">
                        <asp:HiddenField runat="server" ID="hid_Index" Value="-1" />
                        <asp:HiddenField runat="server" ID="hid_Paquete" Value="0" />
                        <asp:GridView runat="server" ID="gvd_Reaseguro" AutoGenerateColumns="false" ForeColor="#333333"  AllowPaging="true" PageSize="25"
                                        GridLines="None" onprerender="gvd_Reaseguro_PreRender"  ShowHeaderWhenEmpty="true"   CssClass="LetraDetalleGrid"
                                        ShowHeader ="false" DataKeyNames="Id_Pol,bln_Cambio,Poliza,nro_reas,nro_layer,cod_ramo_contable,ramo_contable,
                                                                            id_contrato,Contrato,nro_tramo,PrimaCedida,Comision,PrimaPagada,ComPagada,SaldoPrima,SaldoComision,blnPendiente,
                                                                            id_pv,cod_suc,cod_ramo,nro_pol,aaaa_endoso,nro_endoso,cod_moneda,cod_aseg,id_persona,Asegurado,cod_estatus_op,PrimaTotal,PrimaCobrada,PrimaCompensada,PrimaDeposito,PrimaRecuperada,
                                                                            cod_grupo_endo,GrupoEndoso,cod_tipo_endo,TipoEndoso,Parcial,blnCancelada,sn_NoPago">
                            
                             <Columns> 

                                <%-- <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" CommandName="GeneraOP"  ID="btn_GeneraOP" ImageUrl="~/Images/icono-orden-pago.png" AlternateText="Genera OP" Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Póliza" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Image runat="server" ID="img_Check" ImageUrl="~/Images/icono_check.png" Visible="false" />
                                        <asp:Label runat="server" ID="lbl_Poliza" Text='<%# Eval("Poliza") %>' CssClass="form-control Poliza" Font-Bold="true"  ForeColor="White" BackColor="#284775" Font-Size="11px"  ></asp:Label>
                                        <asp:Label runat="server" ID="lbl_Cobranza" Text="Detalle Cobranza Prima Neta" CssClass="form-control Encabezado" Font-Bold="true"  ForeColor="white" BackColor="#284775"  ></asp:Label>
                                        <div id="div_primaCob" style="text-align:right;" runat="server" class="form-control Cobranza masterTooltip" title='<%# Eval("Asegurado") %>' >
                                                <label style="width:75px">Emitida:</label>
                                                <asp:Label runat="server" ID="lbl_PrimaTotal" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaTotal")))  %>' Width="85px" Font-Bold="true" ForeColor="DarkBlue" CssClass="Monto"></asp:Label>
                                                <label style="width:75px">Aplicada:</label>
                                                <asp:Label runat="server"  ID="lbl_PrimaCobrada" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaCobrada")))  %>' Width="85px" Font-Bold="true" ForeColor="DarkBlue" CssClass="Monto"></asp:Label>
                                                <label style="width:75px">Compensada:</label>
                                                <asp:Label runat="server"  ID="lbl_PrimaCompensada" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaCompensada")))  %>' Width="85px" Font-Bold="true" ForeColor="DarkBlue" CssClass="Monto"></asp:Label>
                                                <label style="width:75px">en Deposito:</label>
                                                <asp:Label runat="server"  ID="lbl_PrimaDeposito" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaDeposito")))  %>' Width="85px" Font-Bold="true" ForeColor="DarkBlue" CssClass="Monto"></asp:Label>
                                                <label style="width:75px">Saldo:</label>
                                                <asp:Label runat="server"  ID="lbl_SaldoCob" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaTotal")) - (CDbl(Eval("PrimaCobrada")) + CDbl(Eval("PrimaCompensada"))))  %>' BorderColor="DarkBlue" Width="85px" Font-Bold="true" ForeColor="DarkBlue" CssClass="Monto"></asp:Label>
                                                <label style="width:75px">Recargo:</label>
                                                <asp:Label runat="server"  ID="lbl_Recargo" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("Recargo")))  %>' BorderColor="DarkBlue" Width="85px" Font-Bold="true" ForeColor="DarkBlue" CssClass="Monto"></asp:Label>
                                        </div>   
                                        <asp:Button runat="server" ID="btn_DetalleCuotas" CommandName="DetalleCob" Text="Detalle Cuotas" CssClass="form-control btn-success" Visible="false" Height="22px" />   
                                        <asp:Button runat="server" ID="btn_GeneraOP" CommandName="GeneraOP" Text="Generar Órden de Pago" CssClass="form-control btn-primary" Visible="false" Height="22px" />   
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Capa / Ramo / Contrato" >
                                    <ItemTemplate>      
                                    <%--       <asp:Label runat="server" ID="lbl_Espacio3" Text="" CssClass="form-control Espacio"  BorderStyle="None"  ></asp:Label>--%>
                                        <asp:Label runat="server" ID="lbl_Espacio3" Text='<%# Eval("GrupoEndoso") %>' CssClass="form-control GrupoEndoso masterTooltip" Font-Bold="true"  ForeColor="Yellow" BackColor="#284775" Font-Size="9px" title='<%# Eval("TipoEndoso") %>' ></asp:Label>
                                        <asp:Label runat="server" ID="lbl_InfoPol" Text="Detalle Póliza" CssClass="form-control DetAdicional" Font-Bold="true"  ForeColor="White" BackColor="#284775"  ></asp:Label>                                                                                
                                        <div id="div_capa" runat="server" class="form-control Capa">
                                                <div class="clear padding3"></div>
                                                                        
                                                <table style="width:100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="lbl_TitCapa" Font-Bold="true" ForeColor="DarkBlue" Text='<%# Eval("LabelCapa")   %>' Width="70px" ></asp:Label>
                                                                <asp:Label runat="server" ID="lbl_Capa" Font-Bold="true" ForeColor="DarkBlue" Text='<%# Eval("nro_layer")   %>' ></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lbl_TitAjuste" Font-Bold="true" ForeColor="DarkBlue" Width="70px" >Ajuste:</asp:Label>
                                                                <asp:Label runat="server" ID="lbl_Ajuste" Font-Bold="true" ForeColor="DarkBlue" Text='<%# Eval("nro_reas")%>' ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:150px; height:30px;">
                                                                <asp:Label runat="server" ID="lbl_TitRamo" Font-Bold="true" ForeColor="DarkBlue" Text='<%# Eval("LabelRamo")   %>' Width="70px" ></asp:Label>
                                                                <asp:Label runat="server" ID="lbl_Linea" Font-Bold="true" ForeColor="DarkBlue" Text='<%# Eval("cod_ramo_contable")   %>' ></asp:Label>
                                                            </td>
                                                            <td  style="border-top:solid; border-width:1px;"><%# Eval("ramo_contable")%></div></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:150px">
                                                                <asp:Label runat="server" ID="lbl_TitContrato" Font-Bold="true" ForeColor="DarkBlue" Text='<%# Eval("LabelContrato")   %>' Width="70px" ></asp:Label>
                                                                <asp:Label runat="server" ID="lbl_IdContrato" Font-Bold="true" ForeColor="DarkBlue" Text='<%# Eval("id_contrato")   %>' ></asp:Label>
                                                            </td>
                                                            <td  style="border-top:solid; border-width:1px;" ><%# Eval("Contrato")%></div></td>
                                                        </tr>
                                                    </table>
                                        </div>
                                        <asp:Label runat="server" ID="lbl_CancelaPoliza"  Text="" CssClass="form-control PieCentro"  Font-Bold="true" ForeColor="Red"  BackColor="LightBlue"  ></asp:Label>                                                                                            
                                        <asp:Label runat="server" ID="lbl_TitMonto"  Text="Monto a Generar:" CssClass="form-control PieCentro"  Font-Bold="true" ForeColor="Red"  BackColor="LightBlue"  ></asp:Label>                                                                                            
                                        <asp:Textbox runat="server" ID="lbl_Total2" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("Parcial")))  %>' CssClass="form-control PieTotal MontoGenerar"  Font-Bold="true" ForeColor="Red"  BackColor="LightBlue"  ></asp:Textbox>
                                    </ItemTemplate>
                                </asp:TemplateField>        

                                <asp:TemplateField HeaderText="Prima">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl_Espacio1" Text="" CssClass="form-control Espacio"  BorderStyle="None"  ></asp:Label>
                                        <asp:Label runat="server" ID="lbl_DetPrima" Text="Detalle Prima" CssClass="form-control DetPrimaCom" Font-Bold="true"  ForeColor="White" BackColor="#284775"  ></asp:Label>                                                  
                                        <div id="div_prima" runat="server" style="text-align:right;" class="form-control Cantidad" >
                                                <label style="width:40px">Cedida:</label>
                                                <asp:Label runat="server" ID="lbl_PrimaCedida" BackColor="" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaCedida")))  %>' Width="100px" Font-Bold="true" ForeColor="DarkBlue" CssClass="Monto"></asp:Label>
                                                <label style="width:40px">Pagada:</label>
                                                <asp:Label runat="server"  ID="lbl_PrimaPagada" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaPagada")))  %>' Width="100px" Font-Bold="true" ForeColor="DarkBlue" CssClass="Monto"></asp:Label>
                                                <label style="width:150px">------------------------------</label>
                                                <asp:Label runat="server" ID="lbl_SaldoPrima" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("SaldoPrima")))   %>' Width="100px" Font-Bold="true" ForeColor="DarkBlue" CssClass="Monto"></asp:Label>
                                        </div>   
                                                              
                                        <asp:Label runat="server" ID="lbl_PCTotal" Text="Prima Neta de Reaseguro=" CssClass="form-control PieTotal" Font-Bold="true" ForeColor="DarkBlue"   ></asp:Label>       
                                        <asp:Label runat="server" ID="lbl_PCGenerada" Text="Pagos Realizados=" CssClass="form-control PieTotal" Font-Bold="true" ForeColor="DarkBlue"   ></asp:Label>                                            
                                        <asp:Label runat="server" ID="lbl_PCRecuperada" Text="Prima Recuperada=" CssClass="form-control PieTotal" Font-Bold="true" ForeColor="DarkBlue"  ></asp:Label> 
                                        <asp:Label runat="server" ID="lbl_PCRestante" Text="Saldo=" CssClass="form-control PieTotal" Font-Bold="true" ForeColor="DarkBlue"  ></asp:Label> 
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Comisión">
                                    <ItemTemplate>

                                        <asp:Label runat="server" ID="lbl_Espacio2" Text="" CssClass="form-control Espacio"  BorderStyle="None"  ></asp:Label>
                                        <asp:Label runat="server" ID="lbl_DetComision" Text="Detalle Comisión" CssClass="form-control DetPrimaCom" Font-Bold="true"  ForeColor="White" BackColor="#284775"  ></asp:Label>                                                                                
                                        <div id="div_comision" runat="server"  style="text-align:right;" class="form-control Cantidad">
                                                <label style="width:40px">Comisión:</label>
                                                <asp:Label runat="server" ID="lbl_ComCed" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("Comision")))   %>' Width="100px" Font-Bold="true" ForeColor="DarkBlue" CssClass="Monto"></asp:Label>
                                                <label style="width:40px">Aplicada:</label>
                                                <asp:Label runat="server" ID="lbl_ComAplicada" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("ComPagada")))  %>' Width="100px" Font-Bold="true" ForeColor="DarkBlue" CssClass="Monto"></asp:Label>
                                                <label style="width:150px">------------------------------</label>
                                                <asp:Label runat="server" ID="lbl_SaldoCom" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("SaldoComision")))   %>' Width="100px" Font-Bold="true" ForeColor="DarkBlue" CssClass="Monto"></asp:Label>
                                        </div>   
                                                               
                                        <asp:Label runat="server" ID="lbl_MntPCTotal" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaCedida")) - CDbl(Eval("Comision")))  %>' CssClass="form-control PieTotal" Font-Bold="true" ForeColor="Black" BackColor="White" ></asp:Label>  
                                        <asp:Label runat="server" ID="lbl_MntPCGenerada" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaPagada")) - CDbl(Eval("ComPagada")))  %>' CssClass="form-control PieTotal" Font-Bold="true" ForeColor="Green" BackColor="White" ></asp:Label> 
                                        <asp:Label runat="server" ID="lbl_MntPCRecuperada" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaRecuperada")))  %>' CssClass="form-control PieTotal" Font-Bold="true" ForeColor="Green" BackColor="White" ></asp:Label>      
                                        <asp:Label runat="server" ID="lbl_MntPCRestante" Text='<%# String.Format("{0:#,#0.00}", (CDbl(Eval("PrimaCedida")) - CDbl(Eval("Comision"))) - (CDbl(Eval("PrimaPagada")) - CDbl(Eval("ComPagada"))) + CDbl(Eval("PrimaRecuperada")))  %>' CssClass="form-control PieTotal MontoLimite" Font-Bold="true" ForeColor="Red" BackColor="White" BorderColor="Red" ></asp:Label>   
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" CommandName="Detalle" data-toggle="modal" data-target="#ExhibicionesModal" ID="btn_Detalle" ImageUrl="~/Images/icono_detalle.png" AlternateText="Detalle" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                                                                           
                                                                   
                            </Columns>
                            <HeaderStyle CssClass="GridviewScrollHeader" /> 
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle CssClass="gridview" />
                            <PagerSettings   Mode="NumericFirstLast" FirstPageText="Primero" LastPageText="Ultimo" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <EditRowStyle BackColor="#999999" />
                            </asp:GridView> 
                    </asp:Panel>    
                                                    
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


     <div class="clear padding20"></div>
     <div class="clear padding20"></div>
     <div class="clear padding20"></div>


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

    <!-- Modal -->
    <div id="PolizaModal" style="width:1300px; height:650px"  class="modalPoliza">
          <%--<div class="modal-content">--%>
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
                     <h4 class="modal-title"><label id="lblPoliza" style="color:darkblue;">Pólizas</label></h4>
               </div>

               <div class="modal-body" style="height:638px">
                   <asp:UpdatePanel runat="server" ID="upPolizas">
                       <ContentTemplate>

                           <table>
                               <tr>
                                   <td style="padding-left: 15px;">
                                       <asp:CheckBox runat="server" ID="chk_Vencidas" Text="Garantias Programadas" AutoPostBack="true" />
                                       <div class="clear padding5"></div>
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                       <asp:Label runat="server" class="col-md-1 control-label" Width="80px">Fecha</asp:Label>
                                       <asp:TextBox runat="server" ID="txt_FechaIni" CssClass="form-control FechaSB" Width="110px" Height="26px" Enabled="false"></asp:TextBox>
                                   </td>
                                   <td>
                                       <asp:Label runat="server" class="col-md-1 control-label" Width="38px">A</asp:Label>
                                       <asp:TextBox runat="server" ID="txt_FechaFin" CssClass="form-control FechaSB" Width="110px" Height="26px" Enabled="false"></asp:TextBox>
                                   </td>
                               </tr>
                           </table>

                           <div class="clear padding10"></div>

                           <div class="form-group">
                               <div class="input-group">
                                   <asp:Label runat="server" class="col-md-1 control-label" Width="80px">Sucursal</asp:Label>
                                   <asp:DropDownList runat="server" ID="ddl_SucursalPol" CssClass="form-control panelPoliza" Width="370px" Height="28px"></asp:DropDownList>
                               </div>
                           </div>

                           <div class="form-group">
                               <div class="input-group">
                                   <asp:Label runat="server" class="col-md-1 control-label" Width="80px">Ramo</asp:Label>
                                   <asp:TextBox runat="server" ID="txtClaveRam" CssClass="form-control panelPoliza cod_ramo" Width="70px" Height="26px"></asp:TextBox>
                                   <button type="button" id="btn_SelRam" class="btn btn-info panelPoliza" data-toggle="modal" style="width: 36px; height: 26px;" data-target="#EsperaModal">...</button>
                                   <asp:TextBox runat="server" ID="txtSearchRam" CssClass="form-control panelPoliza desc_ramo" Width="262px" Height="26px"></asp:TextBox>
                               </div>
                           </div>

                           <div class="form-group">
                               <div class="input-group">
                                   <asp:Label runat="server" class="col-md-1 control-label" Width="80px">Póliza</asp:Label>
                                   <asp:TextBox runat="server" ID="txt_NoPoliza" CssClass="form-control panelPoliza NroPol" Width="70px" Height="26px"></asp:TextBox>
                                   <asp:Label runat="server" Width="10px"></asp:Label>
                                   <asp:Button runat="server" ID="btn_BuscaEndoso" CssClass="form-control btn-primary" Text="Busca Endosos" Width="150px" Height="26px" />
                                   <asp:Button runat="server" ID="btn_CancelaEndoso" CssClass="form-control btn-danger" Text="Cancelar" Width="150px" Height="26px" />
                                   <%--<asp:ImageButton runat="server" ID="btn_BuscaEndoso" ImageUrl="~/Images/buscaPol-icon.png" />
                                    <asp:ImageButton runat="server" ID="btn_CancelaEndoso" ImageUrl="~/Images/cancelaPol-icon.png" Enabled="false"/>--%>
                               </div>
                           </div>
                           <div style="width: 100%; height: 360px; overflow-y: scroll; overflow-x: scroll;">
                               <asp:GridView runat="server" ID="gvd_GrupoPolizas" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal" DataKeyNames="id_pv,cod_grupo_endo,cod_suc,cod_ramo,nro_pol,aaaa_endoso,nro_endoso" AllowPaging="true" PageSize="13" ShowHeaderWhenEmpty="true">
                                   <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                   <Columns>
                                       <asp:TemplateField HeaderText="">
                                           <ItemTemplate>
                                               <asp:CheckBox runat="server" ID="chk_SelPol" Checked='<%# Eval("tSEL_Val") %>' />
                                               <asp:HiddenField runat="server" ID="hid_IdPv" Value="" />
                                               <asp:TextBox runat="server" CssClass="NoDisplay id_pv" Text='<%# Eval("id_pv") %>' />
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Suc">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="txt_Sucursal" Enabled="false" CssClass="form-control panelPoliza" Text='<%# Eval("cod_suc")   %>' Width="30px" Height="26px"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Ram">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="txt_Ramo" Enabled="false" CssClass="form-control panelPoliza" Text='<%# Eval("cod_ramo")   %>' Width="30px" Height="26px"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Poliza">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="txt_Poliza" Enabled="false" CssClass="form-control panelPoliza" Text='<%# Eval("nro_pol")   %>' Width="70px" Height="26px"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Suf">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="txt_Sufijo" Enabled="false" CssClass="form-control panelPoliza" Text='<%# Eval("aaaa_endoso")   %>' Width="35px" Height="26px"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="End">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="txt_Endoso" Enabled="false" CssClass="form-control panelPoliza" Text='<%# Eval("nro_endoso")   %>' Width="35px" Height="26px"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="">
                                           <ItemTemplate>
                                               <div runat="server" id="div_Ajuste" style="width: 85px;">
                                                   <asp:Label runat="server" ID="lbl_Ajuste" class="col-md-1 control-label" Width="60px" Text="Ajuste: "></asp:Label>
                                                   <asp:TextBox runat="server" ID="txt_Ajuste" Enabled="false" CssClass="form-control panelPoliza" Text='<%# Eval("nro_reas")   %>' Width="20px" Height="26px"></asp:TextBox>
                                               </div>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="">
                                           <ItemTemplate>
                                               <asp:Button runat="server" ID="btn_Aclaracion" CommandName="Aclaracion" Text="Aclaraciones" Width="90px" CssClass="form-control btn btn-primary MuestraAclaracion" Height="26px" Font-Size="10px"  data-toggle="modal" data-target="#EsperaModal"></asp:Button>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="">
                                           <ItemTemplate>
                                               <asp:Button runat="server" ID="btn_Cobranzas" CommandName="Cobranzas" Text="Cobranzas" Width="90px" CssClass="form-control btn btn-primary" Height="26px" Font-Size="10px" ></asp:Button>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Descarta Pago">
                                           <ItemTemplate>
                                               <div style="width: 100px; text-align: center;">
                                                   <asp:CheckBox runat="server" ID="chk_NoPago" AutoPostBack="true" OnCheckedChanged="chk_NoPago_CheckedChanged" Checked='<%# Eval("sn_NoPago") %>' ForeColor="red" CssClass="form-control" Height="26px"></asp:CheckBox>
                                               </div>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Asegurado">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="lbl_Asegurado" Enabled="false" Text='<%# Eval("Asegurado")   %>' Width="200px" CssClass="form-control panelPoliza" Height="26px"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Desde">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="lbl_Desde" Enabled="false" Text='<%# String.Format("{0:dd/MM/yyyy}", CDate(Eval("fec_vig_desde")))  %>' Width="75px" Height="26px" CssClass="form-control panelPoliza"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Hasta">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="lbl_Hasta" Enabled="false" Text='<%# String.Format("{0:dd/MM/yyyy}", CDate(Eval("fec_vig_hasta")))  %>' Width="75px" Height="26px" CssClass="form-control panelPoliza"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Grupo">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="lbl_GrupoEndoso" Enabled="false" Text='<%# Eval("GrupoEndoso")   %>' Width="200px" CssClass="form-control panelPoliza" Height="26px"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Tipo">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="lbl_GrupoTipoEndoso" Enabled="false" Text='<%# Eval("TipoEndoso")   %>' Width="200px" CssClass="form-control panelPoliza" Height="26px"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="(Emisión)">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="lbl_PrimaEmitida" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaEmitida")))  %>' Width="95px" Height="26px" Enabled="false" CssClass="form-control panelPoliza"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="(Cobranzas)">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="lbl_PrimaAplicada" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaAplicada")))  %>' Width="95px" Height="26px" Enabled="false" CssClass="form-control panelPoliza"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="(Reaseguro)">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="lbl_PrimaPagada" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaReaseguro")))  %>' Width="95px" Height="26px" Enabled="false" CssClass="form-control panelPoliza"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Emisor">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="lbl_Emisor" Enabled="false" Text='<%# Eval("Emisor")   %>' Width="170px" CssClass="form-control panelPoliza" Height="26px"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Suscriptor">
                                           <ItemTemplate>
                                               <asp:TextBox runat="server" ID="lbl_Suscriptor" Enabled="false" Text='<%# Eval("Suscriptor")   %>' Width="170px" CssClass="form-control panelPoliza" Height="26px"></asp:TextBox>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                   </Columns>
                                   <PagerStyle CssClass="gridview" />
                                   <PagerSettings Mode="NumericFirstLast" FirstPageText="Primero" LastPageText="Ultimo" />
                               </asp:GridView>
                           </div>


                           <div style="width: 100%; text-align: right;">
                               <asp:Button runat="server" ID="btn_OkPoliza" class="btn btn-success" Text="Agregar" Style="height: 30px; width: 80px;" />
                               <button type="button" id="btn_CnlPoliza" class="btn btn-danger" data-dismiss="modal" style="height: 30px; width: 80px;">Cerrar</button>
                           </div>
                       </ContentTemplate>

                       <%-- <Triggers>
                           <asp:PostBackTrigger ControlID="gvd_GrupoPolizas"></asp:PostBackTrigger>
                       </Triggers>--%>
                   </asp:UpdatePanel>
              </div>
          <%--</div>--%>
    </div>

    <div id="AclaracionesModal" style="width:1300px; height:650px"  class="modalPoliza">
        <asp:UpdatePanel runat="server" ID="upAclaraciones" >
         <ContentTemplate>
            <div class="modal-header" style="height:40px">
                <button type="button" class="close"  data-dismiss="modal">&times;</button>
                <h4 class="modal-title"><label style="color:darkblue;">Aclaraciones</label></h4>
            </div>

            <div class="modal-body" style="height:600px">
                
               <asp:Panel runat="server" Height="550px" Width="1280px" ScrollBars="Both">
                   <asp:Label id="lbl_texto" runat="server" CssClass="Info" ></asp:Label>
               </asp:Panel>
                   
               
               
            <%--<div class="Info" style="height:300px;width:800px; overflow-y:scroll;overflow-x:hidden;"></div>--%>
          <%--<FTB:FreeTextBox id="FTB_Aclaraciones"  AutoGenerateToolbarsFromString="false" runat="server" Height="470px" Width="870px" AllowHtmlMode="true">
            <Toolbars>
                <FTB:Toolbar runat="server">
                    <FTB:ParagraphMenu runat="server" />
                    <FTB:FontSizesMenu runat="server" />
                </FTB:Toolbar>
                <FTB:Toolbar runat="server">
                    <FTB:Bold runat="server" />
                    <FTB:Italic runat="server" />
                    <FTB:Underline runat="server" />
                    <FTB:ToolbarSeparator runat="server" />
                    <FTB:BulletedList runat="server" />
                    <FTB:NumberedList runat="server" />
                </FTB:Toolbar>
                <FTB:Toolbar runat="server">
                    <FTB:InsertHtmlMenu runat="server">
                        <Items>
                            <FTB:ToolbarListItem Text="Cool1" Value="<b>lalala</b>" runat="server" />
                            <FTB:ToolbarListItem Text="Cool2" Value="<i>lalala</i>" runat="server" />
                            <FTB:ToolbarListItem Text="Cool3" Value="<u>lalala</u>" runat="server" />
                        </Items>
                    </FTB:InsertHtmlMenu>
                    <FTB:StylesMenu runat="server">
                        <Items>
                            <FTB:ToolbarListItem Text="Highlighed" Value="<b>Highlighed</b>" runat="server" />
                            <FTB:ToolbarListItem Text="SmallCaps" Value="<i>smallcaps</i>" runat="server" />
                        </Items>
                    </FTB:StylesMenu>
                </FTB:Toolbar>
            </Toolbars>
            </FTB:FreeTextBox>--%>
                <div style="width:100%; text-align:right;">                   
                    <button type="button" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cerrar</button>
                </div>
            </div>
            
         </ContentTemplate>
       </asp:UpdatePanel>
    </div>
       
    <div id="ExhibicionesModal" style="width:920px; height:620px"  class="modalExhibiciones">
      <asp:UpdatePanel runat="server" ID="upExhibiciones">
         <ContentTemplate>
          <%--<div class="modal-content">--%>
               <div class="modal-header" style="height:40px">
                     <button type="button" class="close"  data-dismiss="modal">&times;</button>
                             <h4 class="modal-title"><label id="lblExhibiciones" style="color:darkblue;">Detalle Exhibiciones</label>
                                  <label style="width:150px"></label>
                             </h4>
               </div>
               
               <asp:HiddenField runat="server" ID="hid_Moneda" Value="0" />
               <asp:HiddenField runat="server" ID="hid_TipoCambio" Value="0" />
               <asp:HiddenField runat="server" ID="hid_Cancelada" Value="0" />

               <div class="modal-body" style="height:608px">

                    <div class="row"> 
                            <div class="col-md-3">
                                        <asp:HiddenField runat="server" ID="hid_nroreas" Value="0" />
                                        <asp:HiddenField runat="server" ID="hid_PrimaReaseguro" Value="0" />
                                        <asp:label runat="server" class="col-md-1 control-label" ForeColor="DarkBlue" Width="70px">Póliza</asp:label>
                                        <asp:TextBox runat="server" ID="txt_Poliza" Width="110px"  CssClass="form-control" Height="24px" Enabled="false" ></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                         <asp:label runat="server" class="col-md-1 control-label" ForeColor="DarkBlue"  Width="60px">Capa</asp:label>
                                         <asp:TextBox runat="server" ID="txt_Capa" CssClass="form-control" Width="30px" Height="24px" Enabled="false"></asp:TextBox>
                            </div>
                          
                            <div class="col-md-4">
                                        <asp:label runat="server" class="col-md-1 control-label" ForeColor="DarkBlue"  Width="60px">Ramo</asp:label>
                                        <asp:TextBox runat="server" ID="txt_Ramo" CssClass="form-control" Width="200px" Height="24px"  Enabled="false"></asp:TextBox>
                            </div>
                              
                             <div class="col-md-3">
                                        <asp:label runat="server" class="col-md-1 control-label" ForeColor="DarkBlue"  Width="80px">Contrato</asp:label>
                                        <asp:TextBox runat="server" ID="txt_Contrato" CssClass="form-control" Width="100px" Height="24px"  Enabled="false"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hid_nrotramo" Value="0" />
                            </div>   
             
                    </div>

                   <div class="clear padding5"></div>

                    <div class="row"> 
                        <div class="col-md-3">
                            <asp:label runat="server" class="col-md-1 control-label" ForeColor="DarkBlue"  Width="70px">Total</asp:label>
                            <asp:TextBox runat="server" ID="txt_MontoTotal" CssClass="form-control Derecha" Height="24px" Width="110px"  Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-md-4" style="text-align:right;">
                            <asp:CheckBox runat="server" ID="chk_AplicaTodos" CssClass="LetraDetalle" Text="Aplicar cambios a todas las exhibiciones" />
                        </div>
                        <div class="col-md-2">
                        </div>
                        <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txt_MontoFraccionado" CssClass="col-md-1 form-control Derecha Fraccionado" Height="24px" Width="105px" ></asp:TextBox>
                                <asp:Button runat="server" ID="btn_Fraccionar" CssClass="btn btn-primary" Height="24px" Font-Size="11px" Text="Fraccionar" />
                        </div>
                    </div>
                    <div class="clear padding5"></div>

                    <div  style="width:100%; height:430px; overflow-y: scroll;  ">
                        <asp:GridView runat="server" ID="gvd_CiasXBroker" GridLines="Horizontal" BorderStyle="None" AutoGenerateColumns="false" CssClass="LetraDetalleGrid"  DataKeyNames="cod_cia_reas_brok,Broker,cod_cia_reas_cia,Compañia,PrimaCedidaCiaBrok,ComisionCiaBrok,nro_reas,nro_tramo,tipo_ISR,ret_ISR,sn_Comp_RF">
                           
                            <Columns>
                                <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                    <ItemTemplate>
                                        <input type="image" style="width:15px; height:15px;"  src="../Images/collapse.png" class="BotonCierra"  />
                                        <input type="image" style="width:15px; height:15px;"  src="../Images/expand.png"   class="BotonAbre" />
                                        <asp:TextBox runat="server" ID="hid_Marco" CssClass="NoDisplay EdoOculta" Text="0"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>     

                                
                                <asp:TemplateField HeaderText="Broker / Compañia" ItemStyle-VerticalAlign="Top">
                                    <ItemTemplate>
                                        <div class="MarcoCompañias" style="display:none;">

                                            <div class="form-control DescripcionDet">
                                                <label style="width:60px; font-weight:bold;">Broker:</label>
                                                <asp:Label runat="server" ID="lbl_IdBrokerOculto" Font-Bold="true" ForeColor="DarkBlue" Text='<%# Eval("cod_cia_reas_brok")   %>' Width="20px" ></asp:Label>
                                                <%# Eval("Broker")%>
                                            </div>
                                                    
                                            <div class="form-control DescripcionDet"  >
                                                <label style="width:60px; font-weight:bold;">Compañia:</label>
                                                <asp:Label runat="server" ID="lbl_IdCiaOculto" Font-Bold="true" ForeColor="DarkBlue"  Text='<%# Eval("cod_cia_reas_cia")   %>' Width="20px" ></asp:Label>
                                                <%# Eval("Compañia")%>
                                            </div>
                                        </div>

                                        <div class="MarcoExhibiciones">
                                            <div class="form-control DescripcionDet">
                                                <label style="width:60px; font-weight:bold;">Broker:</label>
                                                <asp:Label runat="server" ID="lbl_IdBroker" Font-Bold="true" ForeColor="DarkBlue" Text='<%# Eval("cod_cia_reas_brok")   %>' Width="20px" ></asp:Label>
                                                <%# Eval("Broker")%>
                                            </div>
                                                    
                                            <div class="form-control DescripcionDet"  >
                                                <label style="width:60px; font-weight:bold;">Compañia:</label>
                                                <asp:Label runat="server" ID="lbl_IdCia" Font-Bold="true" ForeColor="DarkBlue" CssClass="IdCia" Text='<%# Eval("cod_cia_reas_cia")   %>' Width="20px" ></asp:Label>
                                                <%# Eval("Compañia")%>
                                            </div>
                                            
                                            <div class="clear padding5"></div>

                                            
                                            <div class="form-control DescripcionReaDet"  >
                                                <label style="font-weight:bold;">Tipo ISR:</label>
                                            </div>

                                             <div class="form-control DescripcionReaDet"  >
                                                <label style="font-weight:bold;">Retención ISR (%):</label>
                                            </div>

                                            <div class="form-control DescripcionReaDet"  >
                                                <label style="font-weight:bold;">(%) Imp. Reasegurador:</label>
                                            </div>

                                            <div class="form-control DescripcionReaDet"  >
                                                <label style="font-weight:bold;">Monto a Retención(ISR):</label>
                                            </div>

                                            <div class="clear padding5"></div>

                                            <div class="form-control DescripcionReaDet"  >
                                                <label style="font-weight:bold;">Prima Neta de Reaseguro:</label>
                                            </div>

                                             <div class="form-control DescripcionReaDet"  >
                                                <label style="font-weight:bold;">Pagos Realizados:</label>
                                            </div>

                                            <div class="form-control DescripcionReaDet"  >
                                                <label style="font-weight:bold;">Saldo:</label>
                                            </div>

                                            <div class="form-control DescripcionReaDet"  >
                                                <label style="font-weight:bold;">Monto Temporal a Generar:</label>
                                            </div>

                                            <div class="clear padding5"></div>

                                            <div style="vertical-align:central; height:22px;"">
                                                <table>
                                                    <tr>
                                                        <td style="width:32px;">
                                                       
                                                        </td>
                                                        <td style="width:27px;">
                                                            
                                                        </td>
                                                        <td style="width:85px; text-align:center;">
                                                            <label class="form-control" style="height:20px; background-color:lightblue;">Generada</label>
                                                        </td>
                                                        <td style="width:85px; text-align:center;"">
                                                            <label class="form-control" style="height:20px; background-color:lightblue;">Pendiente</label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            
                                            <div class="VentanaResumen">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <label style="width:60px; font-weight:bold;">% Prima:</label>
                                                        </td>
                                                        <td>
                                                            <asp:Textbox runat="server" ID="txt_PrcPriGen"  Text="0"  Width="85px" CssClass="form-control Parcial PrcPriGen" ReadOnly="true"   ></asp:Textbox>
                                                        </td>
                                                         <td>
                                                             <asp:Textbox runat="server" ID="txt_PrcPriPend" Text="0"  Width="85px" CssClass="form-control Parcial PrcPriPend" ReadOnly="true"  ></asp:Textbox>
                                                    
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label style="width:60px; font-weight:bold;">Prima:</label>
                                                        </td>
                                                        <td>
                                                            <asp:Textbox runat="server" ID="txt_PriGen" Text="0"  Width="85px" CssClass="form-control Parcial PrimaGen" ReadOnly="true"  ></asp:Textbox>
                                                        </td>
                                                         <td>
                                                            <asp:Textbox runat="server" ID="txt_PriPend" Text="0"  Width="85px" CssClass="form-control Parcial PrimaPend" ReadOnly="true"  ></asp:Textbox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label style="width:60px; font-weight:bold;">% Comisión</label>
                                                        </td>
                                                        <td>
                                                            <asp:Textbox runat="server" ID="txt_PrcComGen" Text="0"  Width="85px" CssClass="form-control Parcial PrcComGen" ReadOnly="true"  ></asp:Textbox>
                                                        </td>
                                                         <td>
                                                            <asp:Textbox runat="server" ID="txt_PrcComPend" Text="0"  Width="85px" CssClass="form-control Parcial PrcComPend" ReadOnly="true"  ></asp:Textbox>
                                                        </td>
                                                    </tr>
                                            
                                                    <tr>
                                                        <td>
                                                            <label style="width:60px; font-weight:bold;">Comisión:</label>
                                                        </td>
                                                        <td>
                                                            <asp:Textbox runat="server" ID="txt_ComGen" Text="0"  Width="85px" CssClass="form-control Parcial ComGen"  ReadOnly="true" ></asp:Textbox>
                                                        </td>
                                                         <td>
                                                            <asp:Textbox runat="server" ID="txt_ComPend" Text="0"  Width="85px" CssClass="form-control Parcial ComPend" ReadOnly="true"  ></asp:Textbox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>    
                                    </ItemTemplate>
                                </asp:TemplateField>

                                
                                <asp:TemplateField HeaderText="Prima / Comisión" ItemStyle-VerticalAlign="Top">
                                    
                                    <ItemTemplate>
                          
                                       <div class="MarcoExhibiciones">
                                            <div class="form-control DescripcionMnt">
                                                <label style="width:90px; font-weight:bold;">Prima Cedida:</label>
                                                <%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaCedidaCiaBrok"))) %>
                                                <asp:textbox runat="server" ID="hid_Prima" Text='<%# Eval("PrimaCedidaCiaBrok")  %>' CssClass="NoDisplay PrimaCedidaEmitida" ></asp:textbox>
                                            </div>
                                                    
                                            <div class="form-control DescripcionMnt">
                                                <label style="width:90px; font-weight:bold;">Comisión:</label>
                                                <%# String.Format("{0:#,#0.00}", CDbl(Eval("ComisionCiaBrok"))) %>
                                                <asp:textbox runat="server" ID="hid_Comision" Text='<%# Eval("ComisionCiaBrok")  %>' CssClass="NoDisplay ComisionCedidaEmitida" ></asp:textbox>
                                            </div>

                                            <div class="clear padding5"></div>

                                            <div class="form-control DescripcionReaMnt">
                                                <asp:Label ID="lbl_TipoISR" runat="server" Text='<%# Eval("tipo_ISR") %>' ></asp:Label>
                                            </div>

                                            <div class="form-control DescripcionReaMnt">
                                                <asp:Label ID="lbl_RetISR" runat="server"  Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("ret_ISR"))) %>'  ></asp:Label>
                                            </div>

                                            <div class="form-control DescripcionReaMnt">
                                                <asp:Label ID="lbl_CompRF" runat="server" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("sn_Comp_RF"))) %>' ></asp:Label>
                                            </div>

                                            <div class="form-control DescripcionReaMnt">
                                                <asp:Label ID="lbl_MontoRet" runat="server" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaCedidaCiaBrok")) * (CDbl(Eval("ret_ISR")) / 100)) %>' ></asp:Label>
                                            </div>

                                           <div class="clear padding5"></div>

                                            <div class="form-control DescripcionReaMnt">
                                                <asp:Label ID="lbl_PriReaseguro" runat="server" CssClass="PriReaTot" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaCedidaCiaBrok")) - CDbl(Eval("ComisionCiaBrok"))) %>' ></asp:Label>
                                            </div>

                                            <div class="form-control DescripcionReaMnt">
                                                <asp:Label ID="lbl_PriPagada" runat="server" CssClass="PriReaPag" ></asp:Label>
                                            </div>

                                            <div class="form-control DescripcionReaMnt">
                                                <asp:Label ID="lbl_SaldoPri" runat="server" CssClass="PriReaSaldo" ></asp:Label>
                                            </div>

                                            <div class="form-control DescripcionReaMnt">
                                                <asp:Label ID="lbl_MntGenerar" runat="server" CssClass="MntGenerar" ></asp:Label>
                                            </div>

                                            <div class="clear padding5"></div>

                                            <div style="text-align:center; height:22px;">
                                                <label class="form-control" style="width:90px; height:20px; background-color:lightblue;">Totales</label>
                                            </div>

                                            <div class="VentanaResumen">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Textbox runat="server" ID="txt_PrcPriTotal" Text="0"  Width="90px" CssClass="form-control Total PrcPriTotal"  ReadOnly="true" ></asp:Textbox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Textbox runat="server" ID="txt_PrimaTotal" Text="0"  Width="90px" CssClass="form-control Total PrimaTotal"  ReadOnly="true" ></asp:Textbox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Textbox runat="server" ID="txt_PrcComTotal" Text="0"  Width="90px" CssClass="form-control Total PrcComTotal" ReadOnly="true"  ></asp:Textbox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Textbox runat="server" ID="txt_ComTotal" Text="0"  Width="90px" CssClass="form-control Total ComTotal"  ReadOnly="true" ></asp:Textbox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>    
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Cuotas" ItemStyle-VerticalAlign="Top">
                                    <ItemTemplate>
                                        <div class="MarcoExhibiciones">
                                            <div  style="width:100%; ">
                                                <asp:GridView runat="server" ID="gvd_Cuotas" GridLines="None" AutoGenerateColumns="false" CssClass="LetraDetalleGrid gvd_Cuotas" BorderStyle="None"  
                                                                DataKeyNames="sn_Seleccion,PrimaCedidaCiaBrok,ComisionCiaBrok,PrimaCedida,Comision,cod_moneda,Moneda,nro_cuota_reas,nro_cuota,nro_subcuota,fecha,pje_pri,pje_com,blnPendiente,nro_op,cod_estatus_op,fec_pago,fec_baja,sn_Origen,cod_cptoPri,ConceptoPrima,cod_cptoCom,ConceptoComision,imp_cambio,sn_cambio,Version"
                                                                OnRowDataBound="gvd_Cuotas_RowDataBound" OnRowCommand="gvd_Cuotas_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                    <asp:CheckBox runat="server" ID="chk_SelCuota" CssClass="SelectCuota" Checked='<%# Eval("sn_Seleccion") %>' />
                                                                    <asp:textbox runat="server" ID="hid_Cambio" Text='<%# Eval("sn_cambio")  %>' CssClass="NoDisplay Cambio" ></asp:textbox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField >
                                                        <asp:TemplateField HeaderText="# Exh." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lbl_Cuota" Text='<%# Eval("nro_cuota_reas")%>' CommandName="Duplicar" ForeColor="DarkBlue" Font-Bold="true" Font-Underline="true" CssClass="form-control CuotaReas DetExh"></asp:LinkButton>
                                                                <asp:textbox runat="server" ID="txt_Cuota" Text='<%# Eval("nro_cuota_reas")  %>'  CssClass="form-control DetExh Cuota CambioCuota" ></asp:textbox>
                                                                <asp:textbox runat="server" ID="hid_Cuota" Text='<%# Eval("nro_cuota")  %>'  CssClass="NoDisplay nro_cuota_ori" ></asp:textbox>
                                                                <asp:textbox runat="server" ID="hidSubcuota" Text='<%# Eval("nro_subcuota")  %>' CssClass="NoDisplay nro_cuota_subori" ></asp:textbox>
                                                                <asp:textbox runat="server" ID="hid_Version" Text='<%# Eval("Version")  %>' CssClass="NoDisplay Version" ></asp:textbox>
                                                                <asp:HiddenField runat="server" ID="hid_subcuota" Value='<%# Eval("nro_subcuota")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="% Pri." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Textbox runat="server" ID="lbl_prcPri"  Text='<%# String.Format("{0:#,#0.0000}", CDbl(Eval("pje_pri")))  %>' Width="50px" CssClass="form-control DetExh PrcPrima masterTooltip" title='<%# Eval("ConceptoPrima") & vbCrLf & "(Doble Click para cambiar Concepto Prima)" %>'></asp:Textbox>
                                                                <asp:textbox runat="server" ID="hid_Prima" Text='<%# Eval("PrimaCedidaCiaBrok")  %>' CssClass="NoDisplay Prima" ></asp:textbox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Prima Cedida" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Textbox runat="server" ID="lbl_Prima" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("PrimaCedida")))  %>'  Width="90px" CssClass="form-control DetExh PagoPrima masterTooltip"  title='<%# Eval("ConceptoPrima") & vbCrLf & "(Doble Click para cambiar Concepto Prima)" %>' ></asp:Textbox>
                                                                <asp:HiddenField runat="server" ID="hid_codcptoPri" Value='<%# Eval("cod_cptoPri")%>' />
                                                                <asp:HiddenField runat="server" ID="hid_ConceptoPrima" Value='<%# Eval("ConceptoPrima")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="% Com." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:textbox runat="server" ID="lbl_prcCom"  Text='<%# String.Format("{0:#,#0.0000}", CDbl(Eval("pje_com")))  %>' Width="50px" CssClass="form-control DetExh PrcCom masterTooltip" title='<%# Eval("ConceptoComision") & vbCrLf & "(Doble Click para cambiar Concepto Comisión)" %>' ></asp:textbox>
                                                                <asp:textbox runat="server" ID="hid_Comision" Text='<%# Eval("ComisionCiaBrok")  %>' CssClass="NoDisplay Comision" ></asp:textbox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Comisión" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Textbox runat="server" ID="lbl_Comision" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("Comision")))  %>'  Width="82px" CssClass="form-control DetExh PagoComision masterTooltip" title='<%# Eval("ConceptoComision") & vbCrLf & "(Doble Click para cambiar Concepto Comisión)" %>' ></asp:Textbox>
                                                                <asp:HiddenField runat="server" ID="hid_codcptoCom" Value='<%# Eval("cod_cptoCom")%>' />
                                                                <asp:HiddenField runat="server" ID="hid_ConceptoComision" Value='<%# Eval("ConceptoComision")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                         <asp:TemplateField HeaderText="T.C." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:textbox runat="server" ID="lbl_TipoCambio"  Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_cambio")))  %>' Width="40px" Enabled="false" CssClass="form-control DetExh masterTooltip" title='<%# Eval("Moneda") %>' ></asp:textbox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Fec Garantia" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:textbox runat="server" ID="lbl_Fecha" Text='<%# String.Format("{0:dd/MM/yyyy}", CDate(Eval("fecha")))  %>' Width="75px" CssClass="form-control DetExh FechaSB CambioFecha"></asp:textbox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Orden Pago" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lbl_OrdenPago" Text='<%# Eval("nro_op")%>' CssClass="form-control DetExh Link"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                         <asp:TemplateField ItemStyle-VerticalAlign="Middle" >
                                                            <ItemTemplate>
                                                               <asp:ImageButton runat="server" ID="btn_BorrarCuota" ImageUrl="~/Images/collapse.png" CommandName="QUitar" Height="22px" />
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
                                            <div style="width:100%; text-align:right;">
                                                <asp:Button ID="btn_AñadirCuota" CommandName="AñadirCuota" runat="server" CssClass="btn btn-primary" Font-Size="11px"  Text="Agregar Cuota" Height="24px" />
                                            </div>
                                            
                                        </div>
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
 
                    <div class="row">        
                        <div class="col-md-10" style="border:1px solid gray; border-width: 1px 0 0 0; padding: 5px 0 0 0">
                            <div class="form-group">
                                <div class="input-group">
                                        <asp:Button runat="server" ID="btn_Guardar" Text="Guardar"  CssClass="btn btn-success" Width="140px" />
                                        <asp:Button runat="server" ID="btn_GenerarOP" Text="Generar Orden" data-toggle="modal" data-target="#OrdenesModal"  CssClass="btn btn-info" Width="140px" />
                                        <asp:Button runat="server" ID="btn_VistaPrevia" Text="Vista Previa"  CssClass="btn btn-primary" Width="140px" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2" style="border:1px solid gray; border-width: 1px 0 0 0; padding: 5px 0 0 0">
                            <div class="form-group">
                                <div class="input-group">
                                        <button type="button" class="btn btn-danger" data-dismiss="modal" style="width:140px;">Cerrar</button>
                                       <%-- <asp:Button runat="server" ID="btn_CerrarExhibiciones" class="btn btn-danger" Width="140px" Text="Cerrar" />--%>
                                </div>
                            </div>
                        </div>
                    </div>
              </div>
          <%--</div>--%>
        </ContentTemplate>
      </asp:UpdatePanel>
    </div>

    <!-- Modal -->
    <div id="OrdenesModal" style="width:920px; height:610px"  class="modalOrdenPago">
        <%--<div class="modal-content">--%>
            <div class="modal-header" style="height:40px">
                <button type="button" class="close"  data-dismiss="modal">&times;</button>
                <h4 class="modal-title"><label id="lblOrdenes" style="color:darkblue;">Órdenes de Pago a Generar</label></h4>
            </div>

            <div class="modal-body" style="height:568px">
                <asp:UpdatePanel runat="server" ID="upOrdenesPago">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:label ID="lbl_Genera" runat="server" class="col-md-1 control-label" Width="120px">Generar por: </asp:label>
                                    <asp:DropDownList runat="server" ID="ddl_TipoGenera" CssClass="form-control" Font-Size="12px"  Width="140px" AutoPostBack="true"  Height="26px">
                                        <asp:ListItem Text="Endoso" Value="EN" Selected="True" />
                                        <asp:ListItem Text="Grupo de Endosos" Value="GE" />
                                        <asp:ListItem Text="Grupo de Pólizas" Value="GP" />
                                    </asp:DropDownList> 
                                </td>
                                <td>
                                    <asp:label runat="server" ID="lbl_Cuotas" class="col-md-1 control-label" Width="90px" Visible="false">Cuotas</asp:label>
                                    <asp:DropDownList runat="server" ID="ddl_Cuotas" CssClass="form-control" Width="100px" Visible="false" Height="26px"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btn_EvaluaOP" Text="Evaluar" CssClass="form-control btn-primary" Visible="false" Width="90px" Height="26px" />
                                </td>
                                <td>
                                    <asp:label runat="server" ID="lbl_Parcial" class="col-md-1 control-label" Width="100px">Monto = </asp:label>
                                    <asp:label runat="server" ID="lbl_MntParcial" Text="" class="control-label" Font-Bold="true" ForeColor="DarkBlue" BackColor="LightBlue"/>
                                </td>
                                <td>
                                    <asp:label runat="server" ID="lbl_Impuesto" class="col-md-1 control-label" Width="100px">Impuesto = </asp:label>
                                    <asp:label runat="server" ID="lbl_MntImpuesto" Text="" class="control-label" Font-Bold="true" ForeColor="DarkBlue" BackColor="LightBlue"/>
                                </td>
                                 <td>
                                    <asp:label runat="server" ID="lbl_Total" class="col-md-1 control-label" Width="130px">Total a Pagar = </asp:label>
                                    <asp:label runat="server" ID="lbl_MntTotal" Text="" class="control-label" Font-Bold="true" ForeColor="DarkBlue" BackColor="LightBlue"/>
                                </td>
                            </tr>
                        </table>
                      
                        <div class="clear padding10"></div>

                        <asp:Panel runat="server" ID="pnlOrdenPago" Width="100%" Height="450px" ScrollBars="Vertical">
                            <asp:GridView runat="server" ID="gvd_OrdenPago" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true"
                                          DataKeyNames="Banco,TipoDeCuenta,Cuenta,Moneda,id_Cuenta,cod_banco,cod_moneda,txt_swift,cod_tipo_banco,id_pv,cod_cia_reas_brok"  >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333"  />
                                <Columns>
                                    <asp:TemplateField HeaderText="No.">
                                        <ItemTemplate>
                                            <asp:textbox runat="server" ID="lbl_Nro" Text='<%# Eval("Nro") %>' Width="30px" Height="80px"  CssClass="form-control" Font-Bold="true" Enabled="false" Font-Size="10px" ></asp:textbox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Póliza">
                                        <ItemTemplate>
                                            <asp:textbox runat="server" ID="lbl_Poliza" Text='<%# Eval("Poliza") %>' Width="100px" Height="80px"  CssClass="form-control" TextMode="MultiLine" Enabled="false" Font-Size="10px" ></asp:textbox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Broker">
                                        <ItemTemplate>
                                            <asp:textbox runat="server" ID="lbl_Broker" Text='<%# Eval("Broker") %>' Width="200px" Height="80px" CssClass="form-control" Enabled="false" Font-Size="10px" TextMode="MultiLine"></asp:textbox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Monto">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txt_Monto"  Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("Parcial")))  %>' Width="80px" Height="80px" Enabled="false"  CssClass="form-control Derecha" Font-Size="11px" Font-Bold="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Impuesto">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txt_Impuesto" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("Impuesto")))  %>' Width="80px" Height="80px" Enabled="false"  CssClass="form-control Derecha" Font-Size="11px" Font-Bold="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fec Pago">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txt_FechaPago" Text='<%# String.Format("{0:dd/MM/yyyy}", CDate(Eval("FechaPago"))) %>' Width="80px" Height="80px"  CssClass="form-control FechaSB" Font-Size="11px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo Pago" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            
                                             <asp:textbox runat="server" ID="hid_Moneda" Text='<%# Eval("cod_moneda")  %>' CssClass="NoDisplay Moneda" ></asp:textbox>
                                             <asp:textbox runat="server" ID="hid_Persona" Text='<%# Eval("id_persona")  %>' CssClass="NoDisplay Persona" ></asp:textbox>
                                             <asp:HiddenField runat="server" ID="hid_cod_banco" Value='<%# Eval("cod_banco")  %>' />
                                             <asp:HiddenField runat="server" ID="hid_id_cuenta" Value='<%# Eval("id_Cuenta")  %>' />
                                             <asp:HiddenField runat="server" ID="hid_nro_cuenta" Value='<%# Eval("Cuenta")  %>' />


                                            <div class="input-group">
                                                <asp:RadioButtonList runat="server" SelectedValue='<%# Eval("TipoPago") %>' ID="opt_TipoPago" Width="200px" Height="28px" RepeatDirection="Horizontal" CssClass="form-control">
                                                    <asp:ListItem Text="Cheque" Value="C"></asp:ListItem>
                                                    <asp:ListItem Text="Transferencia" Value="T"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:textbox runat="server" ID="lbl_Banco" Text='<%# "Banco: " & Eval("Banco")   %>' Width="163px" Height="25px"  CssClass="form-control" Enabled="false" Font-Size="10px" ></asp:textbox>
                                                <asp:Button runat="server" ID="btn_Cuentas" Text=".." CssClass ="btn btn-primary btnCuenta" Height="25px"/>
                                                <asp:textbox runat="server" ID="lbl_Cuenta" Text='<%# "Cuenta: " & Eval("Cuenta")   %>' Width="200px" Height="25px"  CssClass="form-control Cuenta" Enabled="false" Font-Size="10px" ></asp:textbox>
                                            </div>

                                            <%--<asp:Button runat="server" ID="btn_CtasBancarias" data-toggle="popover" title="Cuenta Bancaria" data-trigger="focus" data-content='<%# "Banco: " & Eval("Banco") & vbCrLf & "Cuenta: " & Eval("Cuenta")  %>' Text="Cuentas"  CssClass="btn btn-info Cuentas" Height="30px" Width="100px" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Detalle de Pago">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txt_texto" Text='<%# Eval("Texto") %>' Width="355px" TextMode="MultiLine" Height="80px" CssClass="form-control" Font-Size="10px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#5D7B9D" ForeColor="White" />
                                <HeaderStyle BackColor="#5D7B9D" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                        </asp:Panel>

                        <div class="row">        
                            <div class="col-md-10" style="border:1px solid gray; border-width: 1px 0 0 0; padding: 5px 0 0 0">
                                <asp:Button runat="server" ID="btn_ConfirmarOP" Text="Generar"  CssClass="btn btn-success" Width="140px" />
                            </div>
                            <div class="col-md-2" style="border:1px solid gray; border-width: 1px 0 0 0; padding: 5px 0 0 0">
                                 <button type="button" class="btn btn-danger" data-dismiss="modal" style="width:140px;">Cerrar</button>
                            </div>
                        </div>

                      
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        <%--</div>--%>
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


    <!-- Modal -->
    <div id="EndososModal" style="width:920px; height:610px"  class="modalOrdenPago">
            <div class="modal-header" style="height:40px">
                <h4 class="modal-title"><label id="lblEndosos" style="color:darkblue;">Listado de Endosos</label></h4>
            </div>

            <div class="modal-body" style="height:398px">
                   <asp:UpdatePanel runat="server" ID="up_Endosos">
                       <ContentTemplate>

                          <div class="input-group">
                            <asp:label runat="server" class="col-md-1 control-label" Width="90px">Moneda:</asp:label>
                            <asp:DropDownList runat="server" ID="ddl_MonedaEnd" CssClass="form-control" Width="300px" Height="26px" AutoPostBack="true"></asp:DropDownList>
                          </div>  

                          <div class="clear padding10"></div>

                          <asp:Panel runat="server" ID="pnlEndosos" Width="100%" Height="470px" ScrollBars="Vertical">
                              <asp:GridView runat="server" ID="gvd_Endosos" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                       <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chk_SelPol" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Endoso" ItemStyle-CssClass="ClaveCia">
                                            <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Poliza" Text='<%# Eval("Poliza") %>' Width="100px" Font-Size="9px"></asp:label>
                                                <asp:HiddenField runat="server" ID="hid_idpv" Value='<%# Eval("id_pv") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Asegurado">
                                            <ItemTemplate>
                                                    <asp:textbox runat="server" ID="lbl_Asegurado" Enabled="false" Text='<%# Eval("Asegurado")   %>' Width="300px" CssClass="form-control" Font-Size="9px" Height="26px" ></asp:textbox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Grupo">
                                            <ItemTemplate>
                                                    <asp:textbox runat="server" ID="lbl_GrupoEndoso" Enabled="false" Text='<%# Eval("GrupoEndoso")   %>' Width="220px" CssClass="form-control" Font-Size="9px" Height="26px" ></asp:textbox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Tipo">
                                            <ItemTemplate>
                                                    <asp:textbox runat="server" ID="lbl_GrupoTipoEndoso" Enabled="false" Text='<%# Eval("TipoEndoso")   %>' Width="220px" CssClass="form-control" Font-Size="9px" Height="26px" ></asp:textbox>
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
                             <asp:Button runat="server" id="btn_OkEndosos" class="btn btn-success" Text="Evaluar"  style="height:30px; width:80px;" />
                             <asp:Button runat="server" id="btn_CnlEndosos" class="btn btn-danger" Text="Cancelar"  style="height:30px; width:80px;" />
                          </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
    </div>

    <!-- Modal -->
    <div id="DescartadasModal" style="width:920px; height:400px"  class="modalOrdenPago">
            <button type="button" class="close"  data-dismiss="modal">&times;</button>
            <div class="modal-header" style="height:40px">
                <h4 class="modal-title"><label style="color:darkblue;">Pólizas no sujetas a Pago</label></h4>
            </div>

            <div class="modal-body" style="height:398px">
                   <asp:UpdatePanel runat="server" ID="upDescartadas">
                       <ContentTemplate>

                          <asp:Panel runat="server" ID="pnlDescartadas" Width="100%" Height="300px" ScrollBars="Both">
                              <asp:GridView runat="server" ID="gvd_Descartadas" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                       <asp:TemplateField HeaderText="" ItemStyle-CssClass="SelCia">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chk_SelPol" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Endoso" ItemStyle-CssClass="ClaveCia">
                                            <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Poliza" Text='<%# Eval("Poliza") %>' Width="100px" Font-Size="10px"></asp:label>
                                                <asp:HiddenField runat="server" ID="hid_idpv" Value='<%# Eval("id_pv") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Asegurado">
                                            <ItemTemplate>
                                                    <asp:textbox runat="server" ID="lbl_Asegurado" Enabled="false" Text='<%# Eval("Asegurado")   %>' Width="300px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:textbox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Grupo">
                                            <ItemTemplate>
                                                    <asp:textbox runat="server" ID="lbl_GrupoEndoso" Enabled="false" Text='<%# Eval("GrupoEndoso")   %>' Width="220px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:textbox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Tipo">
                                            <ItemTemplate>
                                                    <asp:textbox runat="server" ID="lbl_GrupoTipoEndoso" Enabled="false" Text='<%# Eval("TipoEndoso")   %>' Width="220px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:textbox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Usuario">
                                            <ItemTemplate>
                                                    <asp:textbox runat="server" ID="lbl_Usuario" Text='<%# Eval("Usuario")   %>' Enabled="false" Width="220px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:textbox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Fecha">
                                            <ItemTemplate>
                                                    <asp:label runat="server" ID="lbl_Fecha" Text='<%# Eval("Fecha")   %>' Width="80px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
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
                             <asp:Button runat="server" id="btn_QuitarNoPago" class="btn btn-success" Text="Quitar"  style="height:30px; width:80px;" />
                             <asp:Button runat="server" id="btn_CerrarNoPago" class="btn btn-danger" Text="Cerrar" data-dismiss="modal"  style="height:30px; width:80px;" />
                          </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
    </div>
   
    <div id="CobranzasModal" style="width:1300px; height:650px"  class="modalPoliza">
        <button type="button" class="close"  data-dismiss="modal">&times;</button>
        <div class="modal-header" style="height:40px">
             <asp:UpdatePanel runat="server" ID="upTitulo">
                <ContentTemplate>
                <h4 class="modal-title"><asp:Label runat="server" ID="lbl_PolizaCobranzas" Font-Bold="true" ForeColor="DarkBlue"></asp:Label></h4>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>


        <div class="modal-body" style="height:598px">
            <asp:UpdatePanel runat="server" ID="upCobranzas">
                <ContentTemplate>
                    <asp:Label runat="server" Text="Pagadores:" Font-Bold="true" Font-Size ="12px" ForeColor="DarkBlue"></asp:Label>
                    <asp:Panel runat="server" ID="pnlPagadores" Width="100%" Height="120px" ScrollBars="Both">
                        <asp:GridView runat="server" ID="gvd_Pagadores" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" DataKeyNames="id_pv,cod_aseg,cod_ind_pagador" >
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <Columns>
                                    <asp:TemplateField HeaderText="Codigo" ItemStyle-CssClass="ClaveCia">
                                        <ItemTemplate>
                                            <asp:linkbutton runat="server" ID="lnk_Codigo" CommandName="DetallePagador" Text='<%# Eval("cod_aseg") %>' Width="60px" Font-Size="10px" Font-Bold="true" CssClass="Centro"></asp:linkbutton>
                                            <asp:hiddenfield runat="server" ID="hid_indPagador" value='<%# Eval("cod_ind_pagador") %>'></asp:hiddenfield>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Apellidos y Nombre">
                                        <ItemTemplate>
                                                <asp:textbox runat="server" ID="lbl_Pagador" Enabled="false" Text='<%# Eval("pagador")   %>' Width="300px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:textbox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Medios de Pago">
                                        <ItemTemplate>
                                                <asp:textbox runat="server" ID="lbl_Medio" Enabled="false" Text='<%# Eval("txt_desc")   %>' Width="140px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:textbox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Conducto de Pago">
                                        <ItemTemplate>
                                                <asp:textbox runat="server" ID="lbl_Conducto" Enabled="false" Text='<%# Eval("txt_desc_cond")   %>' Width="140px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:textbox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Medio Actual">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_MedioActual" Text='<%# Eval("Med_Pago_Act")   %>' Width="140px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Conducto Actual">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_ConductoActual" Text='<%# Eval("Cond_Act")   %>' Width="140px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
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

                    <div class="clear padding5"></div>
                    <asp:Label runat="server" ID="lbl_DetPagador" Text="" Font-Bold="true" Font-Size ="12px" ForeColor="DarkBlue"></asp:Label>
                    <asp:Panel runat="server" ID="pnlPagadorCuota" Width="100%" Height="230px" ScrollBars="Both">
                        <asp:GridView runat="server" ID="gvd_PagadorCuota" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" DataKeyNames="id_pv,cod_aseg,cod_ind_pagador,nro_cuota" >
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <Columns>
                                    <asp:TemplateField HeaderText="Cuota" ItemStyle-CssClass="ClaveCia">
                                        <ItemTemplate>
                                            <asp:linkbutton runat="server" ID="lnk_Cuota" CommandName="DetalleCuotaPagador" Text='<%# Eval("nro_cuota") %>' Width="50px" CssClass="Centro" Font-Bold="true" Font-Size="10px"></asp:linkbutton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Documento">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Documento" Enabled="false" Text='<%# Eval("Documento")   %>' Width="110px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Vencimiento">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Venc" Enabled="false" Text='<%# String.Format("{0:dd/MM/yyyy}", CDate(Eval("fec_venc")))  %>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Fecha Inicio">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_inicio" Enabled="false" Text='<%# String.Format("{0:dd/MM/yyyy}", CDate(Eval("inicio_vigencia")))  %>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Fecha Fin">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_fin" Enabled="false" Text='<%# String.Format("{0:dd/MM/yyyy}", CDate(Eval("fin_vigencia")))  %>'  Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Prima Neta">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_PrimaNeta" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_prima_me")))%>' Width="90px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Recargos">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Recargos" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_rec_fin_me")))%>' Width="90px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Der. Emision">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_DerEmision" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_der_emi_me")))%>' Width="110px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="I.V.A.">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_IVA" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_iva_me")))%>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Prima Total">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_PrimaTotal" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_premio_me")))%>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Imp. Art. 41">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_ImpArt41" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_decreto_me")))%>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="IVA Art. 41">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_IvaArt41" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_dto_me")))%>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Compensado">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Compensado" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_compensado")))%>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Pagos">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Pagos" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_pago_parcial")))%>' Width="90px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Cancelado">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Cancelado" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_pago_cancelado")))%>' Width="90px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Devuelto">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Devuelto" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_pago_devuelto")))%>' Width="90px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Pendiente">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Pendiente" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_pago_pendiente")))%>' Width="90px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Estado">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Estado" Enabled="false" Text='<%# Eval("Estado")   %>' Width="180px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Fecha Debito">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Debito" Enabled="false" Text='<%# String.Format("{0:dd/MM/yyyy}", CDate(Eval("fecha_debito")))  %>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
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

                    <div class="clear padding5"></div>
                    <asp:Label runat="server" ID="lbl_DetCuota" Text="" Font-Bold="true" Font-Size ="12px" ForeColor="DarkBlue"></asp:Label>
                    <asp:Panel runat="server" ID="pnlDetalleCuota" Width="100%" Height="120px" ScrollBars="Both">
                        <asp:GridView runat="server" ID="gvd_DetCuotaPagador" AutoGenerateColumns="false" ForeColor="#333333" GridLines="Horizontal"  ShowHeaderWhenEmpty="true" >
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <Columns>
                                    <asp:TemplateField HeaderText="Pago" ItemStyle-CssClass="ClaveCia">
                                        <ItemTemplate>
                                            <asp:label runat="server" ID="lbl_pago" CommandName="DetalleCuotaPagador" Text='<%# Eval("nro_correlativo") %>' Width="40px" CssClass="Centro" Font-Bold="true" Font-Size="10px"></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Transacción">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_transaccion"  Text='<%# Eval("nro_recibo")   %>' Width="110px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="[Anulación]">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_anulacion" Text='<%# Eval("nro_recibo_anula")  %>' Width="90px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Aplicación">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Aplicacion"  Text='<%# String.Format("{0:dd/MM/yyyy}", CDate(Eval("fec_cobranza")))  %>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Prima Cobrada">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_PrimaCob" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_prima")))%>'  Width="120px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Der. Póliza">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_DerPoliza" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_gastos_emision")))%>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Imp. IVA">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_ImpIVA" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_iva")))%>' Width="90px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Recargo Fin">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Recargo" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_recargo")))%>' Width="110px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Prima Total">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_PrimaTotal" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_premio_bruto")))%>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Fecha Pago">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_FechaPago" Text='<%# String.Format("{0:dd/MM/yyyy}", CDate(Eval("fec_real_pago")))  %>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Comisión Desc.">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_ComDesc" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_comis_normal_descon")))%>' Width="120px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Imp. Art. 41">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_ImpArt41" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_art41")))%>' Width="100px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Concepto">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_Concepto" Text='<%# Eval("text_caja") %>' Width="250px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Imp. Cambio">
                                        <ItemTemplate>
                                                <asp:label runat="server" ID="lbl_ImpCambio" Text='<%# String.Format("{0:#,#0.00}", CDbl(Eval("imp_cambio")))%>' Width="110px" CssClass="form-control" Font-Size="10px" Height="26px" ></asp:label>
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
                        <asp:Button runat="server" id="Button2" class="btn btn-danger" Text="Cerrar" data-dismiss="modal"  style="height:30px; width:80px;" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

