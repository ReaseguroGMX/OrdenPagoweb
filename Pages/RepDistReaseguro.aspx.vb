Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Partial Class Pages_RepDistReaseguro
    Inherits System.Web.UI.Page


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
End Class
