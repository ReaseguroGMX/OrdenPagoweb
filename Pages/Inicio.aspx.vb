
Partial Class Pages_Inicio
    Inherits System.Web.UI.Page

    Private Sub Pages_Inicio_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Master.Usuario = IIf(Session("Usuario") = "", "Inicia Sesión", Session("Usuario"))
            Master.cod_usuario = Session("cod_usuario")
            Master.cod_suc = Session("cod_suc")
            Master.cod_sector = Session("cod_sector")

            If Master.cod_usuario = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Redirect", "Redireccionar('Login.aspx');", True)
            End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Menu", "openNav();", True)
        End If
    End Sub
End Class
