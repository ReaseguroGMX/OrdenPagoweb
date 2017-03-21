<%@ Page Language="VB" AutoEventWireup="false" ClientIDMode="AutoID" MasterPageFile="SiteMaster.master" CodeFile="Login.aspx.vb" Inherits="Pages_Login" %>
<%@ MasterType VirtualPath="~/Pages/SiteMaster.master" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentMaster" Runat="Server">
    <script src="../Scripts/Login.js"></script>


    <asp:HiddenField runat="server" ID="hid_Mensaje" Value="" /> 
        
    <div style="padding-left:300px">
        <div class="panel-heading" style="width:400px; min-width:400px;  text-align:center;">
            <div class="panel-heading">
                <strong>INICIAR SESIÓN</strong>
            </div>
            <div class="panel-body">
                <div class="text-center">
                    <img class="profile-img" src ="../Images/Login.png" alt=""/>
                </div>
                <br />

                <div class="form-group ">
                    <div class="input-group">
                        <asp:label runat="server" class="col-md-1 control-label" Width="86px">Usuario</asp:label>
                        <span class="input-group-addon">
                            <i class="glyphicon glyphicon-user"></i>
                        </span>
                        <input id="Usuario" class="form-control Usuario" style="font-size:12px; width:150px; "  name="Usuario" type="text" autofocus="autofocus" />
                        <asp:HiddenField runat="server" ID="hid_usuario" Value="" />
                        </div>
                </div>

                <div class="form-group">
                    <div class="input-group"> 
                        <asp:label runat="server" class="col-md-1 control-label" Width="86px">Contraseña</asp:label>
                        <span class="input-group-addon">
                            <i class="glyphicon glyphicon-lock"></i>
                        </span>
                        <input id="Contraseña" class="form-control Contraseña" style="font-size:12px; width:150px;" name="Contraseña" type="password"  value="" />
                        <asp:HiddenField runat="server" ID="hid_contraseña" Value="" />
                    </div>
                </div>

                <div class="form-group">
                        <div id="messages"></div>
                </div>



                <asp:Button ID="btnAceptar" runat="server" Text="Ingresar" Width="300px"  class="btn btn-primary btn-block submit" />

                       
            </div>
        </div>
    </div>
            



    <div class="clear padding20"></div>

    <div id="inicio_eslogan" align="center">
        <p><span class="info_negritas">GMX Seguros</span> es una aseguradora 100% mexicana, comprometida con la seguridad y protección patrimonial, a través de productos innovadores con un gran respaldo y experiencia.</p>
        <h3 class="info_negritas_verdes">Juntos el riesgo es menor<sup>&reg;</sup></h3>
    </div>
    <div class="clear padding20"></div>

     <!-- Modal -->
    <div id="MensajeModal" style="width:400px; height:185px"  class="modalPoliza">
          <div class="modal-content">
               <div class="modal-header" style="height:40px">
                    <button type="button" class="close"  data-dismiss="modal">&times;</button>
               </div>

               <div class="modal-body" style="height:143px">
                   <asp:UpdatePanel runat="server" ID="upMensaje">
                       <ContentTemplate>
                           <div id="Mensaje" style="width:368px; height:80px"></div>
                           <div class="clear padding5"></div>
                            <div style="width:100%; text-align:right;">
                                 <button type="button" id="btn_CnlMensaje" class="btn btn-danger" data-dismiss="modal" style="height:30px; width:80px;">Cerrar</button>
                            </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
              </div>
          </div>
    </div>
</asp:Content>



        
 
        
