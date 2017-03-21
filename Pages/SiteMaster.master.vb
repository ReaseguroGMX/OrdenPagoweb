Imports System.Net
Imports System.Net.Dns
Partial Class SiteMaster
    Inherits System.Web.UI.MasterPage

    Public Property Usuario() As String
        Get
            Return lbl_Usuario.Text
        End Get
        Set(ByVal value As String)
            lbl_Usuario.Text = value
        End Set
    End Property

    Public Property cod_usuario() As String
        Get
            Return hid_codUsuario.Value
        End Get
        Set(ByVal value As String)
            hid_codUsuario.Value = value
        End Set
    End Property

    Public Property cod_suc() As Integer
        Get
            Return hid_codSuc.Value
        End Get
        Set(ByVal value As Integer)
            hid_codSuc.Value = value
        End Set
    End Property

    Public Property cod_sector() As Integer
        Get
            Return hid_codSector.Value
        End Get
        Set(ByVal value As Integer)
            hid_codSector.Value = value
        End Set
    End Property


    Public Property HostName() As String
        Get
            Return hid_HostName.Value
        End Get
        Set(ByVal value As String)
            hid_HostName.Value = value
        End Set
    End Property

    Public Property VisibleBtnCerrar() As Boolean
        Get
            Return btn_CerrarSesion.Visible
        End Get
        Set(ByVal value As Boolean)
            btn_CerrarSesion.Visible = value
        End Set
    End Property


    Private Sub btn_CerrarSesion_Click(sender As Object, e As ImageClickEventArgs) Handles btn_CerrarSesion.Click
        Dim ConsultaBD As ConsultaBD
        ConsultaBD = New ConsultaBD
        ConsultaBD.InsertaBitacora(ConsultaBD.ConsultaUsuarioNT(hid_codUsuario.Value), hid_HostName.Value, "Cierre de Sesión", "Cierre Exitoso")
        Session.Abandon()
        lbl_Usuario.Text = "Inicia Sesión"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Redirect", "Redireccionar('Login.aspx');", True)
    End Sub

    'Obtiene el Nombre de la Maquina del Cliente
    Public Sub GetHostNameCallBack(ByVal asyncResult As IAsyncResult)
        Dim userHostAddress As String = CStr(asyncResult.AsyncState)
        Dim hostEntry As IPHostEntry = EndGetHostEntry(asyncResult)

        Session.Add("HostName", hostEntry.HostName)
        hid_HostName.Value = hostEntry.HostName
    End Sub

    Private Sub SiteMaster_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'BeginGetHostEntry(Request.UserHostAddress, AddressOf GetHostNameCallBack, Request.UserHostAddress)
            Session.Add("HostName", "")
        End If
    End Sub
End Class

