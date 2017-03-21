Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Catalogo
    Private _Clave As String
    Private _Descripcion As String

    Public Sub Catalogo(ByVal clave As String, ByVal descripcion As String)
        Me.Clave = clave
        Me.Descripcion = descripcion
    End Sub

    Public Property Clave() As String
        Get
            Return _Clave
        End Get
        Set(ByVal value As String)
            _Clave = value
        End Set
    End Property

    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
        End Set
    End Property

    Public Function ObtieneCatalogo(ByVal Catalogo As String) As DataTable
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_CatalogosOP '" & Catalogo & "'"

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function

    Public Function ObtieneNumCuotas(ByVal id_pv As Integer) As DataTable
        Dim sCnn As String
        sCnn = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString

        Dim sSel As String = "spS_NumCuotas " & id_pv

        Dim da As SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlDataAdapter(sSel, sCnn)
        da.Fill(dt)

        Return dt
    End Function


End Class

